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
using System.IO;
using System.Windows.Threading;

namespace Client.PlayerClient.GamesControl
{
	/// <summary>
	/// Interaction logic for StartPlayerControl.xaml
	/// </summary>
	public partial class StartPlayerControl : UserControl
	{
		SimpleSocketClient client;

		const int timeLimit = 6000;
		DateTime timeBegin;
		DispatcherTimer timer;
		int getTime()
		{
			TimeSpan span = DateTime.Now - timeBegin;
			return (span.Seconds * 1000 + span.Milliseconds) / 10;
		}

		public StartPlayerControl(SimpleSocketClient client)
		{
			InitializeComponent();
			this.client = client;
			timer = new DispatcherTimer();
		}

		public void StartTimer()
		{
		}

		void timer_Tick(object? sender, EventArgs e)
		{
			int time = getTime();
			txtTime.Text = string.Format("{0:0.00}", time / 100.0);
			//
			if (time >= timeLimit)
			{
				timer.Stop();
			}
		}

		public void ShowQuestion(OQuestion question)
		{
			Dispatcher.Invoke(() => { 
				txtQuestion.Text = question.question;
				try {
					string path = Directory.GetCurrentDirectory() + @"\Resources\Media\" + question.attach;
					image.Source = new BitmapImage(new Uri(path));
				}
				catch {
					image = new Image();
				}
			});
		}

	}
}
