using SES.Domain.Response;
using SES.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SES.Service.Interfaces
{
	public interface IAccountService
	{
		Task<BaseResponse<bool>> Register(RegisterVM model);

		Task<BaseResponse<ClaimsIdentity>> Login(LoginVM model);

	}
}
