using Microsoft.AspNetCore.Http;
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
			return Ok("Lab 2 controller");
		}
	}
}
