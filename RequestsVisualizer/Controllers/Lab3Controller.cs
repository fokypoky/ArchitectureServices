using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
		public async Task<IActionResult> GetReport(ReportRequest reportRequest)
		{
			#region Input validation

			if (String.IsNullOrEmpty(reportRequest.GroupNumber))
			{
				return View("Report", new Report() { Error = "Empty group number" });
			}

			#endregion

			var httpClient = new HttpClient();

			try
			{
				httpClient.BaseAddress = new Uri("http://gateway:80/");
				httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {reportRequest.Token}");

				var response = await httpClient.GetAsync(
					$"api/lab3?groupnumber={reportRequest.GroupNumber}&startdate={reportRequest.StartDate}&enddate={reportRequest.EndDate}");
				response.EnsureSuccessStatusCode();

				var responseBody = await response.Content.ReadAsStringAsync();
				var report = JsonConvert.DeserializeObject<Report>(responseBody);

				return View("Report", report);
			}
			catch (Exception ex)
			{
				return View("Report", new Report() { Error = ex.Message });
			}
		}

	}
}
