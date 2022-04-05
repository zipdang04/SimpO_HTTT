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

		SimpleSocketTcpListener listener;
		ObstacleClass obstaClass;
		PlayerClass playerClass { get; set; }
		public PointsControl pointsControl;
		public AnswersControl answersControl;
		PlayerNetwork playerNetwork;

		bool[] hasBelled = new bool[4] { false, false, false, false };
		int playerWinner = NaN;
		int remainingPoint;

		DispatcherTimer timer;
		int timeLimit;
		DateTime timeBegin;
		int getTime()
		{
			TimeSpan span = DateTime.Now - timeBegin;
			return (span.Seconds * 1000 + span.Milliseconds) / 10;
		}

		public ObstaController(SimpleSocketTcpListener listener, ObstacleClass obstaClass, PlayerClass playerClass, PlayerNetwork playerNetwork)
		{
			InitializeComponent();
			this.listener = listener;
			this.obstaClass = obstaClass;
			this.playerClass = playerClass;
			this.playerNetwork = playerNetwork;

			answersControl = new AnswersControl(playerClass);
			pointsControl = new PointsControl(playerClass);
			gridAnswer.Children.Add(answersControl);
			gridPoint.Children.Add(pointsControl);

			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += timer_Tick;
		}
		public void sendMessageToEveryone(string message)
		{
			foreach (KeyValuePair<int, IClientInfo> client in listener.GetConnectedClients()) {
				listener.SendMessage(client.Value.Id, message);
			}
		}

		void timer_Tick(object? sender, EventArgs e)
		{
			int time = getTime();
			lblTime.Content = string.Format("{0:0.00}", time / 100.0);
			if (time >= timeLimit) {
				timer.Stop();
				answersControl.IsEnabled = true;
				btnShowAnswer.IsEnabled = true;
				btnConfirm.IsEnabled = true;
			}
		}

		public void SomeoneBelling(int player)
		{
			if (hasBelled[player]) return;
			stackPlayerList.Children.Add(new PlayerVCNVBelling(player, playerClass.names[player]));
		}
		private void btnReset_Click(object sender, RoutedEventArgs e)
		{
			string command = string.Format("OLPA VCNV START {0}", HelperClass.MakeString(obstaClass.attach));
			sendMessageToEveryone(command);
			for (int i = 0; i < 4; i++) hasBelled[i] = false;
			playerWinner = NaN;
			remainingPoint = 80;
		}

		public void PlayerAnswering(int player, string answer, int time)
		{
			answersControl.SomeoneAnswering(player, answer, time);
		}

		void Prepare(int qIdx)
		{
			OQuestion question = obstaClass.questions[0];
			questionBox.displayQA(question.question, question.answer);
			btnStart.IsEnabled = true;

			if (qIdx == 4) remainingPoint -= 10;
			else remainingPoint -= 20;
			
			string command = string.Format("OLPA VCNV SHOW {0} {1}", qIdx, HelperClass.ServerJoinQA(question));
			sendMessageToEveryone(command);
		}
		private void btnHN1_Click(object sender, RoutedEventArgs e)
		{
			btnHN1.IsEnabled = false;
			Prepare(0);
		}

		private void btnStart_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone("OLPA VCNV TIME");

			timer.Start(); timeBegin = DateTime.Now;
			btnStart.IsEnabled = false;
		}
	}
}
