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
	public class OptionService : IOptionService
	{
		private readonly IBaseRepository<Option> _optionRepository;

		public OptionService(IBaseRepository<Option> optionRepository)
		{
			_optionRepository = optionRepository;
		}

		public async Task<BaseResponse<bool>> Create(Option model)
		{
			try
			{
				await _optionRepository.Create(model);

				return BaseResponse<bool>.Success(true, "Варіант відповіді створено");
			}
			catch (Exception e)
			{
				return BaseResponse<bool>.Error($"[Option Create] : {e.Message}");
			}
		}

		public async Task<BaseResponse<bool>> Create(List<Option> model)
		{
			try
			{
				await _optionRepository.Create(model);

				return BaseResponse<bool>.Success(true, "Варіанти відповідей створено");
			}
			catch (Exception e)
			{
				return BaseResponse<bool>.Error($"[Options Create] : {e.Message}");
			}
		}

		public async Task<BaseResponse<bool>> Delete(int id)
		{
			try
			{
				var option = await _optionRepository.Get().FirstOrDefaultAsync(x => x.Option_ID == id);
				if (option == null) return BaseResponse<bool>.Fail(ResponseStatus.ItemNotFound, "Варіант відповіді не знайдено");

				await _optionRepository.Delete(option);

				return BaseResponse<bool>.Success(true, "Варіант відповіді видалено");
			}
			catch (Exception e)
			{
				return BaseResponse<bool>.Error($"[Options Delete] : {e.Message}");
			}
		}
	}
}
