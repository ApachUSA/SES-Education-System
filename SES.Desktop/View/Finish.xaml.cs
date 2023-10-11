using SES.Desktop.Model;
using SES.Desktop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SES.Desktop.View
{
	/// <summary>
	/// Логика взаимодействия для Finish.xaml
	/// </summary>
	public partial class Finish : Page
	{
		public Finish(UserApiVM user, Test_Result result, int question_count)
		{
			InitializeComponent();
			DataContext = new FinishVM(user, result, question_count);
		}
	}
}
