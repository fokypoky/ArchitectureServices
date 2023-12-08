using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RequestsVisualizer.Infrastructure.Repositories;
using RequestsVisualizer.Models.Lab3;

namespace RequestsVisualizer.Controllers
{
	public class Lab3Controller : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public IActionResult GetReport(ReportRequest reportRequest)
		{
			#region Input validation

			if (String.IsNullOrEmpty(reportRequest.GroupNumber))
			{
				return View("Report", new Report() { Error = "Empty group number" });
			}

			#endregion

			var request =
				$"api/lab3?groupnumber={reportRequest.GroupNumber}&startdate={reportRequest.StartDate}&enddate={reportRequest.EndDate}";
			var response = new HttpRepository<Report>("http://gateway:80/", reportRequest.Token).GetData(request)
				.GetAwaiter().GetResult();

			if (response.Status != 200)
			{
				return View("Report", new Report() { Error = response.ErrorMessage });
			}

			return View("Report", response.Response);
		}
	}
}
