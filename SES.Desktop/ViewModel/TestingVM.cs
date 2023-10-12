using SES.Desktop.Model;
using SES.Desktop.View;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SES.Desktop.ViewModel
{
	public class TestingVM : ViewModelBase
	{

		public Visibility PreviousButtonVisibility { get; set; } = Visibility.Collapsed;
		public Visibility NextButtonVisibility { get; set; }
		public Visibility FinishButtonVisibility { get; set; } = Visibility.Collapsed;

		private DispatcherTimer timer;
		public TimeSpan CurrentTime { get; set; }

		public ApiGetTestVM TestVM { get; set; }

		public int CurrentIndex { get; set; } = 1;

		public List<Question> UserQuestions { get; set; }

		public Question CurrentQuestion
		{
			get { return UserQuestions[CurrentIndex - 1]; }
			set { UserQuestions[CurrentIndex - 1] = value; }
		}


		public ICommand NextQuestion { get; set; }
		public ICommand PreviousQuestion { get; set; }
		public ICommand FinishTesting { get; set; }

		public TestingVM(ApiGetTestVM test)
		{
			TestVM = test;

			CopyQuestion();

			NextQuestion = new RelayCommand(param => Next());
			PreviousQuestion = new RelayCommand(param => Prev());
			FinishTesting = new RelayCommand(param => Finish());

			TimerStart();

			Application.Current.MainWindow.Closing += MainWindow_Closing;


		}

		private void Next()
		{
			CurrentIndex++;
			CheckButtonVisibility();
			OnPropertyChanged(nameof(CurrentQuestion));
			OnPropertyChanged(nameof(CurrentIndex));

		}

		private void Prev()
		{
			CurrentIndex--;
			CheckButtonVisibility();
			OnPropertyChanged(nameof(CurrentQuestion));
			OnPropertyChanged(nameof(CurrentIndex));
		}

		private async void Finish()
		{
			timer.Stop();

			int totalPoint = 0;
			for (int i = 0; i < UserQuestions.Count; i++)
			{
				if (AreOptionsEqual(UserQuestions[i].Options, TestVM.Test[i].Options))
				{
					totalPoint++;
				}
			}


			int mark = (double)(totalPoint * 100 / UserQuestions.Count) switch
			{
				>= 90 => 5,
				>= 80 => 4,
				>= 70 => 3,
				>= 60 => 2,
				_ => 1,
			};

			var Test_Result = new Test_Result()
			{
				Test_Result_ID = TestVM.TestResult_ID,
				Date = DateTime.Now.ToShortDateString(),
				Mark = mark,
				Right_Answer_Count = totalPoint,
				Status = TestVM.Status switch
				{
					null => mark <= 2 ? ResultStatus.Retake : ResultStatus.Pass,
					_ => mark <= 2 ? ResultStatus.Fail : ResultStatus.Pass,
				},
				Test_ID = UserQuestions[0].Test_ID,
				User_ID = TestVM.User.User_ID
			};

			using HttpClient client = new();
			try
			{
				var response = await client.PostAsJsonAsync($"https://localhost:7277/TestApi/PostResult",  Test_Result);
				response.EnsureSuccessStatusCode();

				var baseResponse = JsonSerializer.Deserialize<BaseResponse<bool>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
				if(baseResponse.StatusCode == ResponseStatus.Success)
				{
					Application.Current.MainWindow.Closing -= MainWindow_Closing;
					((MainWindowVM)Application.Current.MainWindow.DataContext).NavigateToFinish(TestVM.User, Test_Result, UserQuestions.Count);
				}
				else MessageBox.Show(baseResponse.Description);

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}


		//Help methods

		private void CopyQuestion()
		{
			UserQuestions = TestVM.Test.Select(question => new Question
			{
				Question_ID = question.Question_ID,
				Text = question.Text,
				Test_ID = question.Test_ID,
				Options = question.Options.Select(option => new Option
				{
					Option_ID = option.Option_ID,
					Text = option.Text,
					Right = false,
					Question_ID = option.Question_ID,
				}).ToList()
			}).ToList();
		}

		private void CheckButtonVisibility()
		{
			if (CurrentIndex == 1)
			{
				PreviousButtonVisibility = Visibility.Collapsed;
			}
			else if (CurrentIndex == UserQuestions.Count)
			{
				PreviousButtonVisibility = Visibility.Visible;
				NextButtonVisibility = Visibility.Collapsed;
				FinishButtonVisibility = Visibility.Visible;
			}
			else
			{
				PreviousButtonVisibility = Visibility.Visible;
				NextButtonVisibility = Visibility.Visible;
				FinishButtonVisibility = Visibility.Collapsed;
			}
			OnPropertyChanged(nameof(PreviousButtonVisibility));
			OnPropertyChanged(nameof(NextButtonVisibility));
			OnPropertyChanged(nameof(FinishButtonVisibility));
		}

		private void TimerStart()
		{
			CurrentTime = TimeSpan.FromMinutes(UserQuestions.Count);

			timer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(1)

			};

			timer.Tick += Timer_Tick;
			timer.Start();
		}

		private void Timer_Tick(object sender, EventArgs e)
		{

			CurrentTime = CurrentTime.Subtract(TimeSpan.FromSeconds(1));

			if (CurrentTime <= TimeSpan.Zero)
			{
				Finish();
			}
			OnPropertyChanged(nameof(CurrentTime));
		}

		private bool AreOptionsEqual(List<Option> options1, List<Option> options2)
		{
			if (options1.Count != options2.Count)
				return false;

			for (int j = 0; j < options1.Count; j++)
			{

				if (options1[j].Right != options2[j].Right)
				{
					return false;
				}
			}

			return true;
		}

		private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{

			if (MessageBox.Show("Буде збережено ту кількість відповідей, яку ви встигли обрати.", "Ви впевнені що хочете вийти з тесту?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
			{
				Finish();
			}
			else
			{
				e.Cancel = true;
			}
		}
	}
}
