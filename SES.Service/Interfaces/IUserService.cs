using SES.Domain.Entity.UserE;
using SES.Domain.Response;
using SES.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Service.Interfaces
{
	public interface IUserService
	{
		Task<BaseResponse<List<User>>> Get();

		Task<BaseResponse<User>> Get(int id);

		Task<BaseResponse<User>> Update(User model);

		Task<BaseResponse<bool>> Delete(int id);
	}
}
