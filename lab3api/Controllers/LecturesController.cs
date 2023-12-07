using lab3api.Infrastructure.Repositories;
using lab3api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
					.Include(g => g.Speciality)
					.Include(g => g.Students)
					.Include(g => g.Department)
						.ThenInclude(d => d.MainSpeciality)
					.FirstOrDefault(g => g.Number == groupNumber);

				if (group == null)
				{
					return NotFound("Group not found");
				}

				var courseTitles = new Neo4jRepository()
					.GetCoursesByGroupAndDepartment(group.Number, group.Department.Title,
						group.Department.MainSpeciality.Code)
					.GetAwaiter()
					.GetResult();

				var reports = new List<CourseReport>();

				foreach (var courseTitle in courseTitles)
				{
					var course = context.Courses
						.Include(c => c.Lectures)
							.ThenInclude(l => l.Type)
						.FirstOrDefault(c => c.Title == courseTitle);

					var courseReport = new CourseReport()
						{ CourseTitle = course.Title, LectureReports = new List<LectureReport>() };

					foreach (var lecture in course.Lectures)
					{
						var visits = context.Visits
							.Where(v => v.Date >= startDate && v.Date <= endDate &&
							            group.Students.Contains(v.Student) && v.LectureId == lecture.Id)
							.GroupBy(g => new
							{
								StudentId = g.Student.Id,
								StudentName = g.Student.Name,
								StudentPassbook = g.Student.PassbookNumber
							})
							.Select(g => new
							{
								StudentName = g.Key.StudentName,
								StudentPassbook = g.Key.StudentPassbook,
								ListenedHours = g.Count() * 2
							})
							.ToList();

						var plannedHours = context.Timetables
							.Count(t => t.GroupId == group.Id && t.Date >= startDate && t.Date <= endDate && t.LectureId == lecture.Id) * 2;

						var lectureReport = new LectureReport()
						{
							LectureAnnotation = lecture.Annotation, LectureRequirements = lecture.Requirements,
							PlannedHours = plannedHours, LectureType = lecture.Type.Type
						};

						var studentReports = new List<StudentReport>();
						foreach (var student in group.Students)
						{
							var visitedStudent =
								visits.FirstOrDefault(v => v.StudentPassbook == student.PassbookNumber);
							
							if (visitedStudent == null)
							{
								studentReports.Add(new StudentReport() {ListenedHours = 0, Name = student.Name, Passbook = student.PassbookNumber});
								continue;
							}

							studentReports.Add(new StudentReport()
							{
								ListenedHours = visitedStudent.ListenedHours, Name = visitedStudent.StudentName,
								Passbook = visitedStudent.StudentPassbook
							});
						}

						lectureReport.StudentReports = studentReports;
						courseReport.LectureReports.Add(lectureReport);
					}

					reports.Add(courseReport);
				}

				var finalReport = new Report()
				{
					CourseReports = reports,
					DepartmentTitle = group.Department.Title,
					GroupNumber = group.Number,
					StudentsCount = group.Students.Count,
					SpecialityCode = group.Speciality.Code
				};

				return Ok(JsonConvert.SerializeObject(finalReport));
			}
		}
	}
}
