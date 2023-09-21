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
		Task<BaseResponse<bool>> Delete(int id);
	}
}
