using SES.Domain.Entity.TestE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Domain.Entity.UserE
{
	public class Position
	{
		public int Position_ID { get; set; }

		public required string Name { get; set;}

		public string? Image { get; set;}

		public List<User>? Users { get; set; }

		public List<Test>? Tests { get; set; }
	}
}
