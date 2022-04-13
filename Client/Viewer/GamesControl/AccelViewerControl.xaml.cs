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
	/// Interaction logic for AccelControl.xaml
	/// </summary>
	public partial class AccelViewerControl : UserControl
	{
		public AccelViewerControl()
		{
			InitializeComponent();
			mediaStart.Source = new Uri(HelperClass.PathString("Effects", "TT_Start.mp4"));
			mediaStart.BeginInit(); mediaStart.Play(); mediaStart.Stop();
			mediaRun.BeginInit();
		}

		public void Prepare(string question, string attach, int turn)
		{
			Dispatcher.Invoke(() => {
				txtQuestion.Visibility = Visibility.Hidden;
				txtQuestion.Text = question;

				media.Source = new Uri(HelperClass.PathString("Media", attach));
				media.Visibility = Visibility.Hidden;
				media.Play(); media.Stop();
				media.Volume = 0;

				mediaRun.Source = new Uri(HelperClass.PathString("Effects", String.Format("TT_{0}0s.mp4", turn + 1)));
				mediaRun.Play(); mediaRun.Stop();

				mediaStart.Visibility = Visibility.Visible;
				mediaRun.Visibility = Visibility.Hidden;
				mediaStart.Position = TimeSpan.Zero; mediaStart.Play();
			});
		}
		private void mediaStart_MediaEnded(object sender, RoutedEventArgs e)
		{
			Dispatcher.Invoke(() => {
				txtQuestion.Visibility = Visibility.Visible;
				media.Visibility = Visibility.Visible;
			});
		}

		public void Run()
		{
			Dispatcher.Invoke(() => {
				mediaStart.Visibility = Visibility.Hidden;
				mediaRun.Position = TimeSpan.Zero; 
				mediaRun.Visibility = Visibility.Visible;
				mediaRun.Play(); media.Play();
			});
		}

		
	}
}
