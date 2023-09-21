using SES.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Domain.Response
{
	public class BaseResponse<T>
	{
		public string Description { get; set; }
		public ResponseStatus StatusCode { get; set; }
		public T Data { get; set; }

		public static BaseResponse<T> Success(T data)
		{
			return new BaseResponse<T> { StatusCode = ResponseStatus.Success, Data = data };
		}

		public static BaseResponse<T> Fail(ResponseStatus responseStatus, string description)
		{
			return new BaseResponse<T> { StatusCode = responseStatus, Description = description };
		}
	}

	
}
