using SES.Desktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SES.Desktop.ViewModel
{
	public class LoginVM : ViewModelBase
	{

		public string? Login { get; set; } = "student";
		public string? Password { get; set; } = "student";
		public string? Error { get; set; }

		public ICommand LoginCommand { get; set; }


		public LoginVM()
		{
			LoginCommand = new RelayCommand(param => Auth());
		}

		private async void Auth()
		{
			using HttpClient client = new();
			try
			{
				var response = await client.GetAsync($"https://localhost:7277/TestApi/GetTest?Login={Login}&Password={Password}");
				response.EnsureSuccessStatusCode();

				var json = await response.Content.ReadAsStringAsync();
				var test = JsonSerializer.Deserialize<BaseResponse<ApiGetTestVM>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
				if(test == null)
				{
					Error = "При десеріалізації щось пішло не так. \n Спробуйте ще раз.";
					OnPropertyChanged(nameof(Error));
				}
				else if(test.StatusCode == ResponseStatus.Success)
				{
					((MainWindowVM)Application.Current.MainWindow.DataContext).NavigateToStart(test.Data);
				}
				else
				{
					Error = test.Description;
					OnPropertyChanged(nameof(Error));
				}
				OnPropertyChanged(nameof(Error));
			}
			catch (Exception ex)
			{
				Error = ex.Message;
				OnPropertyChanged(nameof(Error));
			}
			
			
		}

	}
}
