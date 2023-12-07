using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
		public async Task<IActionResult> GetReport(ReportRequest reportRequest)
		{
			#region Input validation

			if (String.IsNullOrEmpty(reportRequest.Phrase))
			{
				return View("Report", new StudentAttendenceReport() { Error = "Empty phrase" });
			}
			
			#endregion

			var httpClient = new HttpClient();

			try
			{
				httpClient.BaseAddress = new Uri("http://localhost:1162/");
				httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {reportRequest.Token}");

				var response = await httpClient.GetAsync(
					$"api/lab1?startdate={reportRequest.StartDate}&enddate={reportRequest.EndDate}&phrase={reportRequest.Phrase}");
				response.EnsureSuccessStatusCode();

				var responseBody = await response.Content.ReadAsStringAsync();
				var studentAttends = JsonConvert.DeserializeObject<List<StudentAttend>>(responseBody);

				var report = new StudentAttendenceReport()
				{
					Attends = studentAttends,
					Error = null,
					Phrase = reportRequest.Phrase,
					StartDate = reportRequest.StartDate,
					EndDate = reportRequest.EndDate
				};

				return View("Report", report);
			}

			catch (Exception ex)
			{
				return View("Report", new StudentAttendenceReport() { Error = ex.Message });
			}
		}
	}
}
