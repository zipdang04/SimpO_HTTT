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

namespace Server.HostServer
{
	/// <summary>
	/// Interaction logic for TieController.xaml
	/// </summary>
	public partial class TieController : UserControl
	{
		SimpleSocketListener listener;
		TieBreaker tieClass;
		PlayerClass playerClass { get; set; }
		public PointsControl pointsControl;

		Simer timer;

		CheckBox[] chkBoxes = new CheckBox[4];
		bool[] participating = new bool[4];

		const int NaN = -1;
		int curPlayer = NaN;

		public TieController(SimpleSocketListener listener, TieBreaker tieBreaker, PlayerClass playerClass)
		{
			InitializeComponent();
			this.tieClass = tieBreaker;
			this.listener = listener;
			this.playerClass = playerClass;
			
			pointsControl = new PointsControl(playerClass);
			stack.Children.Add(pointsControl);

			timer = new Simer(TimeSpan.FromSeconds(15));
			timer.Tick += timer_Tick;
			for (int i = 0; i < 4; i++) {
				chkBoxes[i] = new CheckBox();
				Grid.SetRow(chkBoxes[i], 0);
				Grid.SetColumn(chkBoxes[i], i + 1);
				gridPlayerList.Children.Add(chkBoxes[i]);
			}

			int[] haha = new int[TieBreaker.QUES_CNT];
			for (int i = 0; i < TieBreaker.QUES_CNT; i++) haha[i] = i + 1;
			listBox.ItemsSource = haha;
			listBox.SelectedIndex = 0;
			
			mainGrid.IsEnabled = false;
			btnStart.IsEnabled = false;
			Reset();
		}

		public void sendMessageToEveryone(string message)
		{
			foreach (KeyValuePair<int, IClientInfo> client in listener.GetConnectedClients())
				listener.SendMessage(client.Value.Id, message);
		}

		void timer_Tick(int time, bool done) {
			lblTime.Content = string.Format("{0:0.00}", (time) / 100.0);
			if (done) {
				btnCorrect.IsEnabled = false;
				btnWrong.IsEnabled = false;
				sendMessageToEveryone("OLPA CHP DONE");
			}
		}

		void Reset() {
			curPlayer = NaN;
			Dispatcher.Invoke(() => { 
				btnCorrect.IsEnabled = btnWrong.IsEnabled = false;
				pointsControl.Reset();
			});
		}

		private void btnConfirm_Click(object sender, RoutedEventArgs e)
		{
			for (int i = 0; i < 4; i++) if (chkBoxes[i].IsChecked == true) participating[i] = true; else participating[i] = false;
			mainGrid.IsEnabled = true;

			string command = "OLPA CHP PAR";
			for (int i = 0; i < 4; i++) {
				int num = participating[i] ? 1 : 0;
				command += " " + num.ToString();
			}
			sendMessageToEveryone(command);
		}

		private void btnShow_Click(object sender, RoutedEventArgs e) {
			btnStart.IsEnabled = true; Reset();
			int pos = listBox.SelectedIndex;
			listBox.SelectedIndex = (listBox.SelectedIndex + 1) % TieBreaker.QUES_CNT;
			OQuestion question = tieClass.questions[pos];
			questionBox.displayQA(question.question, question.answer);
			sendMessageToEveryone(String.Format("OLPA CHP SHOW {0}", HelperClass.ServerJoinQA(question)));
		}

		private void btnStart_Click(object sender, RoutedEventArgs e) {
			btnStart.IsEnabled = false;
			sendMessageToEveryone("OLPA CHP START");
			timer.Start(); 
		}

		public void SomeoneSucking(int player) {
			if (timer.IsEnabled == false || curPlayer != NaN) return;
			curPlayer = player;
			Dispatcher.Invoke(() => { 
				pointsControl.ChoosePlayer(player);
				btnCorrect.IsEnabled = btnWrong.IsEnabled = true;
			});
			timer.Pause();

			sendMessageToEveryone(String.Format("OLPA CHP SUCKED {0}", player));
		}

		private void btnCorrect_Click(object sender, RoutedEventArgs e) {
			timer.Stop();
			btnCorrect.IsEnabled = btnWrong.IsEnabled = false;
			sendMessageToEveryone("OLPA CHP CORRECT");
		}

		private void btnWrong_Click(object sender, RoutedEventArgs e) {
			timer.Resume();
			btnCorrect.IsEnabled = btnWrong.IsEnabled = false;
			sendMessageToEveryone("OLPA CHP RESUME");
		}

		private void btnCHP_Click(object sender, RoutedEventArgs e) {
			sendMessageToEveryone("OLPA SCENE CHP");
		}
	}
}
