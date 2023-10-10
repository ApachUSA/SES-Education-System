using SES.Desktop.Model;
using SES.Desktop.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            CurrentPage = new View.Login();
		}

		// Метод для переключения на страницу Start
		public void NavigateToStart(ApiGetTestVM test)
		{
			CurrentPage = new Start(test);
		}
	}
}
