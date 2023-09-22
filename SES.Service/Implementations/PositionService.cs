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
	public class PositionService : IPositionService
	{
		private readonly IBaseRepository<Position> _positionRepository;

		public PositionService(IBaseRepository<Position> positionRepository)
		{
			_positionRepository = positionRepository;
		}

		public async Task<BaseResponse<SelectList>> Get()
		{
			try
			{
				var positions = await _positionRepository.Get().ToListAsync();
				return BaseResponse<SelectList>.Success(new SelectList(positions, "Position_ID", "Name"), "");
			}
			catch (Exception e)
			{
				return BaseResponse<SelectList>.Error($"[Position Get] : {e.Message}");
			}
		}
	}
}
