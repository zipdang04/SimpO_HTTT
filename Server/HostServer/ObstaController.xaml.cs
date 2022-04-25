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
		List<Button> buttons = new List<Button>();
		PlayerClass playerClass { get; set; }
		public PointsControl pointsControl;
		public AnswersControl answersControl;
		PlayerNetwork playerNetwork;

		bool[] hasBelled = new bool[4] { false, false, false, false };
		int playerWinner = NaN;
		int remainingPoint, cntRow;
		int currentRow = NaN;

		Simer timer, timerLast15;
		const int timeLimit = 1600;

		bool stopAllow = false;

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
			timerLast15 = new Simer(timeLimit);
			timerLast15.Tick += timerLast15_Tick;

			labels.Add(lblHN1); labels.Add(lblHN2); labels.Add(lblHN3); labels.Add(lblHN4); labels.Add(lblTT);
			buttons.Add(btnPic1); buttons.Add(btnPic2); buttons.Add(btnPic3); buttons.Add(btnPic4); buttons.Add(btnPicTT);
			lblAnswer.Content = obstaClass.keyword;
		}
		public void sendMessageToEveryone(string message)
		{
			foreach (KeyValuePair<int, IClientInfo> client in listener.GetConnectedClients()) {
				listener.SendMessage(client.Value.Id, message);
			}
		}

		void timerLast15_Tick(int time, bool done)
		{
			if (done) {
				stopAllow = true;
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
			sendMessageToEveryone(String.Format("OLPA VCNV BELLING {0}", player));
			Dispatcher.Invoke(() =>{
				stackPlayerList.Children.Add(new PlayerVCNVBelling(player, playerClass.names[player]));
			});
		}
		private void btnReset_Click(object sender, RoutedEventArgs e)
		{
			btnHN1.IsEnabled = true;btnHN2.IsEnabled = true;btnHN3.IsEnabled = true;btnHN4.IsEnabled = true; btnTT.IsEnabled = false;
			btnLast15s.IsEnabled = false; btnAll.IsEnabled = false;
			for (int i = 0; i < 5; i++) buttons[i].IsEnabled = false;
			btnStart.IsEnabled = false; btnShowAnswer.IsEnabled = false; btnConfirm.IsEnabled = false;
			answersControl.Reset();
			stackPlayerList.Children.Clear();

			for (int i = 0; i < 4; i++) hasBelled[i] = false;
			playerWinner = NaN;
			remainingPoint = 80; cntRow = 0;
			currentRow = NaN;
			stopAllow = false;

			sendMessageToEveryone("OLPA ANS SCENE VCNV");
			string command = string.Format("OLPA VCNV START {0}", HelperClass.MakeString(obstaClass.attach));
			for (int i = 0; i < 5; i++){
				int cntLetter = HelperClass.VCNV_CountLetter(obstaClass.questions[i].answer);
				labels[i].Content = cntLetter;
				command = String.Format(command + " {0}", cntLetter);
			}
			sendMessageToEveryone(command);
			sendMessageToEveryone(string.Format("OLPA VCNV KEY {0}", HelperClass.VCNV_CountLetter(obstaClass.keyword)));
		}

		public void PlayerAnswering(int player, string answer, int time)
		{
			if (stopAllow == false)
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
			string command = "OLPA ANS RES";
			for (int i = 0; i < 4; i++)
				command += " " + ((answersControl.checkBoxes[i].IsChecked == true) ? 1 : 0).ToString();
			sendMessageToEveryone(command);

			bool willOpen = false;
			for (int i = 0; i < 4; i++){
				if (answersControl.checkBoxes[i].IsChecked == true){
					playerClass.points[i] += 10;
					willOpen = true;
				}
			}
			sendMessageToEveryone(HelperClass.ServerPointCommand(playerClass.points));
			if (willOpen) {
				buttons[currentRow].IsEnabled = true;
				sendMessageToEveryone(String.Format("OLPA VCNV ENAROW {0} {1}", currentRow, HelperClass.MakeString(obstaClass.questions[currentRow].answer)));
			} else
				sendMessageToEveryone(String.Format("OLPA VCNV DISROW {0}", currentRow));

			btnConfirm.IsEnabled = false;
			if (cntRow == 5) btnLast15s.IsEnabled = true;
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
			string command = "OLPA ANS ANSWER";
			PlayerAnswers answers = answersControl.data.answers;
			for (int i = 0; i < 4; i++)
				command += " " + HelperClass.MakeString(answers.answers[i]);
			command += " TIME";
			for (int i = 0; i < 4; i++)
				command += " " + answers.times[i].ToString();
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
						sendMessageToEveryone("OLPA VCNV WINNER");
						btnAll_Click(this, null);
					}
					stackPlayerList.Children.Remove(control); i--;
					sendMessageToEveryone(string.Format("OLPA VCNV REMOVESTACK {0}", player));
				}
				else if (control.radioWrong.IsChecked == true){
					stackPlayerList.Children.Remove(control);
					i--;
					sendMessageToEveryone(string.Format("OLPA VCNV REMOVESTACK {0}", player));
				}
			}
		}

		private void btnPic1_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone("OLPA VCNV OPEN 0");
		}
		private void btnPic2_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone("OLPA VCNV OPEN 1");
		}
		private void btnPic3_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone("OLPA VCNV OPEN 2");
		}
		private void btnPic4_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone("OLPA VCNV OPEN 3");
		}

		private void btnIntro_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone("OLPA VCNV INTRO");
		}

		private void btnVCNV_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone("OLPA SCENE VCNV");
		}

		private void btnOpening_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone("OLPA VCNV OPENING");
		}

		private void btnVCNVPic_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone("OLPA VCNV SCENE PIC");
		}

		private void btnVCNVWord_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone("OLPA VCNV SCENE WORD");
		}

		private void btnAll_Click(object sender, RoutedEventArgs e)
		{
			for (int i = 0; i < 5; i++)
				sendMessageToEveryone(String.Format("OLPA VCNV OPEN {0}", i));
			for (int i = 0; i < 4; i++)
				sendMessageToEveryone(String.Format("OLPA VCNV ENAROW {0} {1}", i, HelperClass.MakeString(obstaClass.questions[i].answer)));
		}

		private void btnVCNVEmpty_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone("OLPA VCNV SCENE ahihihi");
		}

		private void btnLast15s_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone("OLPA VCNV LAST15");
			btnLast15s.IsEnabled = false;
			btnAll.IsEnabled = true;
			timerLast15.Start();
		}

		private void btnPicTT_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone("OLPA VCNV OPEN 4");
		}
	}
}
