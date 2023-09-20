using SES.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Domain.Entity.User
{
	public class City
	{
		public int City_ID { get; set; }

		public required string Name { get; set;}

		public required Region Region_ID { get; set;}

		public List<Department>? Departments { get; set; }
	}
}
