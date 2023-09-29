using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SES.Domain.Entity.AbstractE;
using SES.Domain.Entity.UserE;
using SES.Domain.Enum;
using SES.Service.Implementations;
using SES.Service.Interfaces;
using SES.Web.Models;
using System.Diagnostics;

namespace SES.Web.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet]
		public IActionResult Index() => View();

		[HttpGet]
		public async Task<IActionResult> Profile()
		{
			var response = await _userService.Get(int.Parse(HttpContext.User.FindFirst("UserID").Value));
			if (response.StatusCode == ResponseStatus.Success)
			{
				return PartialView("_UserProfilePartialView", response.Data);
			}
			return View("ErrorView", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpPost]
		public async Task<IActionResult> Profile(User user)
		{
			if (ModelState.IsValid)
			{
				var response = await _userService.Update(user);
				if (response.StatusCode == ResponseStatus.Success)
				{
					return Ok();
				}
				ModelState.AddModelError("", response.Description);
			}
			return BadRequest(ModelState);

		}

		[HttpGet]
		public async Task<IActionResult> GetUsersList()
		{
			var response = await _userService.Get();
			if (response.StatusCode == ResponseStatus.Success)
			{
				return PartialView("_UserListPartialView", response.Data);
			}
			return View("ErrorView", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpGet]
		public async Task<IActionResult> GetUsersListByName(string pib)
		{
			var response = await _userService.Get();
			if (response.StatusCode == ResponseStatus.Success)
			{
				if(pib == null) return PartialView("_UserListPartialView", response.Data);

				pib = pib.ToLower();
				var findedUser = response.Data.Where(x => x.Name.ToLower().Contains(pib) || x.Surname.ToLower().Contains(pib) || x.Patronymic.ToLower().Contains(pib)).ToList();

				if (findedUser == null)
				{
					return BadRequest("Користувачів не знайдено");
				}
				return PartialView("_UserListPartialView", findedUser);
			}
			return View("ErrorView", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

	}
}
