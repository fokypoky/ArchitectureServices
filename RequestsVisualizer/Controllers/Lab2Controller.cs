using Microsoft.AspNetCore.Mvc;

namespace RequestsVisualizer.Controllers
{
	public class Lab2Controller : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
