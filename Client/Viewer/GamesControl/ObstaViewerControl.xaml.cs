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

using Client.Viewer.GamesControl.Components;

namespace Client.Viewer.GamesControl
{
	/// <summary>
	/// Interaction logic for ObstaViewerControl.xaml
	/// </summary>
	public partial class ObstaViewerControl : UserControl
	{
		List<Image> images;
		WordControl[] wordControls = new WordControl[4];
		public ObstaViewerControl()
		{
			InitializeComponent();
			images = new List<Image>();
			images.Add(rectUL); images.Add(rectUR); images.Add(rectDR); images.Add(rectDL); images.Add(rectTT);
			for (int i = 0; i < 4; i++)
			{
				wordControls[i] = new WordControl();
				gridWord.Children.Add(wordControls[i]);
				Grid.SetColumn(wordControls[i], 1);
				Grid.SetRow(wordControls[i], (i + 1) * 2);
			}
			mediaOpening.Source = new Uri(HelperClass.PathString("Effects", "VCNV_Opening")); mediaOpening.BeginInit();
			mediaShow.Source = new Uri(HelperClass.PathString("Effects", "VCNV_ShowQuestion")); mediaShow.BeginInit();
			media15s.Source = new Uri(HelperClass.PathString("Effects", "VCNV_15s")); media15s.BeginInit();
		}

		public void ResetGame(string attach, int[] cntLetter)
		{
			Dispatcher.Invoke(() => { 
				image.Source = new BitmapImage(new Uri(HelperClass.PathString("Media", attach)));
				double width = image.RenderSize.Width;
				double height = image.RenderSize.Height;
				for (int i = 0; i < 5; i++) {
					images[i].Visibility = Visibility.Visible;
					images[i].Width = width; images[i].Height = height;
				}
				for (int i = 0; i < 4; i++)
					wordControls[i].SetWord(cntLetter[i]);

				media15s.Visibility = Visibility.Hidden;
				mediaShow.Visibility = Visibility.Hidden;
				gridWord.Visibility = Visibility.Visible;
				gridPic.Visibility = Visibility.Hidden;
				qBox.Visibility = Visibility.Hidden;
			});
		}



		public void Opening() { Dispatcher.Invoke(() => { mediaOpening.Position = TimeSpan.Zero; mediaOpening.Play(); }); }
		public void ChangeScene(string s)
		{
			Dispatcher.Invoke(() => { 
				gridPic.Visibility = Visibility.Hidden;
				gridWord.Visibility = Visibility.Hidden;
				if (s == "PIC") gridPic.Visibility = Visibility.Visible;
				else if (s == "WORD") gridWord.Visibility = Visibility.Visible;
			});
		}

		public void ShowQuestion(int index, string question, string attach)
		{
			Dispatcher.Invoke(() => { 
				qBox.SetQuestion(question);
				qBox.Visibility = Visibility.Hidden;
				wordControls[index].SetChoosing();
				mediaShow.Visibility = Visibility.Visible;
				mediaShow.Position = TimeSpan.Zero; mediaShow.Play();
			});
		}
		private void mediaShow_MediaEnded(object sender, RoutedEventArgs e)
		{
			qBox.Visibility = Visibility.Visible;
		}

		public void StartTimer()
		{
			Dispatcher.Invoke(() => { 
				mediaShow.Visibility = Visibility.Hidden;
				media15s.Visibility = Visibility.Visible;
				media15s.Position = TimeSpan.Zero; media15s.Play();
			});
		}
	
		public void Open(int index) {
			// media Mở
			Dispatcher.Invoke(() => { 
				images[index].Visibility = Visibility.Collapsed;
			});
		}
	}
}
