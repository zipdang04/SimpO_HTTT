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

		Simer timer;

		public StartController(SimpleSocketTcpListener listener, StartClass startClass, PlayerClass playerClass, PlayerNetwork playerNetwork)
		{
			InitializeComponent();
			this.listener = listener;
			this.startClass = startClass;
			this.playerClass = playerClass;
			this.playerNetwork = playerNetwork;
			pointsControl = new PointsControl(playerClass);
			gridPoint.Children.Add(pointsControl);

			timer = new Simer(6000); // 60s + 1
			timer.Tick += timer_Tick;
			this.playerClass = playerClass;
			
			btnCorrect.IsEnabled = false;
			btnWrong.IsEnabled = false;
			btnDone.IsEnabled = false;
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
			if (timer.IsEnabled == false) {
				questionBox.displayQA("", "");
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

			questionBox.displayQA(question.question, question.answer);
			sendMessageToEveryone("OLPA KD QUES " + HelperClass.ServerJoinQA(question));
			gridMain.Focus();
		}

		private void StartTurn(int player)
		{
			playerTurn = player; questionPtr = 0;
			sendMessageToEveryone(string.Format("OLPA KD START {0}", player));
			btnStartTIME.IsEnabled = true;
			lblTurn.Content = string.Format("Lượt {0}", player + 1);
		}

		private void btnStartTIME_Click(object sender, RoutedEventArgs e)
		{
			btnStartTurn1.IsEnabled = false;
			btnStartTurn2.IsEnabled = false;
			btnStartTurn3.IsEnabled = false;
			btnStartTurn4.IsEnabled = false;
			btnStartTIME.IsEnabled = false;
			btnCorrect.IsEnabled = true;
			btnWrong.IsEnabled = true;
			sendMessageToEveryone("OLPA KD TIME");
			timer.Start();
			showQuestion();
			gridMain.Focus();
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
			if (questionPtr <= StartClass.QUES_CNT) {
				playerClass.ChangePoint(playerTurn, 10);
				sendMessageToEveryone("OLPA KD CORRECT");
				sendMessageToEveryone(HelperClass.ServerPointCommand(playerClass.points));
			}

			showQuestion();
		}

		private void btnWrong_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone("OLPA KD WRONG");
			showQuestion();
		}


		void timer_Tick(int time, bool done)
		{
			lblTime.Content = string.Format("{0:0.00}", (time) / 100.0);
			if (done) {
				btnDone.IsEnabled = true;
				btnCorrect.IsEnabled = false;
				btnWrong.IsEnabled = false;
			}
		}
		

		private void btnChangeScene_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone("OLPA SCENE KD");
		}

		private void btnIntro_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone("OLPA KD INTRO");
		}
		private void btnOpening_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone("OLPA KD OPENING");
		}

		private void btnBlank_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone("OLPA KD BLANK");
		}

		private void btnDone_Click(object sender, RoutedEventArgs e)
		{
			btnDone.IsEnabled = false;
			playerTurn = -1;

			
			btnStartTurn1.IsEnabled = true;
			btnStartTurn2.IsEnabled = true;
			btnStartTurn3.IsEnabled = true;
			btnStartTurn4.IsEnabled = true;
			sendMessageToEveryone("OLPA KD DONE");
		}

		bool pressed = false;
		private void gridMain_KeyDown(object sender, KeyEventArgs e) {
			if (pressed) return; pressed = true;
			if (e.Key == Key.Q && btnCorrect.IsEnabled)
				btnCorrect_Click(sender, new RoutedEventArgs());
			else if (e.Key == Key.E && btnWrong.IsEnabled)
				btnWrong_Click(sender, new RoutedEventArgs());
		}

		private void gridMain_KeyUp(object sender, KeyEventArgs e) {
			pressed = false;
		}
	}
}
