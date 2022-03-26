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

using Server.Information;
using SimpleSockets.Client;

namespace Server.HostServer.Components
{
	/// <summary>
	/// Interaction logic for PointsControl.xaml
	/// </summary>
	public partial class PointsControl : UserControl
	{
		readonly Brush NORMAL = new SolidColorBrush(Color.FromArgb(100, 0, 0, 0));
		readonly Brush DISABLE = new SolidColorBrush(Color.FromArgb(100, 249, 160, 164));
		readonly Brush CHOOSING = new SolidColorBrush(Color.FromArgb(100, 0, 219, 0));

		//public PlayerClass points { get; set; }
		List<Label> playersName;
		List<Label> playersPoint;
		PlayerClass points;

		public PointsControl(PlayerClass points)
		{
			InitializeComponent();
			//this.points = points;
			playersName = new List<Label> { lblName1, lblName2, lblName3, lblName4 };
			playersPoint = new List<Label> { lblPoint1, lblPoint2, lblPoint3, lblPoint4 };
			for (int i = 0; i < 4; i++)
				playersName[i].Background = NORMAL;
			this.points = points;
			DataContext = this.points;
		}

		public void ChoosePlayer(int player)
		{
			Dispatcher.Invoke(() => {
				playersName[player].Background = CHOOSING;
			});
		}
		public void DisablePlayer(int player)
		{
			Dispatcher.Invoke(() => {
				playersName[player].Background = DISABLE;
			});
		}
		public void BackToNormal(int player)
		{
			Dispatcher.Invoke(() => {
				playersName[player].Background = NORMAL;
			});
		}
	}
}
