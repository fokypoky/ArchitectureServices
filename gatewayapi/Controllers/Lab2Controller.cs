using gatewayapi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace gatewayapi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class Lab2Controller : ControllerBase
	{
		[HttpGet]
		public ActionResult Get(DateTime startDate, DateTime endDate, string courseTitle)
		{
			var request = $"api/lectures?startDate={startDate}&endDate={endDate}&courseTitle={courseTitle}";
			var response = new HttpRepository("http://api2:80/").GetData(request).GetAwaiter().GetResult();

			if (response.Status == 200)
			{
				return Ok(response.Response);
			}
			
			return BadRequest($"ERROR {response.Status}. {response.Response}");
		}
	}
}
