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

		//int timeMainRemaining;

		SimpleSocketTcpListener listener;
		StartClass startClass;
		PlayerClass playerClass { get; set; }
		public PointsControl pointsControl;
		PlayerNetwork playerNetwork;

		int playerTurn = NaN;
		int questionPtr = 0;

		//DispatcherTimer timer;
		DispatcherTimer timerMain;
		const int timeLimit = 6000;
		DateTime timeBegin;
		int getTime()
		{
			TimeSpan span = DateTime.Now - timeBegin;
			return (span.Seconds * 1000 + span.Milliseconds) / 10;
		}

		public StartController(SimpleSocketTcpListener listener, StartClass startClass, PlayerClass playerClass, PlayerNetwork playerNetwork)
		{
			InitializeComponent();
			this.listener = listener;
			this.startClass = startClass;
			this.playerClass = playerClass;
			this.playerNetwork = playerNetwork;
			pointsControl = new PointsControl(playerClass);
			gridPoint.Children.Add(pointsControl);

			timerMain = new DispatcherTimer();
			timerMain.Interval = TimeSpan.FromSeconds(1);
			timerMain.Tick += timerMain_Tick;
			this.playerClass = playerClass;
			
			btnCorrect.IsEnabled = false;
			btnWrong.IsEnabled = false;
			playerTurn = NaN;
		}

		public void sendMessageToEveryone(string message)
		{
			foreach (KeyValuePair<int, IClientInfo> client in listener.GetConnectedClients()) {
				listener.SendMessage(client.Value.Id, message);
			}
		}

		public void showQuestion()
		{
			if (timerMain.IsEnabled == false) {
				lblStartQuestion.Content = "";
				lblStartAnswer.Content = "";
				return;
			}

			OQuestion question;
			if (questionPtr >= StartClass.QUES_CNT) {
				question = new OQuestion();
				question.question = "Hết câu hỏi";
				question.answer = "Hết câu hỏi";
			}
			else {
				question = startClass.questions[playerTurn][questionPtr]; questionPtr++;
			}

			lblStartQuestion.Content = question.question;
			lblStartAnswer.Content = question.answer;

			sendMessageToEveryone("OLPA KD QUES " + HelperClass.ServerJoinQA(question));
		}

		private void StartTurn(int player)
		{
			playerTurn = player; questionPtr = 0;
			timeBegin = DateTime.Now; timerMain.Start(); 
			showQuestion();
			btnCorrect.IsEnabled = true;
			btnWrong.IsEnabled = true;
			btnStartTurn1.IsEnabled = false;
			btnStartTurn2.IsEnabled = false;
			btnStartTurn3.IsEnabled = false;
			btnStartTurn4.IsEnabled = false;
		}

		private void btnStartTurn1_Click(object sender, RoutedEventArgs e)
		{
			StartTurn(0);
		}

		private void btnStartTurn2_Click(object sender, RoutedEventArgs e)
		{
			StartTurn(1);
		}

		private void btnStartTurn3_Click(object sender, RoutedEventArgs e)
		{
			StartTurn(2);
		}

		private void btnStartTurn4_Click(object sender, RoutedEventArgs e)
		{
			StartTurn(3);
		}

		private void btnCorrect_Click(object sender, RoutedEventArgs e)
		{
			if (playerTurn == NaN) return;
			if (questionPtr < StartClass.QUES_CNT) {
				playerClass.points[playerTurn] += 10;
				sendMessageToEveryone(HelperClass.ServerPointCommand(playerClass.points));
			}

			showQuestion();
		}

		private void btnWrong_Click(object sender, RoutedEventArgs e)
		{
			showQuestion();
		}


		void timerMain_Tick(object? sender, EventArgs e)
		{
			int time = getTime();
			lblTime.Content = string.Format("{0:0.00}", time / 100.0);
			if (time >= timeLimit) {
				playerTurn = -1;
				timerMain.Stop();

				btnCorrect.IsEnabled = false;
				btnWrong.IsEnabled = false;
				btnStartTurn1.IsEnabled = true;
				btnStartTurn2.IsEnabled = true;
				btnStartTurn3.IsEnabled = true;
				btnStartTurn4.IsEnabled = true;
			}
		}
		
	}
}
