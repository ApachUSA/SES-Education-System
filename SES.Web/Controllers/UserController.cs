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
		public async Task<IActionResult> Index()
		{
			var response = await _userService.Get(int.Parse(HttpContext.User.FindFirst("UserID").Value));
			if(response.StatusCode == ResponseStatus.Success)
			{
				return View(response.Data);
			}
			return View("ErrorView", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpPost]
		public async Task<IActionResult> Index(User user)
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
	}
}
