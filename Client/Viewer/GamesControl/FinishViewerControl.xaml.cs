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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAnimatedGif;

namespace Client.Viewer.GamesControl
{
	/// <summary>
	/// Interaction logic for FinishViewerControl.xaml
	/// </summary>
	public partial class FinishViewerControl : UserControl
	{
		PlayerClass playerClass;
		MediaPlayer mediaOpening = new MediaPlayer(),
					mediaStartTurn = new MediaPlayer(),
					mediaStar = new MediaPlayer(),
					mediaPrac = new MediaPlayer(), // 4 file + 2 file mystery / prac
					media5s = new MediaPlayer(),
					mediaBell = new MediaPlayer(),
					mediaResult = new MediaPlayer(),
					mediaEnding = new MediaPlayer(); // 2 file
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
			mediaEnding.Open(new Uri(HelperClass.PathString("Effects", "VD_Ending.mpeg")));
			mediaStartTurn.Open(new Uri(HelperClass.PathString("Effects", "VD_StartTurn.mpeg")));
			mediaOpening.Open(new Uri(HelperClass.PathString("Effects", "VD_Opening.mpeg")));
			media10s.Source = new Uri(HelperClass.PathString("Effects", "VD_10_Run.mp4"));
			media15s.Source = new Uri(HelperClass.PathString("Effects", "VD_20_Run.mp4"));
			media20s.Source = new Uri(HelperClass.PathString("Effects", "VD_30_Run.mp4"));
			mediaBegin.Source = new Uri(HelperClass.PathString("Effects", "VD_Begin.mp4")); mediaBegin.BeginInit();
			//testStar.BeginAnimation();

			mediaStart.Visibility = Visibility.Visible;
			
			this.playerClass = playerClass;
			questionBox.SetContext(playerClass);
			Reset();
			timer = new Simer(); timer.Tick += timer_Tick;

			mediaStartTurn.MediaEnded += MediaStartTurn_MediaEnded;
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

				mediaBegin.Visibility = Visibility.Hidden;
				backgroundPoint.Visibility = Visibility.Hidden;
				mediaStart.Visibility = Visibility.Hidden;
				// questionBox.Visibility = Visibility.Hidden;
				questionBox.SetSuck(-1);
			});
		}
		private void MediaStartTurn_MediaEnded(object? sender, EventArgs e)
		{
			Dispatcher.Invoke(() => {
				mediaBegin.Visibility = Visibility.Visible;
				mediaBegin.Play(); 
			});
		}

		
		public void Choosing(int player)
		{
			Reset();
			currentPlayer = player;
			Dispatcher.Invoke(() => {
				mediaStart.Source = new Uri(HelperClass.PathString("Effects", "VD_Start.mp4"));
				mediaStart.Play(); mediaStart.Stop();
				mediaBegin.Position = TimeSpan.Zero;
				mediaBegin.Stop();
				mediaStarAnimate.Visibility = Visibility.Hidden;

				mediaStartTurn.Position = TimeSpan.Zero;
				mediaStartTurn.Play();

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
				for (int i = 0; i < 3; i++) difficulty[i] = diff[i];
				mediaStart.Visibility = Visibility.Visible;
				questionBox.SetGroup(difficulty);
				mediaStart.Play();
				pointChoosing.Play(difficulty);
			});
		}

		public void SetTurn(int turn)
		{
			this.turn = turn;
			Dispatcher.Invoke(() => {
				questionBox.SetCurr(turn);
				questionBox.SetQuestion("");
				questionBox.SetSuck(-1);
			});
		}
		bool choosingStar = false;
		public void ShowQuestion(string question, string attach) {
			Dispatcher.Invoke(() => {
				if (choosingStar) choosingStar = false;
				else mediaStarAnimate.Visibility = Visibility.Hidden;
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
						media10s.Visibility = Visibility.Visible;
						media10s.Position = TimeSpan.Zero; media10s.Play();
						break;
					case 1:
						media15s.Visibility = Visibility.Visible;
						media15s.Position = TimeSpan.Zero; media15s.Play();
						break;
					case 2:
						media20s.Visibility = Visibility.Visible;
						media20s.Position = TimeSpan.Zero; media20s.Play();
						break;
				};
			});
		}
		private void media10s_MediaEnded(object sender, RoutedEventArgs e) { media10s.Visibility = Visibility.Hidden; }
		private void media15s_MediaEnded(object sender, RoutedEventArgs e) { media15s.Visibility = Visibility.Hidden; }
		private void media20s_MediaEnded(object sender, RoutedEventArgs e) { media20s.Visibility = Visibility.Hidden; }

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
				questionBox.SetSuck(player);
			});
		}

		public void Star()
		{
			choosingStar = true;
			Dispatcher.Invoke(() => {
				mediaStarAnimate.Visibility = Visibility.Visible;
				mediaStar.Position = TimeSpan.Zero;
				ImageBehavior.GetAnimationController(mediaStarAnimate).GotoFrame(0);
				mediaStar.Play(); ImageBehavior.GetAnimationController(mediaStarAnimate).Play();
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
		public void IntroPractice()
		{
			Dispatcher.Invoke(() => {
				mediaPrac.Open(new Uri(HelperClass.PathString("Effects", "VD_Mystery.mpeg")));
				mediaPrac.Position = TimeSpan.Zero;
				mediaPrac.Play();
			});
		}
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

		public void Opening()
		{
			ChangeScene("");
			Dispatcher.Invoke(() => {
				mediaOpening.Position = TimeSpan.Zero;
				mediaOpening.Play();
			});
		}
		public void Ending()
		{
			ChangeScene("");
			Dispatcher.Invoke(() => {
				mediaEnding.Position = TimeSpan.Zero;
				mediaEnding.Play();
			});
		}
	}
}
