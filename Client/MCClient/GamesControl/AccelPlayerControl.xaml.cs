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
using Server.HostServer.Components;

namespace Client.MCClient.GamesControl
{
	/// <summary>
	/// Interaction logic for AccelUserControl.xaml
	/// </summary>

	public partial class AccelPlayerControl : UserControl
	{
		SimpleSocketClient client;

		Simer timer;
		int timeLimit;

		public AccelPlayerControl(SimpleSocketClient client)
		{
			InitializeComponent();
			this.client = client;
			timer = new Simer();
			timer.Tick += timer_Tick;
			mediaPlayer.LoadedBehavior = MediaState.Manual;
			
		}

		void timer_Tick(int time, bool done)
		{
			lblTime.Content = string.Format("{0:0.00}", time / 100.0);
		}

		public void ShowQuestion(int turn, string question, string attach, string answer, int time)
		{
			timeLimit = time;
			attach = HelperClass.PathString("Media", attach);
			Dispatcher.Invoke(() => {
				mediaPlayer.Source = new Uri(attach);
				lblTemp.Content = turn;
				lblQuestion.Content = question;
				lblAnswer.Content = answer;
			});
		}

		public void StartTimer()
		{
			Dispatcher.Invoke(() => {
				mediaPlayer.Position = TimeSpan.FromMilliseconds(1);
				mediaPlayer.Play();
			});
			Dispatcher.Invoke(() => { 
				timer.Start(timeLimit);
			});
		}

		public void ResetGame()
		{
			Dispatcher.Invoke(() => {
				lblAnswer.Content = "";
				timer.Stop();
				mediaPlayer.Stop();
			});
		}
	}
}
