using Microsoft.AspNetCore.Mvc;
using RequestsVisualizer.Infrastructure.Repositories;
using RequestsVisualizer.Models.Lab1;

namespace RequestsVisualizer.Controllers
{
	public class Lab1Controller : Controller
	{

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public IActionResult GetReport(ReportRequest reportRequest)
		{
			#region Input validation

			if (String.IsNullOrEmpty(reportRequest.Phrase))
			{
				return View("Report", new StudentAttendenceReport() { Error = "Empty phrase" });
			}

			#endregion

			var request =
				$"api/lab1?startdate={reportRequest.StartDate}&enddate={reportRequest.EndDate}&phrase={reportRequest.Phrase}";
			var response =
				new HttpRepository<List<StudentAttend>>("http://gateway:80/", reportRequest.Token)
					.GetData(request).GetAwaiter().GetResult();

			if (response.Status != 200)
			{
				return View("Report", new StudentAttendenceReport() { Error = response.ErrorMessage });
			}

			var report = new StudentAttendenceReport()
			{
				Attends = response.Response,
				Error = null,
				Phrase = reportRequest.Phrase,
				StartDate = reportRequest.StartDate,
				EndDate = reportRequest.EndDate
			};

			return View("Report", report);
		}
	}
}
