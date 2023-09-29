using Microsoft.AspNetCore.Mvc;
using SES.Domain.Entity.TestE;
using SES.Domain.Enum;
using SES.Service.Implementations;
using SES.Service.Interfaces;
using SES.Web.Models;
using System.Diagnostics;

namespace SES.Web.Controllers
{
	public class TestResultController : Controller
	{
		private readonly ITestHistoryService _historyService;
		private readonly ITestResultService _resultService;

		public TestResultController(ITestResultService resultService, ITestHistoryService historyService)
		{
			_resultService = resultService;
			_historyService = historyService;
		}

		[HttpGet]
		public async Task<IActionResult> TestHistory()
		{
			var response = await _historyService.Get(int.Parse(HttpContext.User.FindFirst("UserID").Value));
			if (response.StatusCode == ResponseStatus.Success)
			{
				return PartialView("_HistoryPartialView", response.Data.OrderByDescending(x => x.Date).ToList());
			}
			return View("ErrorView", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpGet]
		public async Task<IActionResult> TestResult()
		{
			var response = await _resultService.Get(1);
			if (response.StatusCode == ResponseStatus.Success)
			{
				return PartialView("_ResultPartialView", response.Data.OrderByDescending(x => x.Date).ToList());
			}
			return View("ErrorView", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpGet]
		public async Task<IActionResult> TestResultFiltered(string positions, string date)
		{
			var response = await _resultService.Get(1);
			if (response.StatusCode == ResponseStatus.Success)
			{
				if (positions != null) response.Data = ApplyPositionFilter(response.Data, positions.Split(','));

				if (date != null) response.Data = ApplyDateFilter(response.Data, date);

				if (!response.Data.Any()) return BadRequest("Результатів не знайдено");

				return PartialView("_ResultPartialView", response.Data.OrderByDescending(x => x.Date).ToList());
			}
			return View("ErrorView", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}


		private static List<Test_Result> ApplyPositionFilter(List<Test_Result> results, string[]? selectedPosition) => results.Where(x => selectedPosition.Contains(x.User?.Position?.Name)).ToList();

		private static List<Test_Result> ApplyDateFilter(List<Test_Result> results, string date) => results.Where(x => x.Date == date).ToList();
	}
}
