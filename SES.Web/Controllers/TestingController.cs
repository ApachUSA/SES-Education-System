using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SES.Domain.Entity.TestE;
using SES.Service.Interfaces;
using System.Linq;
using System.Text.Json;

namespace SES.Web.Controllers
{
	public class TestingController : Controller
	{
		private readonly ITestService _testService;
		private readonly ITestHistoryService _historyService;


		public TestingController(ITestService testService, ITestHistoryService historyService)
		{
			_testService = testService;
			_historyService = historyService;
		}

		[HttpGet]
		public async Task<IActionResult> GetTest(int test_ID)
		{
			var response = await _testService.Get(test_ID);
			if (response.StatusCode == Domain.Enum.ResponseStatus.Success)
			{
				return PartialView("_StartingPartialView", response.Data);
			}
			return BadRequest(response.Description);
		}

		[HttpGet]
		public async Task<IActionResult> TestStarting(int test_ID)
		{
			Random rng = new();
			HttpContext.Session.Clear();

			var response = await _testService.Get(test_ID);
			if (response.StatusCode == Domain.Enum.ResponseStatus.Success)
			{
				response.Data.Questions?.ForEach(x =>
				{
					x.Test = null;
					x.Options?.ForEach(d => d.Question = null);
				});

				var questions = response.Data.Questions?.OrderBy(x => rng.Next()).ToList();
				foreach (var question in questions)
				{
					question.Options = question.Options?.OrderBy(x => rng.Next()).ToList();
				}
				int i = 1;

				HttpContext.Session.SetString("Question.Count", questions.Count.ToString());
				HttpContext.Session.SetString("Test_ID", test_ID.ToString());
				HttpContext.Session.SetString("CurrentIndex", "1");
				foreach (var item in questions)
				{
					HttpContext.Session.SetString("Question[" + i++.ToString() + "]", JsonSerializer.Serialize(item));
				}

				ViewBag.Number = "1" + "/" + questions.Count.ToString();
				return PartialView("_TestingQusetionPartialView", questions.First());
			}
			return BadRequest(response.Description);
		}

		[HttpGet]
		public async Task<IActionResult> NextQuestion(string answer_option_ID, string status)
		{
			if (int.TryParse(HttpContext.Session.GetString("CurrentIndex"), out int current_index))
			{
				HttpContext.Session.SetString($"UserAnswer[{current_index}]", answer_option_ID);

				if (status == "Finish") return await FinishTesting();

				var question_string = HttpContext.Session.GetString($"Question[{++current_index}]");

				if (question_string != null)
				{
					var question = JsonSerializer.Deserialize<Question>(question_string);
					if (question != null)
					{

						HttpContext.Session.SetString("CurrentIndex", (current_index).ToString());
						ViewBag.Number = HttpContext.Session.GetString("CurrentIndex") + "/" + HttpContext.Session.GetString("Question.Count");
						ViewBag.Answer = HttpContext.Session.GetString($"UserAnswer[{current_index}]");
						return PartialView("_TestingQusetionPartialView", question);
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

						ViewBag.Number = HttpContext.Session.GetString("CurrentIndex") + "/" + HttpContext.Session.GetString("Question.Count");
						ViewBag.Answer = HttpContext.Session.GetString($"UserAnswer[{current_index}]");
						return PartialView("_TestingQusetionPartialView", question);
					}
					else return BadRequest("Не вдалось десерелізувати питання");
				}
				else return BadRequest("Не вдалось зчитати питання");

			}
			return BadRequest("Не вдалось зчитати номер питання");

		}

		[HttpGet]
		public async Task<IActionResult> FinishTesting()
		{
			var right_answers = new List<string>();
			var user_answers = new List<string>();
			var Question_Count = HttpContext.Session.GetString("Question.Count");

			if (Question_Count != null)
			{
				for (int i = 1; i <= int.Parse(Question_Count); i++)
				{
					var question_string = HttpContext.Session.GetString($"Question[{i}]");
					if (question_string != null)
					{
						var question = JsonSerializer.Deserialize<Question>(question_string);
						if (question != null)
						{
							var _ = question.Options?.Where(x => x.Right == true).Select(x => x.Option_ID.ToString());
							right_answers.Add(string.Join(',', _));
						}
						else return BadRequest("Не вдалось десерелізувати питання");
					}
					else return BadRequest("Не вдалось зчитати питання");
				}
			}
			else return BadRequest("Не вдалось зчитати кількість питань");

			for (int i = 1; i <= int.Parse(Question_Count); i++)
			{
				var answer_string = HttpContext.Session.GetString($"UserAnswer[{i}]");
				if (answer_string != null)
				{
					user_answers.Add(answer_string);
				}
				else break;

			}

			double totalScore = 0;

			for (int i = 0; i < user_answers.Count; i++)
			{
				var r_answer = right_answers[i].Split(',');
				var u_answer = user_answers[i].Split(',');
				double scorePerAnswer = 1.0 / r_answer.Length;
				var user_score = 0.0;
				if (u_answer.Length >= r_answer.Length)
				{
					for (int j = 0; j < u_answer.Length; j++)
					{
						if (r_answer.Contains(u_answer[j]))
						{
							user_score += scorePerAnswer;
						}
						else
						{
							user_score = 0;
							break;
						}

					}
				}

				totalScore += user_score;
			}

			var mark = 0;
			mark = (double)((totalScore * 100) / int.Parse(Question_Count)) switch
			{
				>= 90 => 5,
				>= 80 => 4,
				>= 70 => 3,
				>= 60 => 2,
				_ => 1,
			};

			ViewBag.QuestionCount = int.Parse(Question_Count);
			var Test_History = new Test_History()
			{
				Date = DateTime.Now.ToShortDateString(),
				Mark = mark,
				Right_Answer_Count = (int)totalScore,
				Status = mark <= 2 ? Domain.Enum.ResultStatus.Fail : Domain.Enum.ResultStatus.Pass,
				Test_ID = int.Parse(HttpContext.Session.GetString("Test_ID")),
				User_ID = int.Parse(HttpContext.User.FindFirst("UserID").Value)
			};

			var response = await _historyService.Create(Test_History);
			if (response.StatusCode == Domain.Enum.ResponseStatus.Success)
			{
				return PartialView("_TestResultPartialView", Test_History);
			}
			else return BadRequest(response.Description);
			
		}

	}
}
