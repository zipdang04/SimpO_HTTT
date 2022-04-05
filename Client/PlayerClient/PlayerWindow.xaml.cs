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
using System.Windows.Shapes;

using Server.Information;
using SimpleSockets;
using SimpleSockets.Client;
using Client.PlayerClient.GamesControl;
using Server.QuestionClass;
using System.IO;
using System.Text.Json;

namespace Client.PlayerClient
{
	/// <summary>
	/// Interaction logic for PlayerWindow.xaml
	/// </summary>
	public partial class PlayerWindow : Window
	{
		public static readonly string filePath = "Resources/OExam.json";

		SimpleSocketClient client;
		LogInWindow logInWindow;
		PlayerClass playersInfo;

		public StartPlayerControl startPlayerControl;
		public PointsControl pointsControl;
		public ObstaPlayerControl obstaPlayerControl;
		public AccelPlayerControl accelPlayerControl;
		public FinishPlayerControl finishPlayerControl;
		WholeExamClass? wholeExam;
		public PlayerWindow(LogInWindow logInWindow, SimpleSocketClient client)
		{
			InitializeComponent();
			this.logInWindow = logInWindow;

			try {
				byte[] jsonString = File.ReadAllBytes(filePath);
				var utf8Reader = new Utf8JsonReader(jsonString);
				wholeExam = JsonSerializer.Deserialize<WholeExamClass>(ref utf8Reader);
				if (wholeExam == null)
					wholeExam = new WholeExamClass();
			}
			catch {
				wholeExam = new WholeExamClass();
				MessageBox.Show("co van de, khong choi duoc");
				Close();
			}

			this.client = client;
			playersInfo = new PlayerClass();
			this.client.MessageReceived += ServerMessageReceived;

			pointsControl = new PointsControl(playersInfo);
			startPlayerControl = new StartPlayerControl(client);
			obstaPlayerControl = new ObstaPlayerControl(client);
			accelPlayerControl = new AccelPlayerControl(client);
			finishPlayerControl = new FinishPlayerControl(client);
			grid.Children.Add(pointsControl);
			grid.Children.Add(startPlayerControl);
			grid.Children.Add(obstaPlayerControl);
			grid.Children.Add(accelPlayerControl);
			grid.Children.Add(finishPlayerControl);
			pointsControl.Visibility = Visibility.Visible;
			startPlayerControl.Visibility = Visibility.Collapsed;
			obstaPlayerControl.Visibility = Visibility.Collapsed;
			accelPlayerControl.Visibility = Visibility.Collapsed;
			finishPlayerControl.Visibility = Visibility.Collapsed;
		}

		private void ServerMessageReceived(SimpleSocket a, string msg)
		{
			List<string> tokens = HelperClass.ParseToken(msg);
			int len = tokens.Count;
			
			switch (tokens[1]) {
				case "SCENE":
					Dispatcher.Invoke(() =>
					{
						pointsControl.Visibility = Visibility.Hidden;
						startPlayerControl.Visibility = Visibility.Collapsed;
						obstaPlayerControl.Visibility = Visibility.Collapsed;
						finishPlayerControl.Visibility = Visibility.Collapsed;
						switch (tokens[2]) {
							case "POINT":
								pointsControl.Visibility = Visibility.Visible;
								break;
							case "KD":
								startPlayerControl.Visibility = Visibility.Visible;
								break;
							case "VCNV":
								obstaPlayerControl.Visibility = Visibility.Visible;
								break;
							case "TT":
								accelPlayerControl.Visibility = Visibility.Visible;
								break;
							case "VD":
								finishPlayerControl.Visibility = Visibility.Visible;
								break;
						}
					});
					break;
				case "INFO":
					if (tokens[2] == "NAME")
						for (int player = 0; player < 4; player++)
							playersInfo.names[player] = tokens[player + 3];
					else if (tokens[2] == "POINT")
						for (int player = 0; player < 4; player++)
							playersInfo.points[player] = Convert.ToInt32(tokens[player + 3]);
					pointsControl.update();
					break;
				case "KD":
					switch (tokens[2]) {
						case "QUES":
							string question = tokens[3], attach = tokens[4];
							startPlayerControl.ShowQuestion(new OQuestion(question, "", attach));
							break;
					}
					break;
				case "VCNV":
					switch (tokens[2]) {
						case "START":
							obstaPlayerControl.ResetGame();
							break;
						case "SHOW":
							string label = tokens[3];
							string question = tokens[4];
							obstaPlayerControl.ShowQuestion(label, question);
							break;
						case "TIME":
							obstaPlayerControl.StartTimer();
							break;
					}
					break;
				case "TT":
					switch (tokens[2]) {
						case "LOAD":
							int turn = Convert.ToInt32(tokens[3]);
							string question = tokens[4];
							string attach = tokens[5];
							accelPlayerControl.ResetGame();
							accelPlayerControl.ShowQuestion(turn, question, attach, turn * 1000);
							break;
						case "PLAY":
							accelPlayerControl.StartTimer();
							break;
					}
					break;
				case "VD":
					switch (tokens[2]) {
						case "QUES":
							string question = tokens[3], attach = tokens[4];
							finishPlayerControl.ShowQuestion(new OQuestion(question, "", attach));
							break;
						case "LOCK":
							finishPlayerControl.LockButton();
							break;
						case "UNLOCK":
							finishPlayerControl.UnlockButton();
							break;
					}
					break;
			}
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			try { client.Close(); }
			catch { }
			logInWindow.Show();
		}
	}
}
