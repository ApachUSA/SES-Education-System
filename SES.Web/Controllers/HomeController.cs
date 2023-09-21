using Microsoft.AspNetCore.Mvc;
using SES.Infrastructure;
using SES.Web.Models;
using System.Diagnostics;

namespace SES.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly SES_DB_Context context;

		public HomeController(ILogger<HomeController> logger, SES_DB_Context context)
		{
			_logger = logger;
			this.context = context;
		}

		public IActionResult Index()
		{
			
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}