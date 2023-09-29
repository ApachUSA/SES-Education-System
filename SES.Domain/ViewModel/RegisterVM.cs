using SES.Domain.Entity.UserE;
using SES.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Domain.ViewModel
{
	public class RegisterVM
	{
		public int User_ID_VM { get; set; }

		public required string Snp { get; set; }

		public required int Department_ID_VM { get; set; }

		public Department? Department_VM { get; set; }

		public required int Position_ID_VM { get; set; }

		public Position? Position_VM { get; set; }

		public required int Rang_ID_VM { get; set; }

		public Rang? Rang_VM { get; set; }

		public required string Login_VM { get; set; }

		public required string Password_VM { get; set; }

		public required Role Role_ID_VM { get; set; }
	}
}
