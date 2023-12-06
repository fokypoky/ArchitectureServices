using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gatewayapi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class Lab2Controller : ControllerBase
	{
		private readonly HttpClient _httpClient;
		public Lab2Controller()
		{
			_httpClient = new HttpClient();
			_httpClient.BaseAddress = new Uri("http://api2:80/");
		}

		[HttpGet]
		public async Task<ActionResult> Get(DateTime startDate, DateTime endDate, string courseTitle)
		{
			try
			{
				var response = await _httpClient.GetAsync($"api/lectures?startDate={startDate}&endDate={endDate}&courseTitle={courseTitle}");
				response.EnsureSuccessStatusCode();
				string responseBody = await response.Content.ReadAsStringAsync();
				return Ok(responseBody);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
