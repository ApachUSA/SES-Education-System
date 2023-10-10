using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Desktop.Model
{
	public class Option
	{
		public int Option_ID { get; set; }

		public required string Text { get; set; }

		public bool Right { get; set; } = false;

		public required int Question_ID { get; set; }

		public Question? Question { get; set; }
	}
}
