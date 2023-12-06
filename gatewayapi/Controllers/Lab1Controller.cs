using Microsoft.AspNetCore.Http;
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
			return Ok("Lab 1 controller");
		}
	}
}
