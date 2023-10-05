using Microsoft.AspNetCore.Mvc;
using SES.Domain.Entity.TestE;
using SES.Service.Interfaces;
using System.Text.Json;

namespace SES.Web.Controllers
{
	public class TestingController : Controller
	{
		private readonly ITestService _testService;


		public TestingController(ITestService testService)
		{
			_testService = testService;
		
		}

		[HttpGet]
		public async Task<IActionResult> GetTest(int test_ID)
		{
			var response = await _testService.Get(test_ID);
			if(response.StatusCode == Domain.Enum.ResponseStatus.Success)
			{
				return PartialView("_StartingPartialView", response.Data);
			}
			return BadRequest(response.Description);
		}

		[HttpGet]
		public async Task<IActionResult> TestStarting(int test_ID)
		{
			Random rng = new();

			var response = await _testService.Get(test_ID);
			if (response.StatusCode == Domain.Enum.ResponseStatus.Success)
			{
				response.Data.Questions?.ForEach(x =>
				{
					x.Test = null;
					x.Options?.ForEach(d => d.Question = null);
				});

				var questions = response.Data.Questions?.OrderBy(x => rng.Next()).ToList();
				int i = 1;

				HttpContext.Session.SetString("Question.Count", questions.Count.ToString());
				HttpContext.Session.SetString("CurrentIndex", "2");
				foreach (var item in questions)
				{
					HttpContext.Session.SetString("Question[" + i++.ToString()+"]", JsonSerializer.Serialize(item));
				}
				 
				return PartialView("_TestingQusetionPartialView", questions.First());
			}
			return BadRequest(response.Description);
		}

		[HttpGet]
		public async Task<IActionResult> NextQuestion(string answer_option_ID)
		{
			if(int.TryParse(HttpContext.Session.GetString("CurrentIndex"), out int current_index))
			{
				HttpContext.Session.SetString($"UserAnswer[{current_index}]", answer_option_ID);

				var question_string = HttpContext.Session.GetString($"Question[{current_index}]");

				if(question_string != null)
				{
					var question = JsonSerializer.Deserialize<Question>(question_string);
					if(question != null)
					{
						HttpContext.Session.SetString("CurrentIndex", (++current_index).ToString());
						return PartialView( "_TestingQusetionPartialView", question);
					}
					else return BadRequest("Не вдалось десерелізувати питання");
				}
				else return BadRequest("Не вдалось зчитати питання");

			}
			return BadRequest("Не вдалось зчитати номер питання");

		}

		[HttpGet]
		public async Task<IActionResult> PreviousQuestion()
		{
			if (int.TryParse(HttpContext.Session.GetString("CurrentIndex"), out int current_index))
			{
				var question_string = HttpContext.Session.GetString($"Question[{--current_index}]");

				if (question_string != null)
				{
					var question = JsonSerializer.Deserialize<Question>(question_string);
					if (question != null)
					{
						HttpContext.Session.SetString("CurrentIndex", (current_index).ToString());

						ViewBag.Answer = HttpContext.Session.GetString($"UserAnswer[{current_index}]");
						return PartialView("_TestingQusetionPartialView", question);
					}
					else return BadRequest("Не вдалось десерелізувати питання");
				}
				else return BadRequest("Не вдалось зчитати питання");

			}
			return BadRequest("Не вдалось зчитати номер питання");

		}


	}
}
