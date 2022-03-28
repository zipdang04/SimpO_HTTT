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
	/// Interaction logic for FinishController.xaml
	/// </summary>
	public partial class FinishController : UserControl
	{
		public const int NaN = -1;

		DispatcherTimer timerMain;
		int timeRemaining;

		SimpleSocketTcpListener listener;
		PlayerClass playerClass { get; set; }
		public PointsControl pointsControl;
		PlayerNetwork playerNetwork;
		FinishClass finishClass;

		int playerTurn = NaN;
		int[] quesDifficulty = new int[3];

		int questionPtr = 0;

		RadioButton[][] chosen = new RadioButton[3][];

		public FinishController(SimpleSocketTcpListener listener, FinishClass finishClass, PlayerClass playerClass, PlayerNetwork playerNetwork)
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
			this.finishClass = finishClass;

			pointsControl = new PointsControl(playerClass);
			Grid.SetRow(pointsControl, 1);

			grdChoosePoint.Visibility = Visibility.Collapsed;
			mainGrid.IsEnabled = false;
		}

		void MakeDecision(int playerTurn) {
			this.playerTurn = playerTurn;
			btnS1.IsEnabled = false; btnS2.IsEnabled = false; btnS3.IsEnabled = false; btnS4.IsEnabled = false;

			grdChoosePoint.Visibility = Visibility.Visible;
			mainGrid.IsEnabled = true;
			for (int i = 0; i < 3; i++) for (int j = 0; j < 3; j++) chosen[i][j].IsChecked = false; 
		}
		private void btnS1_Click(object sender, RoutedEventArgs e){MakeDecision(0);}
		private void btnS2_Click(object sender, RoutedEventArgs e){MakeDecision(1);}
		private void btnS3_Click(object sender, RoutedEventArgs e){MakeDecision(2);}
		private void btnS4_Click(object sender, RoutedEventArgs e){MakeDecision(3);}

		private void btnFinish_Click(object sender, RoutedEventArgs e)
		{
			playerTurn = NaN;
			btnS1.IsEnabled = true; btnS2.IsEnabled = true; btnS3.IsEnabled = true; btnS4.IsEnabled = true;
			mainGrid.IsEnabled = false;
			// TODO: do somethiing else to refresh database
		}

		private void btnConfirmPts_Click(object sender, RoutedEventArgs e)
		{
			grdChoosePoint.Visibility = Visibility.Collapsed; mainGrid.IsEnabled = false;
			for (int i = 0; i < 3; i++) for (int j = 0; j < 3; j++)
				if (chosen[i][j].IsEnabled) quesDifficulty[i] = j;
		}
	}
}
