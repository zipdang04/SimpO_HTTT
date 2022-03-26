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

using SimpleSockets.Server;
using Server.QuestionClass;
using SimpleSockets.Messaging.Metadata;
using Server.Information;
using Server.HostServer.Components;
using System.Windows.Threading;

namespace Server.HostServer
{
	/// <summary>
	/// Interaction logic for ObstaController.xaml
	/// </summary>
	public partial class ObstaController : UserControl
	{
		public const int NaN = -1;

		DispatcherTimer timer;
		int timeRemaining;

		SimpleSocketTcpListener listener;
		ObstacleClass obstaClass;
		PlayerClass playerClass { get; set; }
		public PointsControl pointsControl;
		PlayerNetwork playerNetwork;

		public ObstaController(SimpleSocketTcpListener listener, ObstacleClass obstaClass, PlayerClass playerClass, PlayerNetwork playerNetwork)
		{
			InitializeComponent();
			this.listener = listener;
			this.obstaClass = obstaClass;
			this.playerClass = playerClass;
			this.playerNetwork = playerNetwork;

			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += timer_Tick;
		}

		void timer_Tick(object? sender, EventArgs e)
		{
			timeRemaining--;
			//lblTime.Content = timeRemaining.ToString();
			if (timeRemaining == 0) {
				timer.Stop();
			}
		}
	}
}
