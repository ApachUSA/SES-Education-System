using AutoMapper;
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
using static System.Net.Mime.MediaTypeNames;

namespace SES.Service.Implementations
{
	public class TestResultService : ITestResultService
	{
		private readonly IBaseRepository<Test_Result> _testResultRepository;
		private readonly ITestHistoryService _testHistoryService;
		private readonly IMapper _mapper;

		public TestResultService(IBaseRepository<Test_Result> testResultRepository, ITestHistoryService testHistoryService, IMapper mapper)
		{
			_testResultRepository = testResultRepository;
			_testHistoryService = testHistoryService;
			_mapper = mapper;
		}

		public async Task<BaseResponse<bool>> Create(Test_Result model)
		{
			try
			{

				await _testResultRepository.Create(model);
				await _testHistoryService.Create(_mapper.Map<Test_History>(model));

				return BaseResponse<bool>.Success(true, "Результат збережено");
			}
			catch (Exception e)
			{
				return BaseResponse<bool>.Error($"[Test_Result Create] : {e.Message}");
			}
		}

		public async Task<BaseResponse<bool>> Delete(int test_result_ID)
		{
			try
			{
				var test_result = await _testResultRepository.Get().FirstOrDefaultAsync(x => x.Test_Result_ID == test_result_ID);
				if (test_result == null) return BaseResponse<bool>.Fail(ResponseStatus.ItemNotFound, "Результат тестування не знайдено");

				await _testResultRepository.Delete(test_result);

				return BaseResponse<bool>.Success(true, "Результат тестування видалено");
			}
			catch (Exception e)
			{
				return BaseResponse<bool>.Error($"[Test_Result Delete] : {e.Message}");
			}
		}

		public async Task<BaseResponse<bool>> DeleteAll(int test_ID)
		{
			try
			{
				var test_result = await _testResultRepository.Get().Where(x => x.Test_ID == test_ID).ToListAsync();
				if (test_result == null) return BaseResponse<bool>.Fail(ResponseStatus.ItemNotFound, "Результатів тестування не знайдено");

				await _testResultRepository.Delete(test_result);

				return BaseResponse<bool>.Success(true, "Результати тестування видалено");
			}
			catch (Exception e)
			{
				return BaseResponse<bool>.Error($"[Test_Results DeleteAll] : {e.Message}");
			}
		}

		public async Task<BaseResponse<List<Test_Result>>> Get(int department_ID)
		{
			try
			{
				var test_result = await _testResultRepository.Get()
					.Include(x => x.User)
					.ThenInclude(x => x.Department)
					.Include(x => x.User)
					.ThenInclude(x => x.Position)
					.Where(x => x.User.Department_ID == department_ID)
					.ToListAsync();

				if (test_result == null) return BaseResponse<List<Test_Result>>.Fail(ResponseStatus.ItemNotFound, "Результатів тестування для вашого департаменту відсутні");


				return BaseResponse<List<Test_Result>>.Success(test_result, "");
			}
			catch (Exception e)
			{
				return BaseResponse<List<Test_Result>>.Error($"[Test_Results Get] : {e.Message}");
			}
		}

		public async Task<BaseResponse<Test_Result>> Get(int test_ID, int user_ID)
		{
			try
			{
				var test_result = await _testResultRepository.Get().FirstOrDefaultAsync(x => x.Test_ID == test_ID && x.User_ID == user_ID);
				if (test_result == null) return BaseResponse<Test_Result>.Fail(ResponseStatus.ItemNotFound, "Результатів тестування відсутні");


				return BaseResponse<Test_Result>.Success(test_result, "");
			}
			catch (Exception e)
			{
				return BaseResponse<Test_Result>.Error($"[Test_Results Get] : {e.Message}");
			}
		}

		public async Task<BaseResponse<bool>> Update(Test_Result model)
		{
			try
			{
				await _testResultRepository.Update(model);
				await _testHistoryService.Create(_mapper.Map<Test_History>(model));
				return BaseResponse<bool>.Success(true, "");
			}
			catch (Exception e)
			{
				return BaseResponse<bool>.Error($"[Test_Results Update] : {e.Message}");
			}
		}
	}
}
