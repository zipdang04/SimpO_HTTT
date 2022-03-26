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
using System.Windows.Threading;

using SimpleSockets.Server;
using Server.QuestionClass;
using SimpleSockets.Messaging.Metadata;
using Server.Information;
using Server.HostServer.Components;

namespace Server.HostServer
{
	/// <summary>
	/// Interaction logic for StartController.xaml
	/// </summary>
	public partial class StartController : UserControl
	{
		public const int NaN = -1;
		
		DispatcherTimer timerMain, timer3s;
		int timeMainRemaining, t3Remaining = 3;

		SimpleSocketTcpListener listener;
		StartClass startClass;
		PlayerClass playerClass { get; set; }
		public PointsControl pointsControl;
		PlayerNetwork playerNetwork;

		int playerBelling = NaN;
		int questionTurn = NaN;
		int playerBlocked = NaN;
		int questionPtr = 0;

		public StartController(SimpleSocketTcpListener listener, StartClass startClass, PlayerClass playerClass, PlayerNetwork playerNetwork)
		{
			InitializeComponent();
			this.listener = listener;
			this.startClass = startClass;
			this.playerClass = playerClass;
			this.playerNetwork = playerNetwork;
			pointsControl = new PointsControl(playerClass);
			gridPoint.Children.Add(pointsControl);

			t3Remaining = 3;
			timerMain = new DispatcherTimer();
			timerMain.Interval = TimeSpan.FromSeconds(1);
			timerMain.Tick += timerMain_Tick;
			timer3s = new DispatcherTimer();
			timer3s.Interval = TimeSpan.FromSeconds(1);
			timer3s.Tick += timer3s_Tick;
			this.playerClass = playerClass;
		}

		public void sendMessageToEveryone(string message)
		{
			foreach (KeyValuePair<int, IClientInfo> client in listener.GetConnectedClients()) {
				listener.SendMessage(client.Value.Id, message);
			}
		}

		void ResetBell()
		{
			for (int i = 0; i < 4; i++)
				pointsControl.BackToNormal(i);
		}

		public void showQuestion()
		{
			playerBelling = NaN;
			btn3s.IsEnabled = true;
			if (timeMainRemaining == 0) {
				lblStartQuestion.Content = "";
				lblStartAnswer.Content = "";
				return;
			}

			OQuestion question;
			if (questionPtr >= StartClass.PART_QUES[questionTurn]) {
				question = new OQuestion();
				question.question = "Hết câu hỏi";
				question.answer = "Hết câu hỏi";
			}
			else {
				question = startClass.questions[questionTurn][questionPtr]; questionPtr++;
			}

			lblStartQuestion.Content = question.question;
			lblStartAnswer.Content = question.answer;

			sendMessageToEveryone("OLPA KD QUES " + HelperClass.ServerJoinQA(question));
			if (playerBlocked != NaN)
				listener.SendMessage(playerNetwork.clients[playerBlocked].Id, "OLPA KD DIS");
		}

		public void SomeoneBelling(int player)
		{
			if (questionTurn == NaN) return;
			if (playerBelling != NaN) return;
			if (player == playerBlocked) return;
			if (t3Remaining == 0) return;

			playerBelling = player;
			sendMessageToEveryone("OLPA KD DIS");
			pointsControl.ChoosePlayer(player);
			if (timer3s.IsEnabled) timer3s.Stop();
			t3Remaining = 3;
		}

		private void btnStartTurn1_Click(object sender, RoutedEventArgs e)
		{
			timeMainRemaining = StartClass.PART_TIME[0];
			questionTurn = 0; questionPtr = 0;
			showQuestion(); timerMain.Start();
		}

		private void btnStartTurn2_Click(object sender, RoutedEventArgs e)
		{
			questionTurn = 1; questionPtr = 0;
		}

		private void btnStartTurn3_Click(object sender, RoutedEventArgs e)
		{
			questionTurn = 2; questionPtr = 0;
		}

		private void btnCorrect_Click(object sender, RoutedEventArgs e)
		{
			if (playerBelling == NaN) return;
			playerClass.points[playerBelling] += 10;
			sendMessageToEveryone(HelperClass.ServerPointCommand(playerClass.points));

			ResetBell();
			showQuestion();
		}

		private void btnWrong_Click(object sender, RoutedEventArgs e)
		{
			ResetBell();
			if (playerBelling != NaN) {
				playerBlocked = playerBelling;
				playerClass.points[playerBelling] -= 5;
				pointsControl.DisablePlayer(playerBlocked);
			}
			showQuestion();
		}

		private void btn3s_Click(object sender, RoutedEventArgs e)
		{
			timer3s.Start();
			btn3s.IsEnabled = false;
		}

		void timerMain_Tick(object? sender, EventArgs e)
		{
			timeMainRemaining--;
			lblTime.Content = timeMainRemaining.ToString();
			if (timeMainRemaining == 0) {
				timerMain.Stop();
				if(timer3s.IsEnabled) timer3s.Stop();
			}
		}
		void timer3s_Tick(object? sender, EventArgs e)
		{
			t3Remaining--;
			if (t3Remaining == 0) {
				ResetBell();
				playerBlocked = NaN;
				timer3s.Stop();
				t3Remaining = 3;
				showQuestion();
			}
		}
	}
}
