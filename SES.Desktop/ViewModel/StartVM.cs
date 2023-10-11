using SES.Desktop.Model;
using System.Windows;
using System.Windows.Input;

namespace SES.Desktop.ViewModel
{
	public class StartVM : ViewModelBase
	{
		public string Test { get; set; }

		public ApiGetTestVM TestVM { get; set; }

		public ICommand StartingTest { get; set; }

		public StartVM(ApiGetTestVM test)
		{
			TestVM = test;
			StartingTest = new RelayCommand(param => StartTest());
		}

		private void StartTest()
		{
			((MainWindowVM)Application.Current.MainWindow.DataContext).NavigateToTesting(TestVM);
		}
	}
}
