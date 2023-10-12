using SES.Desktop.Model;
using SES.Desktop.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SES.Desktop.ViewModel
{
	public class MainWindowVM : ViewModelBase
	{

		private Page currentPage;

		public Page CurrentPage
		{
			get { return currentPage; }
			set
			{
				currentPage = value;
				OnPropertyChanged(nameof(CurrentPage));
			}
		}

		public MainWindowVM()
		{
            // По умолчанию отображаем страницу Login
            CurrentPage = new Login();
		}

		public void NavigateToLogin()
		{
			CurrentPage = new Login();
			Application.Current.MainWindow.SizeToContent = SizeToContent.Manual;
			Application.Current.MainWindow.WindowState = WindowState.Normal;
		}

		// Метод для переключения на страницу Start
		public void NavigateToStart(ApiGetTestVM test)
		{
			CurrentPage = new Start(test);
			Application.Current.MainWindow.WindowState = WindowState.Maximized;
		}

		public void NavigateToTesting(ApiGetTestVM test)
		{
			CurrentPage = new Testing(test);
		}

		public void NavigateToFinish(UserApiVM user, Test_Result result, int question_count)
		{
			CurrentPage = new Finish(user, result, question_count);
		}
	}
}
