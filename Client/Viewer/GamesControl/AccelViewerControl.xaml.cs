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
			mediaRun.Source = new Uri(HelperClass.PathString("Effects", "TT_Run.mp4"));
			mediaRun.BeginInit(); mediaRun.Play(); mediaRun.Stop();
		}

		public void Prepare(string question, string attach, int time)
		{
			txtQuestion.Visibility = Visibility.Hidden;
			media.Source = new Uri(HelperClass.PathString("Media", attach));
			media.Play(); media.Stop();
			media.Volume = 0;

			mediaStart.Position = TimeSpan.Zero; mediaStart.Play();
		}

		public void Run()
		{
		}

		private void media_MediaEnded(object sender, RoutedEventArgs e)
		{
			txtQuestion.Visibility = Visibility.Visible;
		}
	}
}
