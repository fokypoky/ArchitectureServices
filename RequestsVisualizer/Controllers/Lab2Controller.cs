using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
		public async Task<IActionResult> GetReport(ReportRequest reportRequest)
		{
			#region Input validation

			if (String.IsNullOrEmpty(reportRequest.CourseTitle))
			{
				return View("Report", new Report() { Error = "Empty course title" });
			}

			#endregion

			var httpClient = new HttpClient();

			try
			{
				httpClient.BaseAddress = new Uri("http://localhost:1162/");
				httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {reportRequest.Token}");

				var response = await httpClient.GetAsync(
					$"api/lab2?startdate={reportRequest.StartDate}&enddate={reportRequest.EndDate}&coursetitle={reportRequest.CourseTitle}");
				response.EnsureSuccessStatusCode();

				var responseBody = await response.Content.ReadAsStringAsync();
				var lectureReports = JsonConvert.DeserializeObject<List<LectureReport>>(responseBody);

				var report = new Report()
				{
					CourseTitle = reportRequest.CourseTitle,
					Error = null,
					LectureReports = lectureReports
				};

				return View("Report", report);
			}
			catch (Exception ex)
			{
				return View("Report", new Report() { Error = ex.Message });
			}
		}

	}
}
