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

namespace Client.PlayerClient.GamesControl {
	/// <summary>
	/// Interaction logic for TiePlayerControl.xaml
	/// </summary>
	public partial class TiePlayerControl : UserControl {
		
		SimpleSocketClient client;

		Simer timer;
		bool state = false, sucked = false;

		public TiePlayerControl(SimpleSocketClient client) {
			InitializeComponent();
			this.client = client;
			timer = new Simer(TimeSpan.FromSeconds(15));
			timer.Tick += timer_Tick;
		}

		public void Register(bool state) {
			this.state = state;
		}
		public void ShowQuestion(OQuestion question) {
			Dispatcher.Invoke(() => {
				quesControl.DisplayQuestion(question.question, question.attach);
			});
		}
		public void Start() {
			timer.Start();
			sucked = false;
			if (state) Dispatcher.Invoke(() => { button.IsEnabled = true; });
		}
		public void Pause(int player) {
			timer.Pause();
			Dispatcher.Invoke(() => { 
				txtSuck.Text = (player + 1).ToString();
				button.IsEnabled = false; 
			});
		}
		public void Resume(bool allow) {
			timer.Resume();
			Dispatcher.Invoke(() => { 
				txtSuck.Text = "0"; 
				if (allow) button.IsEnabled = true;
			});
		}
		public void Stop() {
			timer.Stop();
			Dispatcher.Invoke(() => { button.IsEnabled = false; });
		}

		private void button_Click(object sender, RoutedEventArgs e) {
			sucked = true;
			button.IsEnabled = false;
			client.SendMessage("OLPA CHP BELL");
		}

		void timer_Tick(int time, bool done) {
			txtTime.Text = string.Format("{0:0.00}", time / 100.0);
			if (done)
				timer.Stop();
		}
	}
}
