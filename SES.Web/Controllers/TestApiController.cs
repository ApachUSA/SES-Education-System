
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SES.Domain.Entity.TestE;
using SES.Domain.Entity.UserE;
using SES.Domain.Response;
using SES.Domain.ViewModel;
using SES.Service.Interfaces;
using System.Web.Http.Results;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SES.Web.Controllers
{

	public class TestApiController
	{
		private readonly IUserService _userService;
		private readonly ITestService _testService;
		private readonly ITestResultService _testResultService;
		private readonly IMapper _mapper;

		public TestApiController(IUserService userService, IMapper mapper, ITestService testService, ITestResultService testResultService)
		{
			_userService = userService;
			_mapper = mapper;
			_testService = testService;
			_testResultService = testResultService;
		}

		[HttpGet]
		public async Task<BaseResponse<ApiGetTestVM>> GetTest([FromQuery] LoginVM model)
		{
			Random rng = new();

			var response = await _userService.Get();
			if (response.StatusCode == Domain.Enum.ResponseStatus.Success)
			{
				var user = response.Data.Where(x => x.Login == model.Login && x.Password == model.Password).FirstOrDefault();
				if (user == null)
				{
					return BaseResponse<ApiGetTestVM>.Fail(Domain.Enum.ResponseStatus.UserNotFound, "Неправильний логін або пароль ");

				}

				var test = await _testService.GetByPosition(user.Position_ID);
				if (test.StatusCode == Domain.Enum.ResponseStatus.Success)
				{
					var test_result = await _testResultService.Get(test.Data.Test_ID, user.User_ID);

					test.Data?.Questions?.ForEach(x =>
					{
						x.Test = null;
						x.Options?.ForEach(x => x.Question = null);
					});

					var shuffleQuestions = test.Data.Questions?.OrderBy(x => rng.Next()).ToList();
					foreach (var question in shuffleQuestions)
					{
						question.Options = question.Options?.OrderBy(x => rng.Next()).ToList();
					}

					if (test_result.Data == null)
					{
						return BaseResponse<ApiGetTestVM>.Success(new ApiGetTestVM() { User = _mapper.Map<UserApiVM>(user), Test = shuffleQuestions }, " ");
					}
					else if (test_result.Data.Status == Domain.Enum.ResultStatus.Retake)
					{
						return BaseResponse<ApiGetTestVM>.Success(new ApiGetTestVM() { User = _mapper.Map<UserApiVM>(user), Test = shuffleQuestions, TestResult_ID = test_result.Data.Test_Result_ID, Status = test_result.Data.Status }, " ");
					}
					else
					{
						return BaseResponse<ApiGetTestVM>.Fail(Domain.Enum.ResponseStatus.InternalServerError, "Ви вже пройшли цей тест і маєте кінцевий результат.");
					}

				}
				return BaseResponse<ApiGetTestVM>.Fail(Domain.Enum.ResponseStatus.InternalServerError, test.Description);


			}
			return BaseResponse<ApiGetTestVM>.Fail(Domain.Enum.ResponseStatus.InternalServerError, response.Description); ;
		}

		[HttpPost]
		public async Task<BaseResponse<bool>> PostResult([FromBody] Test_Result model)
		{

			var resposneUpdate = await _testResultService.Update(model);
			if (resposneUpdate.StatusCode == Domain.Enum.ResponseStatus.Success)
			{
				return BaseResponse<bool>.Success(true, "Результат Оновлено");
			}
			return BaseResponse<bool>.Fail(Domain.Enum.ResponseStatus.InternalServerError, resposneUpdate.Description);
		}


	}
}
