using SES.Domain.Entity.UserE;
using SES.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Domain.Entity.TestE
{
	public class Test
	{
		public int Test_ID { get; set; }

		public required int Year { get; set; }

		public required TestStatus Status { get; set; }

		public required int Position_ID { get; set; }

		public Position? Position { get; set; }

		public List<Question>? Questions { get; set; }

		public List<Test_Result>? Test_Results { get; set; }

		public List<Test_History>? Test_Histories { get; set; }

	}
}
