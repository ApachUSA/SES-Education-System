using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SES.Domain.Entity.AbstractE;
using SES.Domain.Enum;
using SES.Service.Implementations;
using SES.Service.Interfaces;
using SES.Web.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace SES.Web.Controllers
{
	[Authorize]
	public class AbstractController : Controller
	{

		private readonly IAbstractService _abstractService;

		public AbstractController(IAbstractService abstractService)
		{
			_abstractService = abstractService;
		}

		public ActionResult Index()
		{
			return View();
		}

		public IActionResult GetAreaOfStudy()
		{
			return PartialView("_ChoosingAreaPartialView");
		}

		public async Task<IActionResult> GetThemes(int area_ID)
		{
			var response = await _abstractService.GetAll(area_ID);
			ViewData["AreaName"] = area_ID;
			if (response.StatusCode == ResponseStatus.Success)
			{
				return PartialView("_ThemesPartialView", response.Data);
			}
			return BadRequest(response.Description);
		}

		[HttpGet]
		public async Task<IActionResult> Create(int area_ID)
		{
			ViewData["AreaName"] = area_ID;
			return PartialView("_AbstractModalPartialView");

		}

		[HttpPost]
		public async Task<IActionResult> Create(Abstracts _abstract)
		{
			if (ModelState.IsValid)
			{
				var response = await _abstractService.Create(_abstract);
				if (response.StatusCode == ResponseStatus.Success)
				{
					ViewData["AreaName"] = _abstract.AreaOfStudy_ID;
					return Ok(_abstract.AreaOfStudy_ID);
				}
				ModelState.AddModelError("", response.Description);
			}
			return BadRequest(ModelState);

		}

		[HttpGet]
		public async Task<IActionResult> Edit(int abstract_ID)
		{
			var response = await _abstractService.Get(abstract_ID);
			if(response.StatusCode == ResponseStatus.Success)
			{
				return PartialView("_AbstractEditModalPartialView", response.Data);
			}
			return View("ErrorView",new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpPost]
		public async Task<IActionResult> Edit(Abstracts _abstract)
		{
			if (ModelState.IsValid)
			{
				var response = await _abstractService.Update(_abstract);
				if (response.StatusCode == ResponseStatus.Success)
				{
					return Ok(_abstract.AreaOfStudy_ID);
				}
				ModelState.AddModelError("", response.Description);
			}
			return BadRequest(ModelState);

		}

		[HttpPost]
		public async Task<IActionResult> Delete(int abstract_ID, int area_ID)
		{
			if (ModelState.IsValid)
			{
				var response = await _abstractService.Delete(abstract_ID);
				if (response.StatusCode == ResponseStatus.Success)
				{
					return Ok(area_ID);
				}
				ModelState.AddModelError("", response.Description);
			}
			return BadRequest(ModelState);

		}

	}
}
