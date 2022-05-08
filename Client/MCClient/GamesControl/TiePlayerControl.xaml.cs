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

namespace Client.MCClient.GamesControl {
	/// <summary>
	/// Interaction logic for TiePlayerControl.xaml
	/// </summary>
	public partial class TiePlayerControl : UserControl {
		
		SimpleSocketClient client;

		Simer timer;

		public TiePlayerControl(SimpleSocketClient client) {
			InitializeComponent();
			this.client = client;
			timer = new Simer(TimeSpan.FromSeconds(15));
			timer.Tick += timer_Tick;
		}

		public void ShowQuestion(OQuestion question) {
			Dispatcher.Invoke(() => {
				quesControl.DisplayQuestion(question);
			});
		}
		public void Start() {
			timer.Start();
		}
		public void Pause() {
			timer.Pause();
		}
		public void Resume() {
			timer.Resume();
		}
		public void Stop() {
			timer.Stop();
		}

		void timer_Tick(int time, bool done) {
			txtTime.Text = string.Format("{0:0.00}", time / 100.0);
			if (done)
				timer.Stop();
		}
	}
}
