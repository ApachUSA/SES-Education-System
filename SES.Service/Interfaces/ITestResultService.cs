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
	public interface ITestResultService
	{
		Task<BaseResponse<List<Test_Result>>> GetTeacher(int department_ID);

		Task<BaseResponse<List<Test_Result>>> GetUser(int user_ID);

		Task<BaseResponse<bool>> Create(Test_Result model);

		Task<BaseResponse<Test_Result>> Update(Test_Result model);

		Task<BaseResponse<bool>> Delete(int test_result_ID);

		Task<BaseResponse<bool>> DeleteAll(int test_ID);

	}
}
