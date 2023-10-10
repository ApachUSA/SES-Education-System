
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using SES.Domain.Entity.UserE;
using SES.Domain.Response;
using SES.Domain.ViewModel;
using SES.Service.Interfaces;
using System.Web.Http;
using System.Web.Http.Results;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SES.Web.Controllers
{
	
	public class TestApiController : ApiController
	{
		private readonly IUserService _userService;
		private readonly ITestService _testService;
		private readonly IMapper _mapper;

		public TestApiController(IUserService userService, IMapper mapper, ITestService testService)
		{
			_userService = userService;
			_mapper = mapper;
			_testService = testService;
		}

		[HttpGet]
		public async Task<BaseResponse<ApiGetTestVM>> GetTest([FromBody] LoginVM model)
		{
			var response = await _userService.Get();
			if (response.StatusCode == Domain.Enum.ResponseStatus.Success)
			{
				var user = response.Data.Where(x => x.Login == model.Login && x.Password == model.Password).FirstOrDefault();
				if (user == null)
				{
					return BaseResponse<ApiGetTestVM>.Fail(Domain.Enum.ResponseStatus.UserNotFound, "Неправельний логін або пароль ");
					
				}

				var test = await _testService.GetByPosition(user.Position_ID);
				if(test.StatusCode == Domain.Enum.ResponseStatus.Success)
				{
					test.Data?.Questions?.ForEach(x => {
						x.Test = null;
						x.Options?.ForEach(x => x.Question = null);
					});
					return BaseResponse<ApiGetTestVM>.Success(new ApiGetTestVM() { User = _mapper.Map<UserApiVM>(user), Test = test.Data.Questions }, " ");
				}
				return BaseResponse<ApiGetTestVM>.Fail(Domain.Enum.ResponseStatus.InternalServerError, test.Description);

				
			}
			return BaseResponse<ApiGetTestVM>.Fail(Domain.Enum.ResponseStatus.InternalServerError, response.Description); ;
		}

		
	}
}
