using SES.Domain.Entity.UserE;
using SES.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Domain.Entity.TestE
{
	public class Test_History
	{
		public int Test_History_ID { get; set; }

		public required int Right_Answer_Count { get; set; }

		public required int Mark { get; set; }

		public required string Date { get; set; }

		public required ResultStatus Status { get; set; }

		public required int User_ID { get; set; }

		public User? User { get; set; }

		public required int Test_ID { get; set; }

		public Test? Test { get; set; }
	}
}
