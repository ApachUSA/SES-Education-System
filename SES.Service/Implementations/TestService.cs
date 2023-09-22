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
	public class TestService : ITestService
	{

		private readonly IBaseRepository<Test> _testRepository;

		public TestService(IBaseRepository<Test> testRepository)
		{
			_testRepository = testRepository;
		}

		public async Task<BaseResponse<Test>> Create(Test model)
		{
			//will return model_ID????
			try
			{
				await _testRepository.Create(model);

				return BaseResponse<Test>.Success(model, "Тест створено");
			}
			catch (Exception e)
			{
				return BaseResponse<Test>.Error($"[Test Create] : {e.Message}");
			}
		}

		public async Task<BaseResponse<bool>> Delete(int id)
		{
			try
			{
				var test = await _testRepository.Get().FirstOrDefaultAsync(x => x.Test_ID == id);
				if(test == null) return BaseResponse<bool>.Fail(ResponseStatus.ItemNotFound, "Тест не знайдено");

				await _testRepository.Delete(test);

				return BaseResponse<bool>.Success(true, "Тест видалено");
			}
			catch (Exception e)
			{
				return BaseResponse<bool>.Error($"[Test Delete] : {e.Message}");
			}
		}

		public async Task<BaseResponse<List<Test>>> Get()
		{
			try
			{
				var tests = await _testRepository.Get()
					.Include(x => x.Questions)
					.Include(x => x.Position)
					.ToListAsync();

				if (tests == null) return BaseResponse<List<Test>>.Fail(ResponseStatus.ItemNotFound, "Тести не знайдено");

				return BaseResponse<List<Test>>.Success(tests, "");
			}
			catch (Exception e)
			{
				return BaseResponse<List<Test>>.Error($"[Test Get (List)] : {e.Message}");
			}
		}

		public async Task<BaseResponse<Test>> Get(int id)
		{
			try
			{
				var test = await _testRepository.Get()
					.Include(x => x.Questions)
					.ThenInclude(x => x.Options)
					.Include(x => x.Position)
					.FirstOrDefaultAsync(x => x.Test_ID == id);

				if (test == null) return BaseResponse<Test>.Fail(ResponseStatus.ItemNotFound, "Тест не знайдено");

				return BaseResponse<Test>.Success(test, "");
			}
			catch (Exception e)
			{
				return BaseResponse<Test>.Error($"[Test Get] : {e.Message}");
			}
		}

		public async Task<BaseResponse<Test>> Update(Test model)
		{
			try
			{
				await _testRepository.Update(model);

				return BaseResponse<Test>.Success(model, "");
			}
			catch (Exception e)
			{
				return BaseResponse<Test>.Error($"[Test Update] : {e.Message}");
			}
		}
	}
}
