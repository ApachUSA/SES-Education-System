using SES.Domain.Entity.AbstractE;
using SES.Domain.Response;
using SES.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Service.Interfaces
{
	public interface IAbstractService
	{
		Task<BaseResponse<bool>> Create(Abstracts model);

		Task<BaseResponse<List<Abstracts>>> GetAll(int area_ID);
		Task<BaseResponse<Abstracts>> Get(int abstract_ID);

		Task<BaseResponse<bool>> Update(Abstracts model);

		Task<BaseResponse<bool>> Delete(int abstract_ID);

	}
}
