using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SES.Domain.ViewModel;
using SES.Service.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using SES.Domain.Enum;
using System.Text.Json;

namespace SES.Web.Controllers
{
	public class AccountController : Controller
	{
		private readonly IAccountService _accountService;
		private readonly ISelectService _selectService;

		public AccountController(IAccountService accountService, ISelectService selectService)
		{
			_accountService = accountService;
			_selectService = selectService;
		}

		[HttpGet]
		public IActionResult Login() => View();

		[HttpPost]
		public async Task<IActionResult> Login(LoginVM model)
		{
			if (ModelState.IsValid)
			{
				var response = await _accountService.Login(model);
				if (response.StatusCode == Domain.Enum.ResponseStatus.Success)
				{
					await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new System.Security.Claims.ClaimsPrincipal(response.Data));
					return RedirectToAction("Index", "Home");

				}
				ModelState.AddModelError("", response.Description);
			}
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Register()
		{
			var rang = await _selectService.GetRangs();
			var position = await _selectService.GetPositions();
			var region = await _selectService.GetRegions();
			var role = await _selectService.GetRoles();

			List<SelectListItem> selectRegion = region.Data.Select(reg => new SelectListItem
			{
				Value = ((int)reg).ToString(),
				Text = reg.ToString()
			}).ToList();

			List<SelectListItem> selectRole = role.Data.Select(rol => new SelectListItem
			{
				Value = ((int)rol).ToString(),
				Text = rol.ToString()
			}).ToList();


			ViewData["Region"] = new SelectList(selectRegion.OrderBy(x => x.Text), "Value", "Text");
			ViewData["Role"] = new SelectList(selectRole, "Value", "Text");
			ViewData["Rang"] = new SelectList(rang.Data, "Rang_ID", "Name");
			ViewData["Position"] = new SelectList(position.Data, "Position_ID", "Name");
			ViewData["PositionImage"] = JsonSerializer.Serialize(position.Data.Select(x => new { ID = x.Position_ID, Image = x.Image }).ToList());
			return PartialView("/Views/User/_CreateUserPartialView.cshtml");
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterVM model)
		{
			model.Role_ID_VM = model.Position_ID_VM switch
			{
				5 => Role.Teacher,
				6 => Role.Admin,
				_ => Role.Student,
			};
			if (ModelState.IsValid)
			{
				var response = await _accountService.Register(model);
				if (response.StatusCode == ResponseStatus.Success)
				{
					return Ok();
				}
				ModelState.AddModelError("", response.Description);
			}
			return BadRequest(ModelState);
		}


		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public async Task<IActionResult> FillDepartmentOption(int region_ID)
		{
			var department = await _selectService.GetDepartments(region_ID);

			List<SelectListItem> selectListItems = new();
			foreach (var depart in department.Data)
			{
				selectListItems.Add(new SelectListItem
				{
					Value = depart.Department_ID.ToString(),
					Text = "№" + depart.Number + " - " + depart.Address
				});
			}
			return Ok(new SelectList(selectListItems, "Value", "Text"));
		}

	}
}
