using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

using Server.Information;

namespace Client.Viewer.GamesControl
{
	/// <summary>
	/// Interaction logic for AnswerViewerControl.xaml
	/// </summary>
	public partial class AnswerViewerControl : UserControl
	{
		PlayerClass? playerClass;
		List<Label> lblNames, lblAnswers, lblTimes;
		ObservableCollection<string> names, answers, timeString;
		ObservableCollection<int> times, positions;
		//int[] times = new int[4], positions = new int[4];
		public AnswerViewerControl(PlayerClass playerClass)
		{
			InitializeComponent();
			this.playerClass = playerClass;

			mediaCorrect.Source = new Uri(HelperClass.PathString("Effects", "PlayerAnswerCorrect.mpeg"));
			mediaCorrect.BeginInit(); mediaCorrect.Play(); mediaCorrect.Stop();
			mediaWrong.Source = new Uri(HelperClass.PathString("Effects", "PlayerAnswerWrong.mpeg"));
			mediaWrong.BeginInit(); mediaWrong.Play(); mediaWrong.Stop();
			media.Source = new Uri(HelperClass.PathString("Effects", "PlayerAnswers_TT.mp4"));
			media.BeginInit(); media.Play(); media.Stop();

			lblNames = new List<Label>();
			lblNames.Add(lblName1); lblNames.Add(lblName2); lblNames.Add(lblName3); lblNames.Add(lblName4);
			lblAnswers = new List<Label>();
			lblAnswers.Add(lblAnswer1); lblAnswers.Add(lblAnswer2); lblAnswers.Add(lblAnswer3); lblAnswers.Add(lblName4);
			lblTimes = new List<Label>();
			lblTimes.Add(lblTime1); lblTimes.Add(lblTime2); lblTimes.Add(lblTime3); lblTimes.Add(lblTime4);

			names = new ObservableCollection<string> { "", "", "", "" };
			answers = new ObservableCollection<string> { "", "", "", "" };
			timeString = new ObservableCollection<string> { "", "", "", "" };
			times = new ObservableCollection<int> { 0, 0, 0, 0 };
			positions = new ObservableCollection<int> { 0, 0, 0, 0 };
			DataContext = new {
				names = names,
				answers = answers,
				times = timeString
			};
		}

		public void Reset()
		{
			Dispatcher.Invoke(() => {
				gridAnswer.Visibility = Visibility.Hidden;
				for (int i = 0; i < 4; i++) {
					lblAnswers[i].Visibility = Visibility.Visible;
					lblNames[i].Visibility = Visibility.Visible;
					lblTimes[i].Visibility = Visibility.Visible;
				}
			});
		}

		public void SetPlayerAnswer(string[] pAns, int[] pTime, bool sort = false)
		{
			Reset();
			for (int i = 0; i < 4; i++) {
				answers[i] = pAns[i]; times[i] = pTime[i]; positions[i] = i;
			}
			if (sort)
				for (int i = 0; i < 4; i++) for (int j = i + 1; j < 4; j++)
						if (times[i] > times[j]) {
							string s = answers[i]; answers[i] = answers[j]; answers[j] = s;
							int x = times[i]; times[i] = times[j]; times[j] = x;
							x = positions[i]; positions[i] = positions[j]; positions[j] = x;
						}
			for (int i = 0; i < 4; i++) {
				names[i] = (playerClass == null) ? "" : playerClass.names[positions[i]];
				timeString[i] = timeString[i] = string.Format("{0:00.00}", pTime[i] / 100.0);
			}
		}
		public void Run(string[] pAns, int[] pTime, bool sort = false)
		{
			SetPlayerAnswer(pAns, pTime, sort);
			Dispatcher.Invoke(() => {
				media.Position = TimeSpan.Zero;
				gridAnswer.Visibility = Visibility.Hidden;
				media.Play();
			});
		}
		private void media_MediaEnded(object sender, RoutedEventArgs e)
		{
			Dispatcher.Invoke(() => { gridAnswer.Visibility = Visibility.Visible; });
		}
		public void Conclusion(bool[] correct)
		{
			Dispatcher.Invoke(() => {
				bool haveWinner = false;
				for (int i = 0; i < 4; i++)
					if (correct[positions[i]] == false) {
						lblAnswers[i].Visibility = Visibility.Hidden;
						lblNames[i].Visibility = Visibility.Hidden;
						lblTimes[i].Visibility = Visibility.Hidden;
					}
					else haveWinner = true;
				if (haveWinner) {
					mediaCorrect.Position = TimeSpan.Zero; mediaCorrect.Play();
				}
				else {
					mediaWrong.Position = TimeSpan.Zero; mediaWrong.Play();
				}
			});
		}

		public void Change(string phanthi)
		{
			Dispatcher.Invoke(() =>
			{
				media.Source = new Uri(HelperClass.PathString("Effects", string.Format("PlayerAnswers_{0}.mp4", phanthi)));
				media.Play(); media.Stop();
			});
		}
	}
}
