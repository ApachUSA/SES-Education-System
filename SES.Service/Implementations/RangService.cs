using Microsoft.EntityFrameworkCore;
using SES.Domain.Entity.UserE;
using SES.Domain.Response;
using SES.Infrastructure.Interfaces;
using SES.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SES.Service.Implementations
{
	public class RangService : IRangService
	{
		private readonly IBaseRepository<Rang> _rangRepository;

		public RangService(IBaseRepository<Rang> rangRepository)
		{
			_rangRepository = rangRepository;
		}

		public async Task<BaseResponse<SelectList>> Get()
		{
			try
			{
				var rangs = await _rangRepository.Get().ToListAsync();
				return BaseResponse<SelectList>.Success(new SelectList(rangs, "Rang_ID", "Name"), "");
			}
			catch (Exception e)
			{
				return BaseResponse<SelectList>.Error($"[Rang Get] : {e.Message}");
			}
		}
	}
}
