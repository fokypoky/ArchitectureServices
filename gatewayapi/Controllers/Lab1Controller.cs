using gatewayapi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace gatewayapi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class Lab1Controller : ControllerBase
	{
		[HttpGet]
		public ActionResult Get(DateTime startDate, DateTime endDate, string phrase)
		{
			var request = $"api/Students?startDate={startDate}&endDate={endDate}&phrase={phrase}";
			var response = new HttpRepository("http://api1:80/").GetData(request).GetAwaiter().GetResult();

			if (response.Status == 200)
			{
				return Ok(response.Response);
			}

			return BadRequest($"ERROR {response.Status}. {response.Response}");
		}
	}
}
