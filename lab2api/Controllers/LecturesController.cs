using System.Text.Json;
using System.Text.Json.Serialization;
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
					.FirstOrDefault(c => c.Title == courseTitle);
				if (course == null || course.Lectures.Count() == 0)
				{
					return NotFound();
				}

				foreach (var lecture in course.Lectures)
				{

				}

				var response = JsonSerializer.Serialize(course, new JsonSerializerOptions() {ReferenceHandler = ReferenceHandler.Preserve});
				return Ok(response);
			}
		}
	}
}
