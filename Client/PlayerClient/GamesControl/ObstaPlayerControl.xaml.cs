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
	/// Interaction logic for ObstaPlayerControl.xaml
	/// </summary>
	public partial class ObstaPlayerControl : UserControl
	{
		SimpleSocketClient client;
		DispatcherTimer timer;

		string imagePath;

		int timeLimit;
		DateTime timeBegin;
		int getTime()
		{
			TimeSpan span = DateTime.Now - timeBegin;
			return (span.Seconds * 1000 + span.Milliseconds) / 10;
		}

		public ObstaPlayerControl(SimpleSocketClient client)
		{
			InitializeComponent();
			this.client = client;
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromMilliseconds(2);
			timer.Tick += timer_Tick;
			setImage();
		}

		void setImage()
		{
			imageControl.setImage(imagePath);
		}
		void erase(int idx) {
			imageControl.erase(idx);
		}

		void timer_Tick(object? sender, EventArgs e)
		{
			int time = getTime();
			lblTime.Content = time / 100.0;
			//
			if (time >= timeLimit){
				StopTimer();
			}
		}

		public void ShowQuestion(string label, string question)
		{
			Dispatcher.Invoke(() => {
				lblTemp.Content = "Hàng ngang " + label;
				lblQuestion.Content = question;
			});
		}

		public void StartTimer()
		{
			txtAnswer.Text = "";
			txtAnswer.IsEnabled = true;
			//time = 15; lblTime.Content = time;
			timer.Start();
			txtAnswer.Focus();
		}
		
		void StopTimer()
		{
			txtAnswer.Text = "";
			txtAnswer.IsEnabled = false;
		}
		
		public void ResetGame()
		{
			btnBell.IsEnabled = true;
			txtAnswer.IsEnabled = false;
			timer.Stop();
			lblAnswer.Content = "";
			setImage();
		}

		private void btnBell_Click(object sender, RoutedEventArgs e)
		{
			client.SendMessage("OLPA VCNV BELL");
			btnBell.IsEnabled = false;
		}

		private void txtAnswer_KeyDown(object sender, KeyEventArgs e)
		{
			if (txtAnswer.IsEnabled == true && e.Key == Key.Enter) {
				client.SendMessage("OLPA VCNV ANSWER " + HelperClass.MakeString(txtAnswer.Text));
				lblAnswer.Content = txtAnswer.Text;
			}
		}
	}
}
