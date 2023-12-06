using Microsoft.AspNetCore.Mvc;
using RequestsVisualizer.Models;
using System.Diagnostics;

namespace RequestsVisualizer.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index() => View();

		public IActionResult Lab1() => RedirectToAction("Index", "Lab1");

		public IActionResult Lab2() => RedirectToAction("Index", "Lab2");

		public IActionResult Lab3() => RedirectToAction("Index", "Lab3");

	}
}