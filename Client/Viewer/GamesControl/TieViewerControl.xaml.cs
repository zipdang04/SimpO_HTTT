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

namespace Client.Viewer.GamesControl {
	/// <summary>
	/// Interaction logic for TieViewerControl.xaml
	/// </summary>
	public partial class TieViewerControl : UserControl {
		Rectangle[] rects = new Rectangle[4];
		bool[] state;
		MediaPlayer mediaBell = new MediaPlayer(),
					mediaCorrect = new MediaPlayer();

        public TieViewerControl(PlayerClass playerClass) {
			InitializeComponent();
			rects[0] = rectBell1; rects[1] = rectBell2; rects[2] = rectBell3; rects[3] = rectBell4;
			qBox.SetContext(playerClass);
			mediaStart.Source = new Uri(HelperClass.PathString("Effects", "CHP_Start.mp4"));
            mediaRun.Source = new Uri(HelperClass.PathString("Effects", "CHP_Run.mp4"));
            mediaBell.Open(new Uri(HelperClass.PathString("Effects", "VD_Bell.m4a")));
            mediaCorrect.Open(new Uri(HelperClass.PathString("Effects", "VCNV_BellCorrect.m4a")));
        }

		public void Register(bool[] state) {
			this.state = state;
		}
		public void ShowQuestion(string question, string attach) {
			Dispatcher.Invoke(() => {
				qBox.SetQuestion(question);
				qBox.SetHiddenAll();
				mediaStart.Position = TimeSpan.Zero;
				mediaRun.Position = TimeSpan.Zero; mediaRun.Stop();

				mediaRun.Visibility = Visibility.Hidden;
				for (int i = 0; i < 4; i++) rects[i].Visibility = Visibility.Hidden;
				
				mediaStart.Visibility = Visibility.Visible;
				mediaStart.Play();
			});
		}
		public void Start() {
			Dispatcher.Invoke(() => {
				mediaRun.Visibility = Visibility.Visible;
				mediaRun.Play();
			});
		}
		public void SomeoneSucking(int player) {
			Dispatcher.Invoke(() => {
				mediaRun.Pause(); rects[player].Visibility = Visibility.Visible;
				mediaBell.Position = TimeSpan.Zero; mediaBell.Play();
			});
		}
		public void Resume() {
			Dispatcher.Invoke(() => {
				for (int i = 0; i < 4; i++) rects[i].Visibility = Visibility.Hidden;
				mediaRun.Play();
			});
		}
		public void Correct() {
			Dispatcher.Invoke(() => {
				mediaRun.Stop();
				mediaCorrect.Position = TimeSpan.Zero; mediaCorrect.Play();
			});
		}

		private void mediaStart_MediaEnded(object sender, RoutedEventArgs e) {
			qBox.SetVisibleAll();
			for (int i = 0; i < 4; i++)
				if (state[i] == false) qBox.SetPlayerHidden(i);
		}
	}
}
