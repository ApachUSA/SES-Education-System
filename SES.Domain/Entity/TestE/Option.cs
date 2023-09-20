using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Domain.Entity.TestE
{
	public class Option
	{
		public int Option_ID { get; set; }

		public required string Text { get; set; }

		public required int Question_ID { get; set; }

		public Question? Question { get; set; }

		public Right_Answer? Right_Answer { get; set; }
	}
}
