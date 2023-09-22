using Microsoft.EntityFrameworkCore;
using SES.Domain.Entity.AbstractE;
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
	public class CityService : ICityService
	{
		private readonly IBaseRepository<City> _cityRepository;

		public CityService(IBaseRepository<City> cityRepository)
		{
			_cityRepository = cityRepository;
		}

		public async Task<BaseResponse<SelectList>> Get(int region_ID)
		{
			try
			{
				var cities = await _cityRepository.Get().Where(x => (int)x.Region_ID == region_ID).ToListAsync();
				return BaseResponse<SelectList>.Success(new SelectList(cities, "City_ID", "Name"), "");
			}
			catch (Exception e)
			{
				return BaseResponse<SelectList>.Error($"[Cities Get] : {e.Message}");
			}
		}
	}
}
