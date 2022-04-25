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
using Client.MCClient.GamesControl;
using Client.PlayerClient.GamesControl.Components;
using Server.QuestionClass;
using System.IO;
using System.IO.Compression;


namespace Client.MCClient
{
	/// <summary>
	/// Interaction logic for PlayerWindow.xaml
	/// </summary>
	public partial class PlayerWindow : Window
	{
		//public static readonly string filePath = "Resources/OExam.json";

		SimpleSocketClient client;
		LogInWindow logInWindow;
		PlayerClass playersInfo;

		public StartPlayerControl startPlayerControl;
		public PointsControl pointsControl;
		public ObstaPlayerControl obstaPlayerControl;
		public AccelPlayerControl accelPlayerControl;
		public FinishPlayerControl finishPlayerControl;

		public PlayerWindow(LogInWindow logInWindow, SimpleSocketClient client)
		{
			InitializeComponent();
			this.logInWindow = logInWindow;

			this.client = client;
			playersInfo = new PlayerClass();
			this.client.MessageReceived += ServerMessageReceived;
			this.client.BytesReceived += ServerBytesReceived;
			this.client.DisconnectedFromServer += Client_DisconnectedFromServer;

			pointsControl = new PointsControl(playersInfo);
			startPlayerControl = new StartPlayerControl(client);
			obstaPlayerControl = new ObstaPlayerControl(client);
			accelPlayerControl = new AccelPlayerControl(client);
			finishPlayerControl = new FinishPlayerControl(client);
			gridPoint.Children.Add(pointsControl);
			grid.Children.Add(startPlayerControl);
			grid.Children.Add(obstaPlayerControl);
			grid.Children.Add(accelPlayerControl);
			grid.Children.Add(finishPlayerControl);
			pointsControl.Visibility = Visibility.Visible;
			startPlayerControl.Visibility = Visibility.Collapsed;
			obstaPlayerControl.Visibility = Visibility.Collapsed;
			accelPlayerControl.Visibility = Visibility.Collapsed;
			finishPlayerControl.Visibility = Visibility.Collapsed;

			pointsControl.Visibility = Visibility.Visible;
		}

		private void Client_DisconnectedFromServer(SimpleSocketClient client)
		{
			if (MessageBox.Show("tạch", "", MessageBoxButton.OK) == MessageBoxResult.OK) {
				try {
					client.Close();
					logInWindow.Show();
				}
				catch { }
			} else {
				try {
					client.Close();
					logInWindow.Show();
				}
				catch { }
			}
			
		}

		public void ServerBytesReceived(SimpleSocketClient client, byte[] messageBytes) {
			Dispatcher.Invoke(() => {
				try {
					string zip = @"Resources/Media.zip";
					if (File.Exists(zip)) File.Delete(zip);
					FileStream file = File.Create(zip);
					file.Write(messageBytes);
					file.Close();

					string dirPath = @"Resources/Media";
					HelperClass.ClearDirectory(new DirectoryInfo(dirPath));
					ZipFile.ExtractToDirectory(zip, dirPath);
					lblStatus.Content = "Chuyển file thành công";
				} catch {
					lblStatus.Content = "Chuyển file tạch, vui lòng liên hệ ban tổ chức trận đấu để có phương án xử lý (như copy file về máy thi đấu)";
				}
			});
		}

		private void ServerMessageReceived(SimpleSocket a, string msg)
		{
			List<string> tokens = HelperClass.ParseToken(msg);
			int len = tokens.Count;
			
			switch (tokens[1]) {
				case "SCENE":
					Dispatcher.Invoke(() =>
					{
						startPlayerControl.Visibility = Visibility.Collapsed;
						obstaPlayerControl.Visibility = Visibility.Collapsed;
						accelPlayerControl.Visibility = Visibility.Collapsed;
						finishPlayerControl.Visibility = Visibility.Collapsed;
						switch (tokens[2]) {
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
						case "TIME":
							startPlayerControl.StartTimer();
							break;
						case "QUES":
							string question = tokens[3], attach = tokens[4], answer = tokens[5];
							startPlayerControl.ShowQuestion(new OQuestion(question, answer, attach));
							break;
					}
					break;
				case "VCNV":
					switch (tokens[2]) {
						case "START":
							string attach = tokens[3];
							int[] cntLetter = new int[5];
							for (int i = 0; i < 5; i++) cntLetter[i] = Convert.ToInt32(tokens[4 + i]);
							obstaPlayerControl.ResetGame(attach, cntLetter);
							break;
						case "KEY":
							int lengg = Convert.ToInt32(tokens[3]);
							obstaPlayerControl.Keyword(lengg);
							break;
						case "SHOW":
							int label = Convert.ToInt32(tokens[3]);
							string question = tokens[4];
							string answer = tokens[6];
							obstaPlayerControl.ShowQuestion(label, new OQuestion(question, answer, tokens[5]));
							break;
						case "TIME":
							obstaPlayerControl.StartTimer();
							break;
						case "OPEN":
							obstaPlayerControl.Erase(Convert.ToInt32(tokens[3]));
							break;
					}
					break;
				case "TT":
					switch (tokens[2]) {
						case "LOAD":
							int turn = Convert.ToInt32(tokens[3]);
							string question = tokens[4];
							string attach = tokens[5];
							string answer = tokens[6];
							accelPlayerControl.ResetGame();
							accelPlayerControl.ShowQuestion(turn, question, attach, answer, turn * 1000);
							break;
						case "PLAY":
							accelPlayerControl.StartTimer();
							break;
					}
					break;
				case "VD":
					switch (tokens[2]) {
						case "QUES":
							string question = tokens[3], attach = tokens[4], answer = tokens[5];
							finishPlayerControl.ShowQuestion(new OQuestion(question, answer, attach));
							break;
						case "TIME":
							int timeLimit = Convert.ToInt32(tokens[3]);
							finishPlayerControl.StartTimer(timeLimit);
							break;
					}
					break;
			}
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			try { 
				client.Close(); 
				logInWindow.Show();
			}
			catch { }
		}
	}
}
