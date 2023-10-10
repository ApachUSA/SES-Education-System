using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Desktop.Model
{
	public class ApiGetTestVM
	{
		public UserApiVM User { get; set; }

		public List<Question> Test { get; set; }
	}
}
