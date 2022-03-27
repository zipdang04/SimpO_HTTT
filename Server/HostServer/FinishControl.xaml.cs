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
	/// Interaction logic for FinishControl.xaml
	/// </summary>
	public partial class FinishControl : UserControl
	{
		public const int NaN = -1;

		DispatcherTimer timerMain;
		int timeRemaining;

		SimpleSocketTcpListener listener;
		PlayerClass playerClass { get; set; }
		public PointsControl pointsControl;
		PlayerNetwork playerNetwork;

		int playerTurn = NaN;
		int questionPtr = 0;

		RadioButton[][] chosen = new RadioButton[3][];

		public FinishControl(SimpleSocketTcpListener listener, PlayerClass playerClass, PlayerNetwork playerNetwork)
		{
			InitializeComponent();
			for (int i = 0; i < 3; i++) {
				chosen[i] = new RadioButton[3];
				for (int j = 0; j < 3; j++) {
					chosen[i][j] = new RadioButton();
					chosen[i][j].GroupName = i.ToString();

					grdChoosePoint.Children.Add(chosen[i][j]);
					Grid.SetRow(chosen[i][j], i + 1);
					Grid.SetColumn(chosen[i][j], j + 1);
				}
			}

			this.listener = listener;
			this.playerClass = playerClass;
			this.playerNetwork = playerNetwork;

			pointsControl = new PointsControl(playerClass);
			Grid.SetRow(pointsControl, 1);

			grdChoosePoint.Visibility = Visibility.Collapsed;
		}
	}
}
