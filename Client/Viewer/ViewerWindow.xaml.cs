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

using System.IO;
using System.IO.Compression;
using Server.Information;
using SimpleSockets;
using SimpleSockets.Client;
using Server.QuestionClass;
using Client.Viewer.GamesControl;

namespace Client.Viewer
{
    /// <summary>
    /// Interaction logic for ViewerWindow.xaml
    /// </summary>
    public partial class ViewerWindow : Window
    {
        LogInWindow logInWindow;
        SimpleSocketClient client;
        PlayerClass playersInfo;

        StartViewerControl startViewerControl;
		PlainControl plainControl;
        public ViewerWindow(LogInWindow logInWindow, SimpleSocketClient client)
        {
            InitializeComponent();
            this.logInWindow = logInWindow; this.client = client;
            this.client.MessageReceived += ServerMessageReceived;
            this.client.BytesReceived += ServerBytesReceived;

			playersInfo = new PlayerClass();

			plainControl = new PlainControl();
            startViewerControl = new StartViewerControl(playersInfo);
			plainControl.Visibility = Visibility.Collapsed;
			grid.Children.Add(plainControl);
			grid.Children.Add(startViewerControl);
		}

		void ChangeScene(string s)
		{
			Dispatcher.Invoke(() => {
				plainControl.Visibility = Visibility.Collapsed;
				startViewerControl.Visibility = Visibility.Collapsed;
				//obstaViewerControl.Visibility = Visibility.Collapsed;
				//accelViewerControl.Visibility = Visibility.Collapsed;
				//finishViewerControl.Visibility = Visibility.Collapsed;
				switch (s) {
					case "PLAIN":
						plainControl.Visibility = Visibility.Visible;
						break;
					case "KD":
						startViewerControl.Visibility = Visibility.Visible;
						break;
					case "VCNV":
						//obstaViewerControl.Visibility = Visibility.Visible;
						break;
					case "TT":
						//accelViewerControl.Visibility = Visibility.Visible;
						break;
					case "VD":
						//finishViewerControl.Visibility = Visibility.Visible;
						break;
				}
			});
		}

		public static void ServerBytesReceived(SimpleSocketClient client, byte[] messageBytes)
		{
			string zip = @"Resources/Media.zip";
			if (File.Exists(zip)) File.Delete(zip);
			FileStream file = File.Create(zip);
			file.Write(messageBytes);
			file.Close();

			string dirPath = @"Resources/Media";
			HelperClass.ClearDirectory(new DirectoryInfo(dirPath));
			ZipFile.ExtractToDirectory(zip, dirPath);
			MessageBox.Show("Đã chuyển xong file!", "Chuyển xong file", MessageBoxButton.OK);
		}

		private void ServerMessageReceived(SimpleSocket a, string msg)
		{
			List<string> tokens = HelperClass.ParseToken(msg);
			int len = tokens.Count;

			switch (tokens[1]) {
				case "SCENE":
					ChangeScene(tokens[2]);
					break;
				case "INFO":
					if (tokens[2] == "NAME")
						for (int player = 0; player < 4; player++)
							playersInfo.names[player] = tokens[player + 3];
					else if (tokens[2] == "POINT")
						for (int player = 0; player < 4; player++)
							playersInfo.points[player] = Convert.ToInt32(tokens[player + 3]);
					break;
				case "KD":
					switch (tokens[2]) {
						case "INTRO":
							ChangeScene("PLAIN");
							plainControl.Play("KD_Intro.mp4");
							break;
						case "START": {
							int player = Convert.ToInt32(tokens[3]);
							startViewerControl.StartPlayer(player);
							break;
						}
						case "TIME": {
							//int player = Convert.ToInt32(tokens[3]);
							startViewerControl.RunPlayer();
							break;
						}
						case "OPENING":
							startViewerControl.Opening();
							break;
						case "CORRECT":
							startViewerControl.Correct();
							break;
						case "WRONG":
							startViewerControl.Wrong();
							break;
						case "BLANK":
							startViewerControl.Blank();
							break;
						case "DONE":
							startViewerControl.Done();
							break;
						case "QUES":
							string question = tokens[3], attach = tokens[4];
							startViewerControl.DisplayQuestion(question, attach);
							break;
					}
					break;
				case "VCNV":
					switch (tokens[2]) {
						case "INTRO":
							break;
						case "START":
							string attach = tokens[3];
							//obstaViewerControl.ResetGame(attach);
							break;
						case "SHOW":
							string label = (Convert.ToInt32(tokens[3]) + 1).ToString();
							string question = tokens[4];
							//obstaViewerControl.ShowQuestion(label, question, tokens[5]);
							break;
						case "TIME":
							//obstaViewerControl.StartTimer();
							break;
						case "OPEN":
							//obstaViewerControl.Erase(Convert.ToInt32(tokens[3]));
							break;
					}
					break;
				case "TT":
					switch (tokens[2]) {
						case "LOAD":
							int turn = Convert.ToInt32(tokens[3]);
							string question = tokens[4];
							string attach = tokens[5];
							//accelViewerControl.ResetGame();
							//accelViewerControl.ShowQuestion(turn, question, attach, turn * 1000);
							break;
						case "PLAY":
							//accelViewerControl.StartTimer();
							break;
					}
					break;
				case "VD":
					switch (tokens[2]) {
						case "QUES":
							string question = tokens[3], attach = tokens[4];
							//finishViewerControl.ShowQuestion(new OQuestion(question, "", attach));
							break;
						case "LOCK":
							//finishViewerControl.LockButton();
							break;
						case "UNLOCK":
							//finishViewerControl.UnlockButton();
							break;
						case "TIME":
							int timeLimit = Convert.ToInt32(tokens[3]);
							//finishViewerControl.StartTimer(timeLimit);
							break;
					}
					break;
			}
		}

		private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
            DragMove();
		}

		private void Window_Unloaded(object sender, RoutedEventArgs e)
		{
			client.Dispose();
			logInWindow.Show();
		}
	}
}
