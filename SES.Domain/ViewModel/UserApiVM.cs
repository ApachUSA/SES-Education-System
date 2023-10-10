using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Domain.ViewModel
{
	public class UserApiVM
	{
		public int User_ID { get; set; }

		public string Snp { get; set; }

		public string? Position { get; set; }

		public string? Rang { get; set; }
	}
}
