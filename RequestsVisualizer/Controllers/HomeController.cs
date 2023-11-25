using Microsoft.AspNetCore.Mvc;
using RequestsVisualizer.Models;
using System.Diagnostics;

namespace RequestsVisualizer.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Lab1()
		{
			return View();
		}

		public IActionResult Lab3()
		{
			return View();
		}

		public IActionResult Lab2()
		{
			return View();
		}
	}
}