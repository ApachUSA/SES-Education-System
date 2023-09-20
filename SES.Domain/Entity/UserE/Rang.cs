using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Domain.Entity.UserE
{
	public class Rang
	{
		public int Rang_ID{ get; set; }

		public required string Name { get; set; }

		public List<User>? Users { get; set; }
	}
}
