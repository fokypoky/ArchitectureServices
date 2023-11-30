using System.Text.Json;
using System.Text.Json.Serialization;
using lab2api.Infrastructure.Repositories;
using lab2api.Models;
using lab3api.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab2api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LecturesController : ControllerBase
	{
		[HttpGet]
		public ActionResult Get(DateTime startDate, DateTime endDate, string courseTitle)
		{
			using (var context = new ApplicationContext())
			{
				var course = context.Courses
					.Include(c => c.Lectures)
						.ThenInclude(l => l.Type)
					.FirstOrDefault(c => c.Title == courseTitle);
				if (course == null || course.Lectures.Count() == 0)
				{
					return NotFound("Course or lectures not found");
				}

				var report = new List<LectureReport>();

				foreach (var lecture in course.Lectures)
				{
					var lectureTimetables = context.Timetables
						.Where(t => t.LectureId == lecture.Id && t.Date >= startDate && t.Date <= endDate)
						.ToList();

					int maxListeners = 0;

					foreach (var timetable in lectureTimetables)
					{
						var qresult = new Neo4jRepository().GetVisitsCount(timetable.Date, timetable.LectureId)
							.GetAwaiter().GetResult().ToList();
						if (qresult[0] > maxListeners)
						{
							maxListeners = qresult[0];
						}
					}

					report.Add(new LectureReport() {
							Annotation = lecture.Annotation,
							ListenersCount = maxListeners,
							Type = lecture.Type.Type,
							Requirements = lecture.Requirements
					});
				}

				var response = JsonSerializer.Serialize(report, new JsonSerializerOptions() {ReferenceHandler = ReferenceHandler.Preserve});
				return Ok(report);
			}
		}
	}
}
