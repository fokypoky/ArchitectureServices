using gatewayapi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace gatewayapi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class Lab3Controller : ControllerBase
	{
		public ActionResult Get(string groupNumber, DateTime startDate, DateTime endDate)
		{
			var request = $"api/lectures?startDate={startDate}&endDate={endDate}&groupNumber={groupNumber}";
			var response = new HttpRepository("http://api3:80/").GetData(request).GetAwaiter().GetResult();

			if (response.Status == 200)
			{
				return Ok(response.Response);
			}

			return BadRequest($"ERROR {response.Status}. {response.Response}");
		}
	}
}
