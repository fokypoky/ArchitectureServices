using System.Diagnostics;
using lab1api.Infrastructure.Repositories;
using lab1api.Infrastructure.Repositories.Repositories;
using lab1api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab1api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StudentsController : ControllerBase
	{
		[HttpGet]
		public ActionResult Get(DateTime startDate, DateTime endDate, string phrase)
		{
			// TODO: добавить валидацию даты и фразы
			var hits = new ElasticSearchRepository().GetByPhrase(phrase);
			if (hits is null || hits?.Count == 0)
			{
				return NotFound();
			}

			using (var context = new ApplicationContext())
			{
				var lectureIds = context.LectureMaterials
					.Where(lm => hits.Contains(lm.MaterialsId))
					.Select(lm => lm.LectureId)
					.ToList();

				var students = new Neo4jRepository() // номера зачетных книжек
					.GetStudentsByLectureAndPeriod(
						lectureIds.Where(id => id != null).Select(id => id.Value).ToList(), 
						startDate, 
						endDate)
					.GetAwaiter()
					.GetResult();

				var result = context
					.Visits
					//.Join(context.Students, visit => visit.StudentId, student => students.Id)
					.Include(v => v.Student)
						.ThenInclude(s => s.Group)
					.Include(v => v.Lecture)
					.Where(v => v.Date >= startDate && v.Date <= endDate &&
					            students.Contains(v.Student.PassbookNumber) && lectureIds.Contains((int)v.LectureId))
					.GroupBy(v => new
					{
						StudentId = v.StudentId,
						GroupId = (int)v.Student.Group.Id,
						StudentName = v.Student.Name,
						StudentPassbook = v.Student.PassbookNumber,
						StudentGroup = v.Student.Group.Number
					})
					.Select(g => new
					{
						Name = g.Key.StudentName,
						Passbook = g.Key.StudentPassbook,
						Group = g.Key.StudentGroup,
						AttendencyPercent = (double)g.Count() / (double)context.Timetables
							.Count(t => t.Date >= startDate && t.Date <= endDate 
							                                && t.GroupId == g.Key.GroupId 
							                                && lectureIds.Contains(t.LectureId)) * 100
					})
					.OrderBy(v => v.AttendencyPercent)
					.Take(10)
					.ToList();
				
				return Ok(result);
			}
		}
	}
}
