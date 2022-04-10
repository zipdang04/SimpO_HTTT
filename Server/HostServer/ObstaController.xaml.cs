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
		List<Label> labels = new List<Label>();
		PlayerClass playerClass { get; set; }
		public PointsControl pointsControl;
		public AnswersControl answersControl;
		PlayerNetwork playerNetwork;

		bool[] hasBelled = new bool[4] { false, false, false, false };
		int playerWinner = NaN;
		int remainingPoint, cntRow;
		int currentRow = NaN;

		Simer timer;
		const int timeLimit = 1600;

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

			timer = new Simer(timeLimit);
			timer.Tick += timer_Tick;

			labels.Add(lblHN1); labels.Add(lblHN2); labels.Add(lblHN3); labels.Add(lblHN4); labels.Add(lblTT);
			for (int i = 0; i < 5; i++) labels[i].Content = obstaClass.questions[i].answer;
			lblAnswer.Content = obstaClass.keyword;
		}
		public void sendMessageToEveryone(string message)
		{
			foreach (KeyValuePair<int, IClientInfo> client in listener.GetConnectedClients()) {
				listener.SendMessage(client.Value.Id, message);
			}
		}

		void timer_Tick(int time, bool done)
		{
			lblTime.Content = string.Format("{0:0.00}", time / 100.0);
			if (done) {
				timer.Stop();
				answersControl.IsEnabled = true;
				btnShowAnswer.IsEnabled = true;
				btnConfirm.IsEnabled = true;
			}
		}

		public void SomeoneBelling(int player)
		{
			if (hasBelled[player]) return;
			Dispatcher.Invoke(() =>{
				stackPlayerList.Children.Add(new PlayerVCNVBelling(player, playerClass.names[player]));
			});
		}
		private void btnReset_Click(object sender, RoutedEventArgs e)
		{
			btnHN1.IsEnabled = true;btnHN2.IsEnabled = true;btnHN3.IsEnabled = true;btnHN4.IsEnabled = true; btnTT.IsEnabled = false;
			btnStart.IsEnabled = false; btnShowAnswer.IsEnabled = false; btnConfirm.IsEnabled = false;
			answersControl.Reset();
			stackPlayerList.Children.Clear();

			for (int i = 0; i < 4; i++) hasBelled[i] = false;
			playerWinner = NaN;
			remainingPoint = 80; cntRow = 0;
			currentRow = NaN;
			
			string command = string.Format("OLPA VCNV START {0}", HelperClass.MakeString(obstaClass.attach));
			sendMessageToEveryone(command);
		}

		public void PlayerAnswering(int player, string answer, int time)
		{
			answersControl.SomeoneAnswering(player, answer, time);
		}

		void Prepare(int qIdx)
		{
			currentRow = qIdx; cntRow++;
			OQuestion question = obstaClass.questions[qIdx];
			questionBox.displayQA(question.question, question.answer);
			btnStart.IsEnabled = true;
			answersControl.Reset();

			if (cntRow == 5) remainingPoint -= 10;
			else if (cntRow > 1) remainingPoint -= 20;
			if (cntRow == 4) btnTT.IsEnabled = true;
			
			string command = string.Format("OLPA VCNV SHOW {0} {1}", qIdx, HelperClass.ServerJoinQA(question));
			sendMessageToEveryone(command);
		}

		private void btnStart_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone("OLPA VCNV TIME");

			timer.Start(); 
			btnStart.IsEnabled = false;
		}

		private void btnConfirm_Click(object sender, RoutedEventArgs e)
		{
			string command = "OLPA TT RES {0} {0} {0} {0}";
			for (int i = 0; i < 4; i++)
				command = string.Format(command, answersControl.checkBoxes[i].IsChecked);
			sendMessageToEveryone(command);

			bool willOpen = false;
			for (int i = 0; i < 4; i++){
				if (answersControl.checkBoxes[i].IsChecked == true){
					playerClass.points[i] += 10;
					willOpen = true;
				}
			}
			sendMessageToEveryone(HelperClass.ServerPointCommand(playerClass.points));
			if (willOpen) sendMessageToEveryone(string.Format("OLPA VCNV OPEN {0}", currentRow));
			btnConfirm.IsEnabled = false;
		}
		private void btnHN1_Click(object sender, RoutedEventArgs e)
		{
			btnHN1.IsEnabled = false;
			Prepare(0);
		}
		private void btnHN2_Click(object sender, RoutedEventArgs e)
		{
			btnHN2.IsEnabled = false;
			Prepare(1);
		}

		private void btnHN3_Click(object sender, RoutedEventArgs e)
		{
			btnHN3.IsEnabled = false;
			Prepare(2);
		}

		private void btnHN4_Click(object sender, RoutedEventArgs e)
		{
			btnHN4.IsEnabled = false;
			Prepare(3);
		}

		private void btnTT_Click(object sender, RoutedEventArgs e)
		{
			btnTT.IsEnabled = false;
			Prepare(4);
		}

		private void btnShowAnswer_Click(object sender, RoutedEventArgs e)
		{
			string command = "OLPA VCNV ANSWER {0} {0} {0} {0} TIME {0} {0} {0} {0}";
			PlayerAnswers answers = answersControl.data.answers;
			for (int i = 0; i < 4; i++)
				command = string.Format(command, HelperClass.MakeString(answers.answers[i]));
			for (int i = 0; i < 4; i++)
				command = string.Format(command, answers.times[i]);
			sendMessageToEveryone(command);
		}

		private void btnBellConfirm_Click(object sender, RoutedEventArgs e)
		{
			for (int i = 0; i < stackPlayerList.Children.Count; i++)
			{
				PlayerVCNVBelling control = (PlayerVCNVBelling) stackPlayerList.Children[i];
				int player = control.playerIndex;
				if (control.radioCorrect.IsChecked == true){
					if (playerWinner == NaN){
						playerWinner = player;
						playerClass.points[player] += remainingPoint;
						sendMessageToEveryone(HelperClass.ServerPointCommand(playerClass.points));
					}
					stackPlayerList.Children.Remove(control); i--;
				}
				else if (control.radioWrong.IsChecked == true){
					stackPlayerList.Children.Remove(control);
					i--;
				}
			}
		}
	}
}
