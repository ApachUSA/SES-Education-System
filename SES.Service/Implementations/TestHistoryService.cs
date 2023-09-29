using Microsoft.EntityFrameworkCore;
using SES.Domain.Entity.TestE;
using SES.Domain.Enum;
using SES.Domain.Response;
using SES.Infrastructure.Interfaces;
using SES.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Service.Implementations
{
	public class TestHistoryService : ITestHistoryService
	{
		private readonly IBaseRepository<Test_History> _testHistoryRepository;

		public TestHistoryService(IBaseRepository<Test_History> testHistoryRepository)
		{
			_testHistoryRepository = testHistoryRepository;
		}

		public async Task<BaseResponse<bool>> Create(Test_History model)
		{
			try
			{
				await _testHistoryRepository.Create(model);

				return BaseResponse<bool>.Success(true, "Результат збережено");
			}
			catch (Exception e)
			{
				return BaseResponse<bool>.Error($"[Test_History Create] : {e.Message}");
			}
			
		}

		public async Task<BaseResponse<List<Test_History>>> Get(int user_ID)
		{
			try
			{
				var test_history = await _testHistoryRepository.Get()
					.Include(x => x.Test)
					.ThenInclude(x => x.Position)
					.Where(x => x.User_ID == user_ID)
					.ToListAsync();

				if (test_history == null) return BaseResponse<List<Test_History>>.Fail(ResponseStatus.ItemNotFound, "Результатів не знайдено");

				return BaseResponse<List<Test_History>>.Success(test_history, "");
			}
			catch (Exception e)
			{
				return BaseResponse<List<Test_History>>.Error($"[Test_History Get (List)] : {e.Message}");
			}
		}
	}
}
