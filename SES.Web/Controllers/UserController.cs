using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SES.Domain.Entity.AbstractE;
using SES.Domain.Entity.UserE;
using SES.Domain.Enum;
using SES.Domain.ViewModel;
using SES.Service.Implementations;
using SES.Service.Interfaces;
using SES.Web.Models;
using System.Diagnostics;
using System.Text.Json;

namespace SES.Web.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		private readonly IUserService _userService;
		private readonly IMapper _mapper;
		private readonly ISelectService _selectService;

		public UserController(IUserService userService, IMapper mapper, ISelectService selectService)
		{
			_userService = userService;
			_mapper = mapper;
			_selectService = selectService;
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
			var response = await _userService.Update(user);
			if (response.StatusCode == ResponseStatus.Success)
			{
				return Ok();
			}
			return BadRequest(response.Description);
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
				if (pib == null) return PartialView("_UserListPartialView", response.Data);

				pib = pib.ToLower();
				var findedUser = response.Data.Where(x => x.Name.ToLower().Contains(pib) || x.Surname.ToLower().Contains(pib) || x.Patronymic.ToLower().Contains(pib)).ToList();

				if (findedUser == null || !findedUser.Any())
				{
					return BadRequest("Користувачів не знайдено");
				}
				return PartialView("_UserListPartialView", findedUser);
			}
			return View("ErrorView", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int user_ID)
		{

			var response = await _userService.Delete(user_ID);
			if (response.StatusCode == ResponseStatus.Success)
			{
				return Ok(response.Description);
			}
			return BadRequest(response.Description);

		}

		[HttpGet]
		public async Task<IActionResult> Edit(int user_ID)
		{
			var response = await _userService.Get(user_ID);

			if (response.StatusCode == ResponseStatus.Success)
			{
				var register = _mapper.Map<RegisterVM>(response.Data);

				var rang = await _selectService.GetRangs();
				var position = await _selectService.GetPositions();
				var region =  _selectService.GetRegions();
				var role =  _selectService.GetRoles();

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


				ViewData["Region"] = new SelectList(selectRegion.OrderBy(x => x.Text), "Value", "Text", (int)response.Data.Department.City.Region_ID);
				ViewData["Role"] = new SelectList(selectRole, "Value", "Text");
				ViewData["Rang"] = new SelectList(rang.Data, "Rang_ID", "Name");
				ViewData["Position"] = new SelectList(position.Data, "Position_ID", "Name");
				ViewData["PositionImage"] = JsonSerializer.Serialize(position.Data.Select(x => new { ID = x.Position_ID, Image = x.Image }).ToList());

				return PartialView("_UserEditPartialView", register);
			}
			return View("ErrorView", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}



		[HttpPost]
		public async Task<IActionResult> Edit(RegisterVM model)
		{
			model.Role_ID_VM = model.Position_ID_VM switch
			{
				5 => Role.Teacher,
				6 => Role.Admin,
				_ => Role.Student,
			};
			var response = await _userService.Update(_mapper.Map<User>(model));
			if (response.StatusCode == ResponseStatus.Success)
			{
				return Ok();
			}
			return BadRequest(response.Description);

		}

	}
}
