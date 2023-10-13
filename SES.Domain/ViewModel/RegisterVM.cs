using SES.Domain.Entity.UserE;
using SES.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Domain.ViewModel
{
	public class RegisterVM
	{
		public int User_ID_VM { get; set; }

		[DisplayName("ПІБ")]
		[Required(ErrorMessage = "Введіть ПІБ.")]
		public required string Snp { get; set; }

		[DisplayName("Пожежна частина")]
		[Required(ErrorMessage = "Оберіть пожежну частину.")]
		public required int Department_ID_VM { get; set; }

		public Department? Department_VM { get; set; }

		[DisplayName("Посада")]
		[Required(ErrorMessage = "Оберіть посаду.")]
		public required int Position_ID_VM { get; set; }

		public Position? Position_VM { get; set; }

		[DisplayName("Ранг")]
		[Required(ErrorMessage = "Оберіть ранг.")]
		public required int Rang_ID_VM { get; set; }

		public Rang? Rang_VM { get; set; }

		[DisplayName("Логін")]
		[Required(ErrorMessage = "Введіть логін.")]
		[MaxLength(25, ErrorMessage = "Максимальна к-сть символів: 25.")]
		public required string Login_VM { get; set; }

		[DisplayName("Пароль")]
		[Required(ErrorMessage = "Введіть пароль.")]
		[MaxLength(10, ErrorMessage = "Максимальна к-сть символів: 10.")]
		public required string Password_VM { get; set; }

		[DisplayName("Роль")]
		[Required(ErrorMessage = "Оберіть роль.")]
		public required Role Role_ID_VM { get; set; }
	}
}
