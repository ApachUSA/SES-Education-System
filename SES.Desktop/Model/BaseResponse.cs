using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Desktop.Model
{
	public class BaseResponse<T>
	{
		public string Description { get; set; }
		public ResponseStatus StatusCode { get; set; }
		public T Data { get; set; }

	}
}
