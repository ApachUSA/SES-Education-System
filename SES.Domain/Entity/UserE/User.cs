using SES.Domain.Entity.TestE;
using SES.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Domain.Entity.UserE
{
	public class User
	{
		public int User_ID { get; set; }

		public required string Surname { get; set;}

		public required string Name { get; set;}

		public required string Patronymic { get; set;}

		public required int Position_ID { get; set; }

		public Position? Position { get; set; }

		public required int Rang_ID { get; set; }

		public Rang? Rang { get; set; }

		public required string Login { get; set; }

		public required string Password { get; set; }

		public required Role Role_ID { get; set; }

		public List<Test_Result>? Test_Results { get; set; }

		public List<Test_History>? Test_Histories { get; set; }
	}
}
