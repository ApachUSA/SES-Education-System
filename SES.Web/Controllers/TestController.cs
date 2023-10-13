using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SES.Domain.Entity.TestE;
using SES.Domain.Enum;
using SES.Service.Implementations;
using SES.Service.Interfaces;
using SES.Web.Models;
using System.Diagnostics;
using System.Text.Json;

namespace SES.Web.Controllers
{
	public class TestController : Controller
	{
		private readonly ITestService _testService;
		private readonly ISelectService _selectService;
		private readonly IQuestionService _questionService;
		private readonly IOptionService _optionService;

		public TestController(ITestService testService, ISelectService selectService, IQuestionService questionService, IOptionService optionService)
		{
			_testService = testService;
			_selectService = selectService;
			_questionService = questionService;
			_optionService = optionService;
		}

		public IActionResult TestIndex() => View();

		[HttpGet]
		public async Task<IActionResult> GetCatalogFilterView()
		{
			var response = await _testService.Get();
			if (response.StatusCode == ResponseStatus.Success)
			{
				if (User.IsInRole("Student")) response.Data = response.Data.Where(x => x.Status == TestStatus.Підготовче).ToList();
				return PartialView("_TestFiltersPartialView", response.Data);
			}
			return View("ErrorView", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpGet]
		public async Task<IActionResult> GetCatalogFiltered(string year, string position, string type)
		{
			if (User.IsInRole("Student")) type = "Підготовче";
			var response = await _testService.Get();
			if (response.StatusCode == ResponseStatus.Success)
			{
				if (year != null) response.Data = ApplyYearFilter(response.Data, year.Split(','));

				if (position != null) response.Data = ApplyPositionFilter(response.Data, position.Split(','));

				if (type != null) response.Data = ApplyTypeFilter(response.Data, type.Split(','));


				if (!response.Data.Any()) return BadRequest("Результатів не знайдено");

				return PartialView("_TestCatalogPartialView", response.Data);
			}
			return View("ErrorView", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpGet]
		public async Task<IActionResult> TestCreate()
		{
			var position = await _selectService.GetPositions();
			ViewData["Position"] = new SelectList(position.Data, "Position_ID", "Name");
			return PartialView("_TestCreatePartialView");
		}

		[HttpPost]
		public async Task<IActionResult> TestCreate(Test model)
		{
			if (model.Status == TestStatus.Основне)
			{
				var test = await _testService.Get();
				if (test.StatusCode == ResponseStatus.Success)
				{
					if (test.Data.Any(x => x.Position_ID == model.Position_ID && x.Status == TestStatus.Основне))
					{
						return BadRequest("Основний тест для цієї посади вже існує.");
					}
				}
				else return BadRequest(test.Description);
			}

			var response = await _testService.Create(model);
			if (response.StatusCode == ResponseStatus.Success)
			{
				return Ok(new { Message = "Тест успішно створено", Test_ID = model.Test_ID });
			}
			return BadRequest(response.Description);



		}

		[HttpGet]
		public IActionResult QuestionCreate(int Test_ID)
		{
			ViewBag.Test_ID = Test_ID;
			return PartialView("_QuestionCreatePartialView");
		}

		[HttpPost]
		public async Task<IActionResult> UploadFile(IFormFile fileInput, int Test_ID)
		{
			if (fileInput != null && fileInput.Length > 0)
			{
				List<Question> questionList = new();
				try
				{
					using var streamReader = new StreamReader(fileInput.OpenReadStream());
					var fileContent = await streamReader.ReadToEndAsync();
					try
					{
						questionList = JsonSerializer.Deserialize<List<Question>>(fileContent);

					}
					catch (JsonException ex)
					{
						return BadRequest("Помилка десеріалізації JSON: " + ex.Message);
					}
					ViewBag.Test_ID = Test_ID;
					return PartialView("_QuestionCreatePartialView", questionList);
				}
				catch (Exception ex)
				{
					return BadRequest("Помилка під час читання файлу: " + ex.Message);
				}
			}
			else
			{
				return BadRequest("Виберіть файл для завантаження.");
			}
		}


		[HttpPost]
		public async Task<IActionResult> QuestionCreate(List<Question> model)
		{

			if (ModelState.IsValid)
			{
				var response = await _questionService.Create(model);
				if (response.StatusCode == ResponseStatus.Success)
				{
					return Ok(response.Description);
				}
				ModelState.AddModelError("", response.Description);
			}
			return BadRequest(ModelState);
		}

		[HttpGet]
		public async Task<IActionResult> TestEdit(int test_ID)
		{
			var position = await _selectService.GetPositions();
			ViewData["Position"] = new SelectList(position.Data, "Position_ID", "Name");

			var response = await _testService.Get(test_ID);
			if (response.StatusCode == ResponseStatus.Success)
			{
				return PartialView("_TestEditPartialView", response.Data);
			}
			return BadRequest(response.Description);
		}

		[HttpPost]
		public async Task<IActionResult> CreateOption(int question_ID)
		{
			var option = new Option() { Question_ID = question_ID, Text = string.Empty };
			var response = await _optionService.Create(option);
			if (response.StatusCode == ResponseStatus.Success)
			{
				return Ok(new { Message = "Варіант відповіді створено", Option_ID = option.Option_ID });
			}
			return BadRequest(response.Description);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteOption(int option_id)
		{
			var response = await _optionService.Delete(option_id);
			if (response.StatusCode == ResponseStatus.Success)
			{
				return Ok("Варіант відповіді видалено");
			}
			return BadRequest(response.Description);
		}

		[HttpPost]
		public async Task<IActionResult> CreateQuestion(int test_ID)
		{
			var question = new Question() { Test_ID = test_ID, Text = string.Empty };
			var response = await _questionService.Create(question);
			if (response.StatusCode == ResponseStatus.Success)
			{
				var response2 = await _optionService.Get(question.Question_ID);
				if (response2.StatusCode == ResponseStatus.Success)
				{
					return Ok(new { message = "Питання успішно створено", question_ID = question.Question_ID, option_ID = response2.Data[0].Option_ID, option_ID2 = response2.Data[1].Option_ID, test_id = test_ID });
				}

				return BadRequest($"Питання створено, але з варіантами проблема :( {response2.Description}");
			}
			return BadRequest(response.Description);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteQuestion(int question_id)
		{
			var response = await _questionService.Delete(question_id);
			if (response.StatusCode == ResponseStatus.Success)
			{
				return Ok("Питання видалено");
			}
			return BadRequest(response.Description);
		}

		[HttpPost]
		public async Task<IActionResult> TestEdit(Test model)
		{
			if (ModelState.IsValid)
			{
				var response = await _testService.Update(model);
				if (response.StatusCode == ResponseStatus.Success)
				{
					return Ok(response.Description);
				}
				ModelState.AddModelError("", response.Description);
			}
			return BadRequest(ModelState);
		}

		[HttpPost]
		public async Task<IActionResult> TestDelete(int test_ID)
		{
			var response = await _testService.Delete(test_ID);
			if (response.StatusCode == ResponseStatus.Success)
			{
				return Ok(response.Description);
			}
			return BadRequest(response.Description);
		}



		private static List<Test> ApplyYearFilter(List<Test> results, string[]? selectedYear) => results.Where(x => selectedYear.Contains(x.Year.ToString())).ToList();

		private static List<Test> ApplyPositionFilter(List<Test> results, string[]? selectedPosition) => results.Where(x => selectedPosition.Contains(x.Position.Name)).ToList();

		private static List<Test> ApplyTypeFilter(List<Test> results, string[]? selectedType) => results.Where(x => selectedType.Contains(x.Status.ToString())).ToList();
	}
}
