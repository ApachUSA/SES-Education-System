using SES.Domain.Entity.TestE;
using SES.Domain.Entity.UserE;
using SES.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Razor.Tokenizer.Symbols;

namespace SES.Service.Interfaces
{
	public interface ITestResultService
	{
		Task<BaseResponse<List<Test_Result>>> Get(int department_ID);

		Task<BaseResponse<Test_Result>> Get(int test_ID, int user_ID);

		Task<BaseResponse<bool>> Create(Test_Result model);

		Task<BaseResponse<bool>> Update(Test_Result model);

		Task<BaseResponse<bool>> Delete(int test_result_ID);

		Task<BaseResponse<bool>> DeleteAll(int test_ID);

	}
}
