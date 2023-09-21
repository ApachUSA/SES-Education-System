using SES.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SES.Service.Interfaces
{
	public interface IPositionService
	{
		Task<BaseResponse<SelectList>> Get();
	}
}
