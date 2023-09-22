using Microsoft.AspNetCore.Mvc;
using SES.Domain.ViewModel;
using SES.Infrastructure;
using SES.Service.Interfaces;
using SES.Web.Models;
using System.Diagnostics;

namespace SES.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly SES_DB_Context context;
		private readonly IAccountService _accountService;

		public HomeController(ILogger<HomeController> logger, SES_DB_Context context, IAccountService accountService)
		{
			_logger = logger;
			this.context = context;
			_accountService = accountService;
		}

		public IActionResult Index()
		{
			var response = _accountService.Register(new RegisterVM { Department_ID = 1, Login = "dwa", Password = "dwa", Position_ID = 1, Rang_ID = 1, Role_ID = Domain.Enum.Role.Teacher, Snp = "Войтенко Ігор Миколайович" });
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