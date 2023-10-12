using SES.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Domain.Entity.AbstractE
{
	public class Abstracts
	{
		public int Abstract_ID { get; set; }

		[DisplayName("Назва теми")]
		[Required(ErrorMessage = "Введіть назву теми.")]
		public required string Name { get; set; }

		[DisplayName("Посилання")]
		[Required(ErrorMessage = "Введіть посилання.")]
		public required string URL { get; set; }

		public required AreaOfStudy AreaOfStudy_ID { get; set; }
	}
}
