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
		MediaPlayer mediaGeneral = new MediaPlayer(), //opening, startturn
					mediaChoosingNStar = new MediaPlayer(), //choosing, star
					mediaMysteryPrac = new MediaPlayer(), // 4 file + 2 file mystery / prac
					media5s = new MediaPlayer(),
					mediaBell = new MediaPlayer(),
					mediaResult = new MediaPlayer(); // 2 file
		int currentPlayer;

		

		int[] difficulty = new int[3];
		public FinishViewerControl(PlayerClass playerClass)
		{
			InitializeComponent();
			media5s.Open(new Uri(HelperClass.PathString("Effects", "")));
			mediaBell.Open(new Uri(HelperClass.PathString("Effects", "")));
			imgPlayer[0] = imgP1; imgPlayer[1] = imgP2; imgPlayer[2] = imgP3; imgPlayer[3] = imgP4;
			for (int i = 0; i < 4; i++) imgPlayer[i].Visibility = Visibility.Hidden;
			mediaStart.Visibility = Visibility.Visible;
			
			this.playerClass = playerClass;
			questionBox.SetContext(playerClass);
		}
		void Reset()
		{
			Dispatcher.Invoke(() => { 
				gridPoint.Visibility = Visibility.Visible;
				gridQuestion.Visibility = Visibility.Hidden;

				backgroundPoint.Visibility = Visibility.Hidden;
				mediaStart.Visibility = Visibility.Visible;
			});
		}

		bool donePlayChoosing;
		public void Choosing(int player)
		{
			donePlayChoosing = false;
			currentPlayer = player;
			Dispatcher.Invoke(() => {
				mediaStart.Visibility = Visibility.Visible;
				mediaStart.Source = new Uri(HelperClass.PathString("Effects", "VD_Begin.mp4"));
				mediaStart.Position = TimeSpan.Zero; mediaStart.Play();
			});
		}
		private void mediaStart_MediaEnded(object sender, RoutedEventArgs e)
		{
			Dispatcher.Invoke(() => { 
				if (donePlayChoosing == false) {
					donePlayChoosing = true;
					mediaStart.Source = new Uri(HelperClass.PathString("Effects", string.Format("VD_{0}_Start.mp4", currentPlayer + 1)));
					backgroundPoint.Visibility = Visibility.Visible;
				} else {
					//
				}
			});
		}
		public void Chosen(int[] diff)
		{
			mediaStart.Visibility = Visibility.Visible;
			for (int i = 0; i < 3; i++) difficulty[i] = diff[i];
			mediaStart.Play();
		}

		public void ShowQuestion(string question, string attach) {
			questionBox.SetQuestion(question);
			media.Source = new Uri(HelperClass.PathString("Media", attach));
		}

	}
}
