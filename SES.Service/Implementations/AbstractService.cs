﻿using Microsoft.EntityFrameworkCore;
using SES.Domain.Entity.AbstractE;
using SES.Domain.Entity.UserE;
using SES.Domain.Enum;
using SES.Domain.Response;
using SES.Infrastructure.Interfaces;
using SES.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SES.Service.Implementations
{
	public class AbstractService : IAbstractService
	{
		private readonly IBaseRepository<Abstract> _abstractRepository;

		public AbstractService(IBaseRepository<Abstract> abstractRepository)
		{
			_abstractRepository = abstractRepository;
		}

		public async Task<BaseResponse<bool>> Create(Abstract model)
		{
			try
			{
				await _abstractRepository.Create(model);

				return BaseResponse<bool>.Success(true, "Конспект створено");
			}
			catch (Exception e)
			{
				return BaseResponse<bool>.Error($"[Abstract Create] : {e.Message}");
			}
		}

		public async Task<BaseResponse<bool>> Delete(int abstract_ID)
		{
			try
			{
				var _abstract = await _abstractRepository.Get().FirstOrDefaultAsync(x => x.Abstract_ID == abstract_ID);
				if(_abstract == null) return BaseResponse<bool>.Fail(ResponseStatus.ItemNotFound, "Лекцію не знайдено");

				await _abstractRepository.Delete(_abstract);
				return BaseResponse<bool>.Success(true, "Конспект видалено");
			}
			catch (Exception e)
			{
				return BaseResponse<bool>.Error($"[Abstract Delete] : {e.Message}");
			}
		}

		public async Task<BaseResponse<List<Abstract>>> Get(int area_ID)
		{
			try
			{
				var _abstracts = await _abstractRepository.Get().Where(x => (int)x.AreaOfStudy_ID == area_ID).ToListAsync();
				if(_abstracts == null) return BaseResponse<List<Abstract>>.Fail(ResponseStatus.ItemNotFound, "Лекції не знайдено");

				return BaseResponse<List<Abstract>>.Success(_abstracts, "");
			}
			catch (Exception e)
			{
				return BaseResponse<List<Abstract>>.Error($"[Abstract Get] : {e.Message}");
			}
		}

		public async Task<BaseResponse<bool>> Update(Abstract model)
		{
			try
			{
				await _abstractRepository.Update(model);
				return BaseResponse<bool>.Success(true, "Конспект оновлено");
			}
			catch (Exception e)
			{
				return BaseResponse<bool>.Error($"[Abstract Update] : {e.Message}");
			}
		}
	}
}
