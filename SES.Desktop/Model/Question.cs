using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Desktop.Model
{
	public class Question
	{
		public int Question_ID { get; set; }

		public required string Text { get; set; }

		public required int Test_ID { get; set; }

		public List<Option>? Options { get; set; }
	}
}
