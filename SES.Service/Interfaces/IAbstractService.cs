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
		Task<BaseResponse<bool>> Create(Abstract model);

		Task<BaseResponse<List<Abstract>>> Get(int area_ID);

		Task<BaseResponse<bool>> Update(int abstract_ID);

		Task<BaseResponse<bool>> Delete(int abstract_ID);

	}
}
