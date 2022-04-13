using Server.HostServer.Components;
using Server.Information;
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

namespace Client.Viewer.GamesControl
{
	/// <summary>
	/// Interaction logic for FinishViewerControl.xaml
	/// </summary>
	public partial class FinishViewerControl : UserControl
	{
		PlayerClass playerClass;
		Image[] imgPlayer = new Image[4];
		Rectangle[] rects = new Rectangle[4];
		MediaPlayer mediaGeneral = new MediaPlayer(), //opening, startturn
					mediaStar = new MediaPlayer(), //choosing, star
					mediaPrac = new MediaPlayer(), // 4 file + 2 file mystery / prac
					media5s = new MediaPlayer(),
					mediaBell = new MediaPlayer(),
					mediaResult = new MediaPlayer(); // 2 file
		int currentPlayer;
		int turn = -1;
		int[] difficulty = new int[3];
		Simer timer;
		public FinishViewerControl(PlayerClass playerClass)
		{
			InitializeComponent();
			media5s.Open(new Uri(HelperClass.PathString("Effects", "VD_5s.m4a")));
			mediaBell.Open(new Uri(HelperClass.PathString("Effects", "VD_Bell.m4a")));
			mediaStar.Open(new Uri(HelperClass.PathString("Effects", "VD_Star.mp3")));
			media10s.Source = new Uri(HelperClass.PathString("Effects", "VD_10_Run.mp4"));
			media15s.Source = new Uri(HelperClass.PathString("Effects", "VD_20_Run.mp4"));
			media20s.Source = new Uri(HelperClass.PathString("Effects", "VD_30_Run.mp4"));

			imgPlayer[0] = imgP1; imgPlayer[1] = imgP2; imgPlayer[2] = imgP3; imgPlayer[3] = imgP4;
			rects[0] = rectBell1; rects[1] = rectBell2; rects[2] = rectBell3; rects[3] = rectBell4;
			for (int i = 0; i < 4; i++) imgPlayer[i].Visibility = Visibility.Hidden;
			mediaStart.Visibility = Visibility.Visible;

			
			this.playerClass = playerClass;
			questionBox.SetContext(playerClass);
			Reset();
			timer = new Simer(); timer.Tick += timer_Tick;
		}

		public void ChangeScene(string s)
		{
			Dispatcher.Invoke(() => {
				gridPoint.Visibility = Visibility.Hidden;
				gridQuestion.Visibility = Visibility.Hidden;
				gridPrac.Visibility = Visibility.Hidden;
				gridMedia.Visibility = Visibility.Hidden;
				switch (s) {
					case "POINT":
						gridPoint.Visibility = Visibility.Visible;
						break;
					case "QUES":
						gridQuestion.Visibility = Visibility.Visible;
						break;
					case "PRAC":
						gridPrac.Visibility = Visibility.Visible;
						break;
					case "MEDIA":
						gridMedia.Visibility = Visibility.Visible;
						break;
				}
			});
		}
		void Reset()
		{
			Dispatcher.Invoke(() => {
				ChangeScene("POINT");

				mediaBegin.Visibility = Visibility.Visible;
				backgroundPoint.Visibility = Visibility.Hidden;
				mediaStart.Visibility = Visibility.Hidden;
				for (int i = 0; i < 4; i++) {
					imgPlayer[i].Visibility = Visibility.Hidden;
					rects[i].Visibility = Visibility.Hidden;
				}
			});
		}

		private void media10s_MediaEnded(object sender, RoutedEventArgs e)
		{
			media10s.Visibility = Visibility.Hidden;
		}

		private void media15s_MediaEnded(object sender, RoutedEventArgs e)
		{
			media15s.Visibility = Visibility.Hidden;
		}

		private void media20s_MediaEnded(object sender, RoutedEventArgs e)
		{
			media20s.Visibility = Visibility.Hidden;
		}
		public void Choosing(int player)
		{
			Reset();
			donePlayChoosing = false;
			currentPlayer = player;
			Dispatcher.Invoke(() => {
				mediaStart.Source = new Uri(HelperClass.PathString("Effects", string.Format("VD_{0}_Start.mp4", currentPlayer + 1)));
				mediaStart.Play(); mediaStart.Stop();

				mediaBegin.Visibility = Visibility.Visible;
				mediaBegin.Position = TimeSpan.Zero; mediaBegin.Play(); 
				
				imgPlayer[player].Visibility = Visibility.Visible;
				questionBox.SetChosenOne(player);
			});
		}
		private void mediaBegin_MediaEnded(object sender, RoutedEventArgs e)
		{
			Dispatcher.Invoke(() => {
				backgroundPoint.Visibility = Visibility.Visible;
				mediaBegin.Visibility = Visibility.Hidden;
			});
		}
		private void mediaStart_MediaEnded(object sender, RoutedEventArgs e)
		{
			Dispatcher.Invoke(() => { 
				ChangeScene("QUES");
				questionBox.Visibility = Visibility.Visible;
			});
		}
		//
		public void Chosen(int[] diff)
		{
			Dispatcher.Invoke(() => {
				mediaStart.Visibility = Visibility.Visible;
				for (int i = 0; i < 3; i++) difficulty[i] = diff[i];
				mediaStart.Play();
				pointChoosing.Play(difficulty);
			});
		}

		public void SetTurn(int turn)
		{
			this.turn = turn;
			Dispatcher.Invoke(() => {
				questionBox.SetLabel((turn + 1).ToString() + "0 điểm");
				questionBox.SetQuestion("");
				for (int i = 0; i < 4; i++)
					rects[i].Visibility = Visibility.Hidden;
			});
		}
		public void ShowQuestion(string question, string attach) {
			Dispatcher.Invoke(() => {
				mediaStarAnimate.Visibility = Visibility.Hidden;
				questionBox.SetQuestion(question);
				media.Source = new Uri(HelperClass.PathString("Media", attach));
			});
		}
		public void ShowMedia()
		{
			ChangeScene("MEDIA");
			Dispatcher.Invoke(() => { 
				media.Position = TimeSpan.Zero;
				media.Play();
			});
		}
		public void Run()
		{
			Dispatcher.Invoke(() => {
				switch (difficulty[turn]) {
					case 0:
						media10s.Position = TimeSpan.Zero; media10s.Play();
						break;
					case 1:
						media15s.Position = TimeSpan.Zero; media15s.Play();
						break;
					case 2:
						media20s.Position = TimeSpan.Zero; media20s.Play();
						break;
				};
			});
		}
		public void Start5s()
		{
			Dispatcher.Invoke(() => {
				media5s.Position = TimeSpan.Zero;
				media5s.Play();
			});
		}
		public void SomeoneSucking(int player)
		{
			Dispatcher.Invoke(() => {
				mediaBell.Position = TimeSpan.Zero;
				mediaBell.Play();
				rects[player].Visibility = Visibility.Visible;
			});
		}

		public void Star()
		{
			//VD_Star.mp3
			Dispatcher.Invoke(() => {
				mediaStar.Position = TimeSpan.Zero;
				mediaStarAnimate.Position = TimeSpan.Zero;
				mediaStar.Play(); mediaStarAnimate.Play();
			});
		}
		public void ResultMusic(bool isCorrect)
		{
			string attach = isCorrect ? "Correct" : "Wrong";
			attach = HelperClass.PathString("Effects", string.Format("VD_{0}.m4a", attach));
			Dispatcher.Invoke(() => {
				mediaResult.Open(new Uri(attach));
				mediaResult.Position = TimeSpan.Zero;
				mediaResult.Play();
			});
		}

		int pracTime;
		public void PracticeMode(bool main)
		{
			ChangeScene("PRAC");
			int diff = difficulty[turn];
			if (diff == 0) return;

			int time = (diff == 1) ? 30 : 60;
			if (main == false) time = time * 2 / 3;

			string attach = HelperClass.PathString("Effects", string.Format("VD_Prac_{0}s.mpeg", time));
			Dispatcher.Invoke(() => {
				mediaPrac.Open(new Uri(attach));
				mediaPrac.Position = TimeSpan.Zero; mediaPrac.Play();
				timer.Start(TimeSpan.FromSeconds(time));
				lblTime.Content = time;
				pracTime = time;
			});
		}
		public void timer_Tick(int time, bool done)
		{
			Dispatcher.Invoke(() => { lblTime.Content = pracTime - (time / 100); if (done) lblTime.Content = 0; });
		}

	}
}
