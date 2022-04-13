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
	/// 
	public enum StarState
	{
		NOPE,
		USING,
		USED
	}

	public partial class FinishController : UserControl
	{
		public const int NaN = -1;

		Simer timerMain, timer5s, timerPrac;

		SimpleSocketTcpListener listener;
		PlayerClass playerClass { get; set; }
		public PointsControl pointsControl;
		PlayerNetwork playerNetwork;
		FinishClass finishClass;

		int playerTurn = NaN, playerSuck = NaN;
		int[] quesDifficulty = new int[3];
		int[] quesPosition = new int[3];

		int questionPtr = 0, currentPtr = NaN, difficulty, score;
		bool practiceMode = false;
		bool isSucking = false; // hút
		StarState starState = StarState.NOPE;

		RadioButton[][] chosen = new RadioButton[3][];
		List<ListBox> lstPoints = new List<ListBox>();
		List<ListBox> lstNos = new List<ListBox>();

		public FinishController(SimpleSocketTcpListener listener, FinishClass finishClass, PlayerClass playerClass, PlayerNetwork playerNetwork)
		{
			InitializeComponent();
			timerMain = new Simer(); timerMain.Tick += TimerMain_Tick;
			timer5s = new Simer(500); timer5s.Tick += Timer5s_Tick;	
			timerPrac = new Simer(); timerPrac.Tick += TimerPrac_Tick;

			lstPoints.Add(lstPoint1); lstPoints.Add(lstPoint2); lstPoints.Add(lstPoint3);
			lstNos.Add(lstNo1); lstNos.Add(lstNo2); lstNos.Add(lstNo3);

			this.listener = listener;
			this.playerClass = playerClass;
			this.playerNetwork = playerNetwork;
			this.finishClass = finishClass;

			pointsControl = new PointsControl(playerClass);
			grid.Children.Add(pointsControl);
			Grid.SetRow(pointsControl, 1);
			pointsControl.Visibility = Visibility.Visible;

			grdChoosePoint.Visibility = Visibility.Collapsed;
			mainGrid.IsEnabled = false;
		}

		public void sendMessageToEveryone(string message)
		{
			foreach (KeyValuePair<int, IClientInfo> client in listener.GetConnectedClients())
				listener.SendMessage(client.Value.Id, message);
		}

		private void TimerMain_Tick(int time, bool done)
		{
			lblTimer.Content = string.Format("{0:0.00}", time / 100.0);
			if (done){
				timerMain.Stop();
				btnPrac.IsEnabled = true;
				btnCorrect.IsEnabled = true;
				btnWrong.IsEnabled = true;
			}
		}
		private void Timer5s_Tick(int time, bool done)
		{
			lblTimer.Content = string.Format("{0:0.00}", time / 100.0);
			if (done)
			{
				timer5s.Stop();
				sendMessageToEveryone("OLPA VD LOCK");
				if (playerSuck == NaN) {
					if (starState == StarState.NOPE) btnStar.IsEnabled = true;
					if (starState == StarState.USING) starState = StarState.USED;
				} else {
					if (practiceMode) btnSuckPrac.IsEnabled = true;
					btnSuckCorrect.IsEnabled = true;
					btnSuckWrong.IsEnabled = true;
				}
				
			}
		}
		private void TimerPrac_Tick(int time, bool done)
		{
			lblTimer.Content = string.Format("{0:0.00}", time / 100.0);
			if (done)
			{
				timerPrac.Stop();
				if (isSucking)
				{
					btnSuckCorrect.IsEnabled = true;
					btnSuckWrong.IsEnabled = true;
				} else
				{
					btnCorrect.IsEnabled = true;
					btnWrong.IsEnabled = true;
				}
			}
		}

		void MakeDecision(int playerTurn) {
			this.playerTurn = playerTurn;
			btnS1.IsEnabled = false; btnS2.IsEnabled = false; btnS3.IsEnabled = false; btnS4.IsEnabled = false;

			grdChoosePoint.Visibility = Visibility.Visible;
			mainGrid.IsEnabled = true;
			for (int i = 0; i < 3; i++) {
				lstPoints[i].UnselectAll();
				lstNos[i].SelectedIndex = i;
			}
			sendMessageToEveryone(String.Format("OLPA VD CHOOSING {0}", playerTurn));
			pointsControl.ChoosePlayer(playerTurn);
		}
		private void btnS1_Click(object sender, RoutedEventArgs e){MakeDecision(0);}
		private void btnS2_Click(object sender, RoutedEventArgs e){MakeDecision(1);}
		private void btnS3_Click(object sender, RoutedEventArgs e){MakeDecision(2);}
		private void btnS4_Click(object sender, RoutedEventArgs e){MakeDecision(3);}

		private void btnFinish_Click(object sender, RoutedEventArgs e)
		{
			playerTurn = NaN; playerSuck = NaN;
			btnS1.IsEnabled = true; btnS2.IsEnabled = true; btnS3.IsEnabled = true; btnS4.IsEnabled = true;
			mainGrid.IsEnabled = false;

			btnShowQuestion.IsEnabled = true; 
			starState = StarState.NOPE; btnStar.IsEnabled = true;
			btnStart.IsEnabled = btnPrac.IsEnabled = btnCorrect.IsEnabled = btnWrong.IsEnabled = true;
			btn5s.IsEnabled = btnSuckPrac.IsEnabled = btnSuckCorrect.IsEnabled = btnSuckWrong.IsEnabled = true;

			questionPtr = 0; currentPtr = NaN; 
			practiceMode = false; isSucking = false; 
			
			// TODO: do somethiing else to refresh database
			for (int i = 0; i < 4; i++) pointsControl.BackToNormal(i);
			sendMessageToEveryone("OLPA VD ENDING");
		}

		private void btnConfirmPts_Click(object sender, RoutedEventArgs e)
		{
			for (int i = 0; i < 3; i++)
				if (lstPoints[i].SelectedItem == null || lstNos[i].SelectedItem == null) {
					MessageBox.Show(String.Format("Lượt {0} chưa chọn", i + 1), "chưa được chọn", MessageBoxButton.OK);
					return;
				}
			for (int i = 0; i < 3; i++) for (int j = i + 1; j < 3; j++) 
				if (lstPoints[i].SelectedIndex == lstPoints[j].SelectedIndex && lstNos[i].SelectedIndex == lstNos[j].SelectedIndex) {
					MessageBox.Show(String.Format("Lượt {0} và {1} cấu hình trùng nhau", i + 1, j + 1), "trùng", MessageBoxButton.OK);
					return;
				}

			grdChoosePoint.Visibility = Visibility.Collapsed; mainGrid.IsEnabled = true;
			for (int i = 0; i < 3; i++) {
				quesDifficulty[i] = lstPoints[i].SelectedIndex;
				quesPosition[i] = lstNos[i].SelectedIndex;
			}
			sendMessageToEveryone(string.Format("OLPA VD CHOSEN {0} {1} {2}", quesDifficulty[0], quesDifficulty[1], quesDifficulty[2]));
		}

		private void btnShowQuestion_Click(object sender, RoutedEventArgs e)
		{
			currentPtr = questionPtr; questionPtr++;
			if (questionPtr == 3) btnShowQuestion.IsEnabled = false;
			difficulty = quesDifficulty[currentPtr]; score = FinishClass.QUES_POINT[difficulty];
			
			OQuestion question = finishClass.questions[playerTurn][difficulty][quesPosition[currentPtr]];
			questionBox.displayQA(question.question, question.answer);
			sendMessageToEveryone(String.Format("OLPA VD TURN {0}", currentPtr));
			sendMessageToEveryone(string.Format("OLPA VD QUES {0}", HelperClass.ServerJoinQA(question)));
			
			btnStart.IsEnabled = true; 
			
			btnStar.IsEnabled = false;
			btnPrac.IsEnabled = false; btnCorrect.IsEnabled = false; btnWrong.IsEnabled = false;
			btn5s.IsEnabled = false; btnSuckCorrect.IsEnabled = false; btnSuckPrac.IsEnabled = false; btnSuckWrong.IsEnabled = false;
			
			practiceMode = false; isSucking = false;
			playerSuck = NaN;
			for (int i = 0; i < 4; i++) pointsControl.BackToNormal(i);
			pointsControl.ChoosePlayer(playerTurn);
		}

		private void btnStart_Click(object sender, RoutedEventArgs e)
		{
			btnStart.IsEnabled = false;
			timerMain.Start(TimeSpan.FromSeconds(FinishClass.QUES_TIME[difficulty]));
			sendMessageToEveryone(String.Format("OLPA VD TIME {0}", FinishClass.QUES_TIME[difficulty] * 100));
		}

		private void btnStar_Click(object sender, RoutedEventArgs e)
		{
			starState = StarState.USING;
			btnStar.IsEnabled = false;
			sendMessageToEveryone("OLPA VD STAR");
		}

		private void btnPrac_Click(object sender, RoutedEventArgs e)
		{
			timerMain.Start(TimeSpan.FromSeconds(FinishClass.PRAC_TIME[difficulty]));
			practiceMode = true;
			btnPrac.IsEnabled = false;
			sendMessageToEveryone("OLPA VD PRAC MAIN");
			btnCorrect.IsEnabled = false;
			btnWrong.IsEnabled = false;
		}

		private void btnCorrect_Click(object sender, RoutedEventArgs e)
		{
			int add = score;
			if (starState == StarState.USING){
				add *= 2;
				starState = StarState.USED;
			}
			playerClass.points[playerTurn] += add;
			
			btnCorrect.IsEnabled = false;
			btnWrong.IsEnabled = false;
			if (starState == StarState.NOPE) btnStar.IsEnabled = true;
			sendMessageToEveryone("OLPA VD CORRECT");
			sendMessageToEveryone(HelperClass.ServerPointCommand(playerClass.points));
		}

		private void btnVeDich_Click(object sender, RoutedEventArgs e) { sendMessageToEveryone("OLPA SCENE VD"); }
		private void btnMedia_Click(object sender, RoutedEventArgs e) { sendMessageToEveryone("OLPA VD MEDIA"); }
		private void btnScenePoint_Click(object sender, RoutedEventArgs e) { sendMessageToEveryone("OLPA VD SCENE POINT"); }
		private void btnSceneQues_Click(object sender, RoutedEventArgs e) { sendMessageToEveryone("OLPA VD SCENE QUES"); }
		private void btnScenePrac_Click(object sender, RoutedEventArgs e) { sendMessageToEveryone("OLPA VD SCENE PRAC"); }
		private void btnSceneMedia_Click(object sender, RoutedEventArgs e) { sendMessageToEveryone("OLPA VD SCENE MEDIA"); }
		private void btnIntro_Click(object sender, RoutedEventArgs e) { sendMessageToEveryone("OLPA VD INTRO"); }
		private void btnOpening_Click(object sender, RoutedEventArgs e) { sendMessageToEveryone("OLPA VD OPENING"); }

		private void btnWrong_Click(object sender, RoutedEventArgs e)
		{
			if (starState == StarState.USING)
				playerClass.points[playerTurn] -= FinishClass.QUES_POINT[difficulty];
			btnCorrect.IsEnabled = false;
			btnWrong.IsEnabled = false;

			btn5s.IsEnabled = true;
			sendMessageToEveryone("OLPA VD WRONG");
			sendMessageToEveryone(HelperClass.ServerPointCommand(playerClass.points));
		}

		private void btn5s_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone("OLPA VD UNLOCK");
			if (playerNetwork.clients[playerTurn] != null)
				listener.SendMessage(playerNetwork.clients[playerTurn].Id, "OLPA VD LOCK");
			btn5s.IsEnabled = false;
			isSucking = true;
			timer5s.Start();
		}

		public void SomeoneSucking(int index){
			if (timer5s.IsEnabled == true && playerSuck == NaN)
			{
				playerSuck = index;
				pointsControl.DisablePlayer(playerSuck);
				sendMessageToEveryone(String.Format("OLPA VD SUCK {0}", playerSuck));
			}
		}

		private void btnSuckPrac_Click(object sender, RoutedEventArgs e)
		{
			timerPrac.Start(TimeSpan.FromSeconds(FinishClass.REMAIN_PRAC_TIME[difficulty]));
			sendMessageToEveryone("OLPA VD PRAC SUCK");
			timerPrac.Start();
			btnSuckCorrect.IsEnabled = false;
			btnSuckWrong.IsEnabled = false;
		}

		private void btnSuckCorrect_Click(object sender, RoutedEventArgs e)
		{
			playerClass.points[playerSuck] += score;
			if (starState != StarState.USING)
				playerClass.points[playerTurn] -= score;
			if (starState == StarState.NOPE) btnStar.IsEnabled = true;
			sendMessageToEveryone("OLPA VD CORRECT");
			sendMessageToEveryone(HelperClass.ServerPointCommand(playerClass.points));
			if (starState == StarState.USING) starState = StarState.USED;
		}

		private void btnSuckWrong_Click(object sender, RoutedEventArgs e)
		{
			playerClass.points[playerSuck] -= score / 2;
			btnSuckCorrect.IsEnabled = false;
			btnSuckWrong.IsEnabled = false;
			if (starState == StarState.NOPE) btnStar.IsEnabled = true;
			sendMessageToEveryone("OLPA VD WRONG");
			sendMessageToEveryone(HelperClass.ServerPointCommand(playerClass.points));
			if (starState == StarState.USING) starState = StarState.USED;
		}
	}
}
