using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Domain.ViewModel
{
	public class LoginVM
	{
		[DisplayName("Логін")]
		[Required(ErrorMessage = "Введіть логін")]

		public required string Login { get; set; }
		[DisplayName("Пароль")]
		[Required(ErrorMessage = "Введіть пароль")]
		public required string Password { get; set; }
	}
}
