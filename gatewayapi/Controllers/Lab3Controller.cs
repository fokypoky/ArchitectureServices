using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gatewayapi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class Lab3Controller : ControllerBase
	{
		[HttpGet]
		public ActionResult Get(string groupNumber, DateTime startDate, DateTime endDate)
		{
			return Ok("Lab 3 controller");
		}
	}
}
