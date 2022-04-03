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

using SimpleSockets.Client;
using Server.QuestionClass;
using Server.Information;
using System.Windows.Threading;

namespace Client.PlayerClient.GamesControl
{
	/// <summary>
	/// Interaction logic for AccelUserControl.xaml
	/// </summary>

	public partial class AccelPlayerControl : UserControl
	{
		SimpleSocketClient client;
		DispatcherTimer timer;

		int time, timeLimit;

		public AccelPlayerControl(SimpleSocketClient client)
		{
			InitializeComponent();
			this.client = client;
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromMilliseconds(10);
			timer.Tick += timer_Tick;
		}

		void timer_Tick(object? sender, EventArgs e)
		{
			time++; lblTime.Content = time;
			if (time == timeLimit) StopTimer();
		}

		public void ShowQuestion(string label, string question, string attach, int time)
		{
			timeLimit = time;
			attach = "Resources/" + attach;
			Dispatcher.Invoke(() => {
				lblTemp.Content = "Hàng ngang " + label;
				lblQuestion.Content = question;
				mediaPlayer.Source = new Uri(attach);
			});
		}

		public void StartTimer()
		{
			time = 0;
			txtAnswer.Text = ""; txtAnswer.IsEnabled = true;
			lblTime.Content = time;
			mediaPlayer.Play(); timer.Start();
			txtAnswer.Focus();
		}

		public void ResetGame()
		{
			txtAnswer.IsEnabled = false;
			timer.Stop();
			lblAnswer.Content = "";
		}

		void StopTimer()
		{
			txtAnswer.Text = "";
			txtAnswer.IsEnabled = false;
		}

		private void txtAnswer_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				client.SendMessage(string.Format("OLPA VD ANSWER {0} {1}", time, txtAnswer.Text));
				lblAnswer.Content = txtAnswer.Text;
				txtAnswer.Text = "";
			}
		}
	}
}
