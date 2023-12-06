using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gatewayapi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class Lab3Controller : ControllerBase
	{
		private readonly HttpClient _httpClient;
		public Lab3Controller()
		{
			_httpClient = new HttpClient();
			_httpClient.BaseAddress = new Uri("http://api3:80/");
		}
		[HttpGet]
		public async Task<ActionResult> Get(string groupNumber, DateTime startDate, DateTime endDate)
		{
			try
			{
				var response = await _httpClient.GetAsync($"api/lectures?startDate={startDate}&endDate={endDate}&groupNumber={groupNumber}");
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
