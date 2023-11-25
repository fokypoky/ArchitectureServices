using System.Text.Json;
using System.Text.Json.Serialization;
using lab3api.Infrastructure.Repositories;
using lab3api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neo4jClient.Extensions;

namespace lab3api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LecturesController : ControllerBase
	{
		[HttpGet]
		public IActionResult Get(string groupNumber, DateTime startDate, DateTime endDate)
		{

			using (var context = new ApplicationContext())
			{
				var group = context.Groups
					.Include(g => g.Students)
					.Include(g => g.Department)
						.ThenInclude(d => d.MainSpeciality)
					.FirstOrDefault(g => g.Number == groupNumber);
				
				if (group is null)
				{
					return NotFound();
				}

				var students = (from student in @group.Students select student.Name).ToList();
				
				var groupCoursesTitles = new Neo4jRepository()
					.GetCoursesByGroupAndDepartment(group.Number, group.Department.Title,
						group.Department.MainSpeciality.Code)
					.GetAwaiter()
					.GetResult();

				var courses = context.Courses
					.Include(c => c.Lectures)
						.ThenInclude(l => l.Type)
					.Where(c => groupCoursesTitles.Contains(c.Title))
					.ToList();

				var reports = new List<Report>();
				var elasticsearchRepository = new ElasticSearchRepository();

				foreach (var course in courses)
				{
					var report = new Report();

					var visits = context.Visits
						.Where(v => v.Date >= startDate && v.Date <= endDate && course.Lectures.Contains(v.Lecture) &&
									group.Students.Contains(v.Student))
						.ToList();

					// определяю сколько часов запланированно
					var lecturesTimetable = context.Timetables
						.Where(t => t.Date >= startDate && t.Date <= endDate && t.GroupId == group.Id &&
						            course.Lectures.Contains(t.Lecture))
						.GroupBy(t => new
						{
							LectureId = t.LectureId
						}).Select(g => new
						{
							LectureId = g.Key.LectureId,
							PlannedHours = g.Count() * 2
						}).ToList();

					report.CourseReports.Add(new CourseReport()
					{
						Course = course, 
						Description = elasticsearchRepository.GetCourseById(course.DescriptionId).content
					});

					// при втором запуске падает...
					var lectureMaterials = context.LectureMaterials.Where(lm => course.Lectures.Contains(lm.Lecture));
					foreach (var lectureMaterial in lectureMaterials)
					{
						report.LectureReports.Add(new LectureReport()
						{
							Lecture = course.Lectures.First(l => l.Id == lectureMaterial.LectureId),
							Materials = elasticsearchRepository.GetLectureMaterialsById(lectureMaterial.MaterialsId).content,
							HoursCount = (from t in lecturesTimetable
								where t.LectureId == lectureMaterial.LectureId
								select t).Count() * 2
						});
					}

					foreach (var student in group.Students)
					{
						var studentReport = new StudentReport();

						var studentVisits = (from v in visits where v.StudentId == student.Id select v).ToList();
						foreach (var lecture in course.Lectures)
						{
							int hoursCount =
								(from v in studentVisits where v.LectureId == lecture.Id select v).Count() * 2;
							studentReport.VisitsReports.Add(new VisitsReport(){ LectureId = lecture.Id, HoursCount = hoursCount});
						}

						report.StudentReports.Add(studentReport);
					}

					reports.Add(report);
				}

				var jsonString = JsonSerializer.Serialize(reports, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.Preserve });
				return Ok(jsonString);
			}
		}
	}
}
