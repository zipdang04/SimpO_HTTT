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
	/// Interaction logic for AccelController.xaml
	/// </summary>
	public partial class AccelController : UserControl
	{
		SimpleSocketListener listener;
		PlayerClass playerClass;
		AccelClass accelClass;
		PlayerNetwork playerNetwork;
		AnswersControl answersControl;
		PointsControl pointsControl;

		int questionTurn;
		DispatcherTimer timer;
		int timeLimit;
		DateTime timeBegin;

		public AccelController(SimpleSocketListener listener, AccelClass accelClass, PlayerClass playerClass, PlayerNetwork playerNetwork)
		{
			InitializeComponent();
			this.listener = listener; this.playerClass = playerClass;
			this.accelClass = accelClass; this.playerNetwork = playerNetwork;

			timer = new DispatcherTimer(); 
			timer.Interval = TimeSpan.FromMilliseconds(2);
			timer.Tick += timer_Tick;

			answersControl = new AnswersControl(playerClass);
			gridAnswer.Children.Add(answersControl);
			pointsControl = new PointsControl(playerClass);
			stackPanel.Children.Add(pointsControl);
			answersControl.IsEnabled = false;
			pointsControl.IsEnabled = false;

			btnPlay.IsEnabled = false;
		}

		public void sendMessageToEveryone(string message)
		{
			foreach (KeyValuePair<int, IClientInfo> client in listener.GetConnectedClients()) {
				listener.SendMessage(client.Value.Id, message);
			}
		}
		int getTime()
		{
			TimeSpan span = DateTime.Now - timeBegin;
			return (span.Seconds * 1000 + span.Milliseconds) / 10;
		}

		void timer_Tick(object? sender, EventArgs e)
		{
			int time = getTime();
			Dispatcher.Invoke(() => { lblTime.Content = time / 100.0; });
			
			if (time >= timeLimit){
				timer.Stop();
				answersControl.IsEnabled = true;
			}
		}

		void Prepare(int turn)
		{
			btnPlay.IsEnabled = true;
			timeLimit = (turn + 1) * 1000 + 200;
			
			OQuestion question = accelClass.accelQuestions[turn].question;
			questionBox.displayQA(question.question, question.answer);
			answersControl.Reset(); answersControl.IsEnabled = false;

			sendMessageToEveryone(string.Format("OLPA TT LOAD {0} {1}", turn + 1, HelperClass.ServerJoinQA(question)));
		}

		private void btnTT1_Click(object sender, RoutedEventArgs e)
		{
			Prepare(0);
		}

		public void PlayerAnswering(int player, string answer, int time)
		{
			answersControl.SomeoneAnswering(player, answer, time);
		}

		private void btnPlay_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone("OLPA TT PLAY");

			timer.Start(); timeBegin = DateTime.Now;
			btnPlay.IsEnabled = false;
		}

		private void btnShowAnswer_Click(object sender, RoutedEventArgs e)
		{
			string command = "OLPA TT ANSWER {0} {0} {0} {0} TIME {0} {0} {0} {0}";
			PlayerAnswers answers = answersControl.data.answers;
			for (int i = 0; i < 4; i++)
				command = string.Format(command, HelperClass.MakeString(answers.answers[i]));
			for (int i = 0; i < 4; i++)
				command = string.Format(command, answers.times[i]);
			sendMessageToEveryone(command);
			sendMessageToEveryone(HelperClass.ServerPointCommand(playerClass.points));
		}

		private void btnConfirm_Click(object sender, RoutedEventArgs e)
		{
			string command = "OLPA TT RES {0} {0} {0} {0}";
			for (int i = 0; i < 4; i++)
				command = string.Format(command, answersControl.checkBoxes[i].IsChecked);
			sendMessageToEveryone(command);

			PlayerAnswers playerAnswers = answersControl.data.answers;
			bool[] visited = new bool[4] { false, false, false, false };
			for (int pts = 40; pts > 0; pts -= 10) {
				int time = 5000;
				List<int> playerDeserved = new List<int>();
				for (int i = 0; i < 4; i++)
					if (answersControl.checkBoxes[i].IsChecked == true) {
						int curTime = answersControl.data.answers.times[i];
						if (curTime < time) playerDeserved = new List<int> { i };
						else if (curTime == time) playerDeserved.Add(i);
					}
				if (playerDeserved.Count == 0) break;
				foreach (int player in playerDeserved){
					visited[player] = true;
					playerClass.points[player] += pts;
				}
			}
			sendMessageToEveryone(HelperClass.ServerPointCommand(playerClass.points));
		}

		private void btnTT2_Click(object sender, RoutedEventArgs e)
		{
			Prepare(1);
		}

		private void btnTT3_Click(object sender, RoutedEventArgs e)
		{
			Prepare(2);
		}

		private void btnTT4_Click(object sender, RoutedEventArgs e)
		{
			Prepare(3);
		}
	}
}
