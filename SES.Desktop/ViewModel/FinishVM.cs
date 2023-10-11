using SES.Desktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SES.Desktop.ViewModel
{
	public class FinishVM : ViewModelBase
	{
		public UserApiVM User {  get; set; }

		public Test_Result Result { get; set; }

		public int QuestionCount { get; set; }

		public string Title { get; set; }

		public string SubTitle { get; set; }

		public string Image { get; set; }

		public ICommand CloseApp { get; set; }

		public FinishVM(UserApiVM user, Test_Result result, int question_count) 
		{ 
			User = user;
			Result = result;
			QuestionCount = question_count;

			CloseApp = new RelayCommand(param => Application.Current.Shutdown());

			if (result.Status == ResultStatus.Pass)
			{
				Title = "Вітаємо з успішно складеним тестом!";
				SubTitle = "Ваш результат щойно було надіслано старшому викладачу для подальшої передачі в Головне управління";
				Image = "\\Images\\SuccessEmoji.png";
			}
			else if (result.Status == ResultStatus.Fail)
			{
				Title = "На жаль, ви не набрали необхідної кількості балів для успішного складання тесту";
				SubTitle = "Ваш результат щойно було надіслано старшому викладачу. Зверніться до адміністратора для отримання подальших інструкцій.";
				Image = "\\Images\\FailEmoji.png";
			}
			else
			{
				Title = "На жаль, ви не набрали необхідної кількості балів для успішного складання тесту";
				SubTitle = "Ваш результат щойно було надіслано старшому викладачу. Ви можете перескласти провалене тестування. Дату перескладання вам повідомить викладач або адміністратор. Добре підготуйтесь, адже перескласти ви можете тільки 1 раз!";
				Image = "\\Images\\FailEmoji.png";
			}
		}
	}
}
