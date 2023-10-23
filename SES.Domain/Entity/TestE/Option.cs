using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Domain.Entity.TestE
{
	public class Option
	{
		public int Option_ID { get; set; }

		[DisplayName("Текст варіанту відповіді")]
		[Required(ErrorMessage = "Введіть текст варіанту відповіді.")]
		public required string Text { get; set; }

		public bool Right { get; set; } = false;

		public required int Question_ID { get; set; }

		public Question? Question { get; set; }

	}
}
