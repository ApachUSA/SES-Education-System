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
	public class DepartmentService : IDepartmentService
	{
		private readonly IBaseRepository<Department> _departmentRepository;

		public DepartmentService(IBaseRepository<Department> departmentRepository)
		{
			_departmentRepository = departmentRepository;
		}

		public async Task<BaseResponse<SelectList>> Get(int city_ID)
		{
			try
			{
				var departments = await _departmentRepository.Get().Where(x => x.City_ID == city_ID).ToListAsync();
				return BaseResponse<SelectList>.Success(new SelectList(departments, "Department_ID", "Number"), "");
			}
			catch (Exception e)
			{
				return BaseResponse<SelectList>.Error($"[Department Get] : {e.Message}");
			}
		}
	}
}
