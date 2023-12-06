using Microsoft.AspNetCore.Mvc;

namespace RequestsVisualizer.Controllers
{
	public class Lab3Controller : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
