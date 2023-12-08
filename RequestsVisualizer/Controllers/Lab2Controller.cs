using Microsoft.AspNetCore.Mvc;
using RequestsVisualizer.Infrastructure.Repositories;
using RequestsVisualizer.Models.Lab2;

namespace RequestsVisualizer.Controllers
{
	public class Lab2Controller : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public IActionResult GetReport(ReportRequest reportRequest)
		{
			#region Input validation

			if (String.IsNullOrEmpty(reportRequest.CourseTitle))
			{
				return View("Report", new Report() { Error = "Empty course title" });
			}

			#endregion

			var request =
				$"api/lab2?startdate={reportRequest.StartDate}&enddate={reportRequest.EndDate}&coursetitle={reportRequest.CourseTitle}";
			var response = new HttpRepository<List<LectureReport>>("http://gateway:80/", reportRequest.Token)
				.GetData(request).GetAwaiter().GetResult();

			if (response.Status != 200)
			{
				return View("Report", new Report() { Error = response.ErrorMessage });
			}

			var report = new Report()
			{
				CourseTitle = reportRequest.CourseTitle,
				Error = null,
				LectureReports = response.Response
			};
			
			return View("Report", report);
		}
	}
}
