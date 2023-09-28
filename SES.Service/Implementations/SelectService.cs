using Microsoft.EntityFrameworkCore;
using SES.Domain.Entity.UserE;
using SES.Domain.Enum;
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
	public class SelectService : ISelectService
	{
		private readonly IBaseRepository<Rang> _rangRepository;
		private readonly IBaseRepository<Position> _positionRepository;
		private readonly IBaseRepository<Department> _departmentRepository;
		private readonly IBaseRepository<City> _cityRepository;

		public SelectService(IBaseRepository<Rang> rangRepository, IBaseRepository<Position> positionRepository, IBaseRepository<Department> departmentRepository, IBaseRepository<City> cityRepository)
		{
			_rangRepository = rangRepository;
			_positionRepository = positionRepository;
			_departmentRepository = departmentRepository;
			_cityRepository = cityRepository;
		}

		public async Task<BaseResponse<List<City>>> GetCities(int region_ID)
		{
			try
			{
				var cities = await _cityRepository.Get().Where(x => (int)x.Region_ID == region_ID).ToListAsync();
				//return BaseResponse<SelectList>.Success(new SelectList(cities, "City_ID", "Name"), "");
				return BaseResponse<List<City>>.Success(cities, "");
			}
			catch (Exception e)
			{
				return BaseResponse<List<City>>.Error($"[Cities Get] : {e.Message}");
			}
		}

		public async Task<BaseResponse<List<Department>>> GetDepartments(int region_ID)
		{
			try
			{

				var departments = await _departmentRepository.Get().Where(x => x.City.Region_ID == (Region)region_ID).ToListAsync();
				return BaseResponse<List<Department>>.Success(departments, "");
			}
			catch (Exception e)
			{
				return BaseResponse<List<Department>>.Error($"[Department Get] : {e.Message}");
			}
		}

		public async Task<BaseResponse<List<Position>>> GetPositions()
		{
			try
			{
				var positions = await _positionRepository.Get().ToListAsync();
				return BaseResponse<List<Position>>.Success(positions, "");
			}
			catch (Exception e)
			{
				return BaseResponse<List<Position>>.Error($"[Position Get] : {e.Message}");
			}
		}

		public async Task<BaseResponse<List<Rang>>> GetRangs()
		{
			try
			{
				var rangs = await _rangRepository.Get().ToListAsync();
				return BaseResponse<List<Rang>>.Success(rangs, "");
			}
			catch (Exception e)
			{
				return BaseResponse<List<Rang>>.Error($"[Rang Get] : {e.Message}");
			}
		}

		public async Task<BaseResponse<Region[]>> GetRegions()
		{
			try
			{
				var regions = (Region[])Enum.GetValues(typeof(Region));
				return BaseResponse<Region[]>.Success(regions, "");
			}
			catch (Exception e)
			{
				return BaseResponse<Region[]>.Error($"[Region Get] : {e.Message}");
			}
		}

		public async Task<BaseResponse<Role[]>> GetRoles()
		{
			try
			{
				var roles = (Role[])Enum.GetValues(typeof(Role));
				return BaseResponse<Role[]>.Success(roles, "");
			}
			catch (Exception e)
			{
				return BaseResponse<Role[]>.Error($"[Role Get] : {e.Message}");
			}
		}
	}
}
