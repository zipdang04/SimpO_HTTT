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
		public AnswerViewerControl()
		{
			InitializeComponent();
			DataContext = playerClass;

			media.Source = new Uri(HelperClass.PathString("Effects", "PlayerAnswer.mp4"));
			media.BeginInit(); media.Play(); media.Stop();

			lblNames = new List<Label>();
			lblNames.Add(lblName1); lblNames.Add(lblName2); lblNames.Add(lblName3); lblNames.Add(lblName4);
			lblAnswers = new List<Label>();
			lblAnswers.Add(lblAnswer1); lblAnswers.Add(lblAnswer2); lblAnswers.Add(lblAnswer3); lblAnswers.Add(lblName4);
			lblTimes = new List<Label>();
			lblTimes.Add(lblTime1); lblTimes.Add(lblTime2); lblTimes.Add(lblTime3); lblTimes.Add(lblTime4);
		}

		public void SetContext(PlayerClass playerClass)
		{
			this.playerClass = playerClass;
			DataContext = playerClass;
		}

		private void media_MediaEnded(object sender, RoutedEventArgs e)
		{
			Dispatcher.Invoke(() => { gridAnswer.Visibility = Visibility.Visible; });
		}
		public void Reset()
		{
			gridAnswer.Visibility = Visibility.Hidden;
		}
		public void Run()
		{
			Dispatcher.Invoke(() => {
				media.Position = TimeSpan.Zero;
				gridAnswer.Visibility = Visibility.Hidden;
				media.Play();
			});
		}
		public void SetPlayerAnswer(int player, string answer, int time)
		{
			Dispatcher.Invoke(() => {
				lblAnswers[player].Content = answer;
				lblTimes[player].Content = string.Format("{0:0.00}", time);
			});
		}
	}
}
