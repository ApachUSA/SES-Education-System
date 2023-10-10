using SES.Domain.Entity.TestE;
using SES.Domain.Entity.UserE;
using SES.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Service.Interfaces
{
	public interface ITestService
	{

		Task<BaseResponse<List<Test>>> Get();

		Task<BaseResponse<Test>> Get(int id);

		Task<BaseResponse<Test>> GetByPosition(int position_ID);

		Task<BaseResponse<Test>> Create(Test model);

		Task<BaseResponse<Test>> Update(Test model);

		Task<BaseResponse<bool>> Delete(int id);
	}
}
