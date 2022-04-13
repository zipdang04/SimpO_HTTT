using Server.HostServer.Components;
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

namespace Client.Viewer.GamesControl.Components
{
	/// <summary>
	/// Interaction logic for PointChoosing.xaml
	/// </summary>
	public partial class PointChoosing : UserControl
	{
		Label[][] labels = new Label[3][];
		Simer timer;
		int[] difficulty = new int[3];
		public PointChoosing()
		{
			InitializeComponent();
			for (int i = 0; i < 3; i++) labels[i] = new Label[3];
			labels[0][0] = lbl1_10;
			labels[0][1] = lbl1_20;
			labels[0][2] = lbl1_30;
			labels[1][0] = lbl2_10;
			labels[1][1] = lbl2_20;
			labels[1][2] = lbl2_30;
			labels[2][0] = lbl3_10;
			labels[2][1] = lbl3_20;
			labels[2][2] = lbl3_30;
			for (int i = 0; i < 3; i++) for (int j = 0; j < 3; j++)
				labels[i][j].Visibility = Visibility.Hidden;

			timer = new Simer(206);
			timer.Tick += timer_Tick;
		}

		int[] times = { 7, 73, 140 };
		public void timer_Tick(int time, bool done)
		{
			int pos = 2;
			for (int i = 0; i < 3; i++)
				if (time < times[i]) { pos = i - 1; break; }
			if (pos == -1) return;
			labels[pos][difficulty[pos]].Visibility = Visibility.Visible;
			if (done) 
				for (int i = 0; i < 3; i++) for (int j = 0; j < 3; j++)
					labels[i][j].Visibility = Visibility.Hidden;
		}
		public void Play(int[] difficulty)
		{
			this.difficulty = difficulty;
			for (int i = 0; i < 3; i++) for (int j = 0; j < 3; j++)
				labels[i][j].Visibility = Visibility.Hidden;
			timer.Start();
		}
	}
}
