using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Domain.Entity.TestE
{
	public class Right_Answer
	{
		public int Right_Answer_ID { get; set; }

		public required int Question_ID { get; set; }

		public required int Option_ID { get; set; }

		public Question? Question { get; set; }

		public Option? Option { get; set; }
	}
}
