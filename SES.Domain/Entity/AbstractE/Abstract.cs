using SES.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Domain.Entity.AbstractE
{
	public class Abstract
	{
		public int Abstract_ID { get; set; }

		public required string Name { get; set; }

		public required string URL { get; set; }

		public required AreaOfStudy AreaOfStudy_ID { get; set; }
	}
}
