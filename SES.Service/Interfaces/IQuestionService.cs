using SES.Domain.Entity.TestE;
using SES.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Service.Interfaces
{
	public interface IQuestionService
	{
		Task<BaseResponse<List<Question>>> Get(int test_ID);

		Task<BaseResponse<bool>> Create(List<Question> model);

		Task<BaseResponse<bool>> Create(Question model);

		Task<BaseResponse<bool>> Delete(int id);
	}
}
