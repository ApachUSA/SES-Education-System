using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Domain.Entity.UserE
{
	public class Department
	{
		public int Department_ID { get; set; }

		public int Number { get; set; }

		public string? Address { get; set; }

		public required int City_ID { get; set; }

		public City? City { get; set; }

		public List<User>? Users { get; set; }
	}
}
