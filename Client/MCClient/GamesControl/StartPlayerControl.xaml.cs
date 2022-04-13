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
using Server.HostServer.Components;

namespace Client.MCClient.GamesControl
{
	/// <summary>
	/// Interaction logic for StartPlayerControl.xaml
	/// </summary>
	public partial class StartPlayerControl : UserControl
	{
		SimpleSocketClient client;

		const int timeLimit = 6000;
		Simer timer;

		public StartPlayerControl(SimpleSocketClient client)
		{
			InitializeComponent();
			this.client = client;
			timer = new Simer(timeLimit);
			timer.Tick += timer_Tick;
		}

		public void StartTimer()
		{
			timer.Start();
		}

		void timer_Tick(int time, bool done)
		{
			txtTime.Text = string.Format("{0:0.00}", time / 100.0);
			if (done)
				timer.Stop();
		}

		public void ShowQuestion(OQuestion question)
		{
			Dispatcher.Invoke(() => {
				quesControl.DisplayQuestion(question);
			});
		}

	}
}
