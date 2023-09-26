using SES.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Domain.Entity.AbstractE
{
	public class Abstracts
	{
		public int Abstract_ID { get; set; }

		public required string Name { get; set; }

		public required string URL { get; set; }

		public required AreaOfStudy AreaOfStudy_ID { get; set; }
	}
}
