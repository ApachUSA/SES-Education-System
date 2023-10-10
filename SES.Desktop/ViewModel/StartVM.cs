using SES.Desktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Desktop.ViewModel
{
	public class StartVM : ViewModelBase
	{
		public string Test { get; set; }

		public ApiGetTestVM TestVM { get; set; }

		public StartVM(ApiGetTestVM test)
		{
			TestVM = test;
		}
	}
}
