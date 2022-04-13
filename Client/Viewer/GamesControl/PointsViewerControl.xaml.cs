using Server.HostServer.Components;
using Server.Information;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	/// Interaction logic for PointsViewerControl.xaml
	/// </summary>
	public partial class PointsViewerControl : UserControl
	{
		Simer timer;
		string[] names = new string[4];
		int[] points = new int[4];
		public PointsViewerControl()
		{
			InitializeComponent();
			timer = new Simer();
			timer.SetTimeLimit(TimeSpan.FromSeconds(11));
			mediaPoint.Source = new Uri(HelperClass.PathString("Effects", "PlayerPoints.mp4"));
			mediaPoint.BeginInit(); mediaPoint.Play(); mediaPoint.Stop();
			timer.Tick += timer_Tick;
		}

		int[] times = new int[7] { 100, 360, 410, 700, 750, 995, 1020 };
		private void timer_Tick(int time, bool done)
		{
			int pos = 6;
			for (int i = 0; i < 7; i++)
				if (time < times[i]) { pos = i - 1; break; }
			if (pos == -1) return;
			Dispatcher.Invoke(() => { 
				if (pos % 2 == 1) {
					lblName.Visibility = Visibility.Hidden;
					lblPoint.Visibility = Visibility.Hidden;
				} else {
					pos /= 2;
					lblName.Visibility = Visibility.Visible;
					lblPoint.Visibility = Visibility.Visible;
					lblName.Content = names[pos];
					lblPoint.Content = points[pos];
				}
			});
		}

		public void Start(ObservableCollection<string> pName, ObservableCollection<int> pPoint)
		{
			for (int i = 0; i < 4; i++) { names[i] = pName[i]; points[i] = pPoint[i]; }
			for (int i = 0; i < 4; i++) for (int j = 0; j < 4; j++)
				if (points[i] > points[j]) {
					int x = points[i]; points[i] = points[j]; points[j] = x;
					string s = names[i]; names[i] = names[j]; names[j] = s;
				}
			Dispatcher.Invoke(() => { 
				lblName.Visibility = Visibility.Hidden;
				lblPoint.Visibility = Visibility.Hidden;
				mediaPoint.Position = TimeSpan.Zero; mediaPoint.Play();
				timer.Start();
			});
		}
	}
}
