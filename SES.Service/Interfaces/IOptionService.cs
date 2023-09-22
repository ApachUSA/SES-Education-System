using SES.Domain.Entity.TestE;
using SES.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Service.Interfaces
{
	public interface IOptionService
	{
		Task<BaseResponse<bool>> Create(Option model);

		Task<BaseResponse<bool>> Create(List<Option> model);

		Task<BaseResponse<bool>> Delete(int id);
	}
}
