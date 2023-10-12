using SES.Domain.Entity.TestE;
using SES.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Domain.Entity.UserE
{
	public class User
	{
		public int User_ID { get; set; }

		[DisplayName("Прізвище")]
		[Required(ErrorMessage = "Введіть прізвище.")]
		public required string Surname { get; set;}

		[DisplayName("Ім'я")]
		[Required(ErrorMessage = "Введіть ім'я.")]
		public required string Name { get; set;}

		[DisplayName("По батькові")]
		[Required(ErrorMessage = "Введіть по батькові.")]
		public required string Patronymic { get; set;}

		[DisplayName("Пожежна частина")]
		[Required(ErrorMessage = "Оберіть пожежну частину.")]
		public required int Department_ID { get; set; }

		public Department? Department { get; set; }

		[DisplayName("Посада")]
		[Required(ErrorMessage = "Оберіть посаду.")]
		public required int Position_ID { get; set; }

		public Position? Position { get; set; }

		[DisplayName("Ранг")]
		[Required(ErrorMessage = "Оберіть ранг.")]
		public required int Rang_ID { get; set; }

		public Rang? Rang { get; set; }

		[DisplayName("Логін")]
		[Required(ErrorMessage = "Введіть логін.")]
		public required string Login { get; set; }

		[DisplayName("Пароль")]
		[Required(ErrorMessage = "Введіть пароль.")]
		public required string Password { get; set; }

		[DisplayName("Підтвердження паролю")]
		[Required(ErrorMessage = "Підтвердіть пароль.")]
		[Compare("Password", ErrorMessage = "Паролі не співпадають.")]
		public required string PasswordConfirm { get; set; }

		public required Role Role_ID { get; set; }

		public List<Test_Result>? Test_Results { get; set; }

		public List<Test_History>? Test_Histories { get; set; }
	}
}
