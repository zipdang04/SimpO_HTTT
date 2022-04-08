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
using System.IO;

namespace Client.PlayerClient.GamesControl
{
	/// <summary>
	/// Interaction logic for AccelUserControl.xaml
	/// </summary>

	public partial class AccelPlayerControl : UserControl
	{
		SimpleSocketClient client;

		DispatcherTimer timer;
		int timeLimit;
		DateTime timeBegin;
		int getTime()
		{
			TimeSpan span = DateTime.Now - timeBegin;
			return (span.Seconds * 1000 + span.Milliseconds) / 10;
		}

		public AccelPlayerControl(SimpleSocketClient client)
		{
			InitializeComponent();
			this.client = client;
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromMilliseconds(2);
			timer.Tick += timer_Tick;
			mediaPlayer.LoadedBehavior = MediaState.Manual;
			
		}



		void timer_Tick(object? sender, EventArgs e)
		{
			int time = getTime();
			lblTime.Content = string.Format("{0:0.00}", time / 100.0);
			//
			if (time >= timeLimit){
				StopTimer();
				mediaPlayer.Stop();
			}
		}

		public void ShowQuestion(int turn, string question, string attach, int time)
		{
			timeLimit = time;
			attach = Directory.GetCurrentDirectory() + @"\Resources\Media\" + attach;
			//attach = "Resources/" + attach;
			Dispatcher.Invoke(() => {
				mediaPlayer.Source = new Uri(attach);
				//mediaPlayer.Visibility = Visibility.Hidden;
				lblTemp.Content = turn;
				lblQuestion.Content = question;
				mediaPlayer.Play();
				mediaPlayer.Stop();
			});
		}

		public void StartTimer()
		{
			timeBegin = DateTime.Now;
			Dispatcher.Invoke(() => {
				//mediaPlayer.Visibility = Visibility.Visible;
				mediaPlayer.Position = TimeSpan.FromSeconds(0);
				mediaPlayer.Play();
				txtAnswer.Text = ""; txtAnswer.IsEnabled = true;
				txtAnswer.Focus();
				mediaPlayer.Play();
			});
			Dispatcher.Invoke(() => { 
				timer.Start();
			});
		}

		public void ResetGame()
		{
			Dispatcher.Invoke(() => {
				txtAnswer.IsEnabled = false;
				lblAnswer.Content = "";
			});
			timer.Stop();
		}

		void StopTimer()
		{
			timer.Stop();
			Dispatcher.Invoke(() => {
				txtAnswer.Text = "";
				txtAnswer.IsEnabled = false;
			});
		}

		private void txtAnswer_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				client.SendMessage(string.Format("OLPA TT ANSWER {0} {1}", getTime(), HelperClass.MakeString(txtAnswer.Text)));
				Dispatcher.Invoke(() => { 
					lblAnswer.Content = txtAnswer.Text;
					txtAnswer.Text = "";
				});
			}
		}

		
	}
}
