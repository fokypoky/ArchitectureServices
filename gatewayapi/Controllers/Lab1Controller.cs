using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace gatewayapi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class Lab1Controller : ControllerBase
	{
		private readonly HttpClient _httpClient;
		public Lab1Controller()
		{
			_httpClient = new HttpClient();
			_httpClient.BaseAddress = new Uri("http://localhost:6212/");
		}

		[HttpGet]
		public async Task<ActionResult> Get(DateTime startDate, DateTime endDate, string phrase)
		{
			try
			{
				var response = await _httpClient.GetAsync($"api/Students?startDate={startDate}&endDate={endDate}&phrase={phrase}");
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
