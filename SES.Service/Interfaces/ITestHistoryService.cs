using SES.Domain.Entity.TestE;
using SES.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Service.Interfaces
{
	public interface ITestHistoryService
	{

		Task<BaseResponse<List<Test_History>>> Get(int user_ID);

		Task<BaseResponse<bool>> Create(Test_History model);
	}
}
