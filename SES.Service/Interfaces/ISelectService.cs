﻿using SES.Domain.Entity.UserE;
using SES.Domain.Enum;
using SES.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SES.Service.Interfaces
{
	public interface ISelectService
	{
		Task<BaseResponse<List<Rang>>> GetRangs();
		Task<BaseResponse<List<Department>>> GetDepartments(int region_ID);
		Task<BaseResponse<List<City>>> GetCities(int region_ID);
		Task<BaseResponse<List<Position>>> GetPositions();
		Task<BaseResponse<Region[]>> GetRegions();
		Task<BaseResponse<Role[]>> GetRoles();
	}
}
