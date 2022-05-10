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

		PlainControl plainControl;
        StartViewerControl startViewerControl;
		ObstaViewerControl obstaViewerControl;
		AnswerViewerControl answerViewerControl;
		AccelViewerControl accelViewerControl;
		PointsViewerControl pointsViewerControl;
		FinishViewerControl finishViewerControl;
		TieViewerControl tieViewerControl;
		public ViewerWindow(LogInWindow logInWindow, SimpleSocketClient client)
        {
            InitializeComponent();
            this.logInWindow = logInWindow; this.client = client;
            this.client.MessageReceived += ServerMessageReceived;
            this.client.BytesReceived += ServerBytesReceived;

			playersInfo = new PlayerClass();

			plainControl = new PlainControl();
            startViewerControl = new StartViewerControl(playersInfo);
			obstaViewerControl = new ObstaViewerControl();
			answerViewerControl = new AnswerViewerControl(playersInfo);
			accelViewerControl = new AccelViewerControl();
			pointsViewerControl = new PointsViewerControl();
			finishViewerControl = new FinishViewerControl(playersInfo);
			tieViewerControl = new TieViewerControl(playersInfo);
			grid.Children.Add(plainControl);
			grid.Children.Add(startViewerControl);
			grid.Children.Add(obstaViewerControl);
			grid.Children.Add(answerViewerControl);
			grid.Children.Add(accelViewerControl);
			grid.Children.Add(pointsViewerControl);
			grid.Children.Add(finishViewerControl);
			grid.Children.Add(tieViewerControl);
			ChangeScene("PLAIN");
		}

		void ChangeScene(string s)
		{
			Dispatcher.Invoke(() => {
				plainControl.Visibility = Visibility.Collapsed;
				startViewerControl.Visibility = Visibility.Collapsed;
				obstaViewerControl.Visibility = Visibility.Collapsed;
				answerViewerControl.Visibility = Visibility.Collapsed;
				accelViewerControl.Visibility = Visibility.Collapsed;
				pointsViewerControl.Visibility = Visibility.Collapsed;
				finishViewerControl.Visibility = Visibility.Collapsed;
				tieViewerControl.Visibility = Visibility.Collapsed;
				switch (s) {
					case "PLAIN":
						plainControl.Visibility = Visibility.Visible;
						break;
					case "ANSWER":
						answerViewerControl.Visibility = Visibility.Visible;
						break;
					case "POINTS":
						pointsViewerControl.Visibility = Visibility.Visible;
						break;
					case "KD":
						startViewerControl.Visibility = Visibility.Visible;
						break;
					case "VCNV":
						obstaViewerControl.Visibility = Visibility.Visible;
						break;
					case "TT":
						accelViewerControl.Visibility = Visibility.Visible;
						break;
					case "VD":
						finishViewerControl.Visibility = Visibility.Visible;
						break;
					case "CHP":
						tieViewerControl.Visibility = Visibility.Visible;
						break;	
				}
			});
		}

		public static void ServerBytesReceived(SimpleSocketClient client, byte[] messageBytes)
		{
			try
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
			catch
			{
				MessageBox.Show("chuyển file bất thành");
			}
		}

		private void ServerMessageReceived(SimpleSocket a, string msg)
		{
			List<string> tokens = HelperClass.ParseToken(msg);
			int len = tokens.Count;

			switch (tokens[1]) {
				case "PLAY":
					string media = tokens[2];
					ChangeScene("PLAIN");
					plainControl.Media(media);
					break;
				case "PLAIN":
					switch (tokens[2]) { 
						case "INTRO":
							ChangeScene("PLAIN");
							plainControl.Play("Intro.mp4");
							break;
						case "OPENING":
							ChangeScene("PLAIN");
							plainControl.Play("Opening.mpeg");
							break;
						case "PLINTRO":
							ChangeScene("PLAIN");
							plainControl.Play("PlayerIntro.mp3");
							break;
					}
					break;
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
				case "POINTS":
					ChangeScene("POINTS");
					pointsViewerControl.Start(playersInfo.names, playersInfo.points);
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
						case "TIME": 
							startViewerControl.RunPlayer();
							break;
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
							ChangeScene("PLAIN");
							plainControl.Play("VCNV_Intro.mp4");
							break;
						case "OPENING":
							obstaViewerControl.Opening();
							break;
						case "SCENE":
							if (tokens[3] == "ANSWER") ;
							else obstaViewerControl.ChangeScene(tokens[3]);
							break;
						case "START":
							string attach = tokens[3];
							int[] cntLetter = new int[4];
							for (int i = 0; i < 4; i++)
								cntLetter[i] = Convert.ToInt32(tokens[4 + i]);
							obstaViewerControl.ResetGame(attach, cntLetter);
							break;
						case "SHOW":
							string question = tokens[4];
							obstaViewerControl.ShowQuestion(Convert.ToInt32(tokens[3]), question, tokens[5]);
							break;
						case "TIME":
							obstaViewerControl.StartTimer();
							break;
						case "OPEN":
							obstaViewerControl.Open(Convert.ToInt32(tokens[3]));
							break;
						case "BELLING": 
							{ int player = Convert.ToInt32(tokens[3]); obstaViewerControl.PlayerBelling(player, playersInfo.names[player]); }
							break;
						case "REMOVESTACK": 
							{ int player = Convert.ToInt32(tokens[3]); obstaViewerControl.RemoveStack(player); }
							break;
						case "WINNER":
							obstaViewerControl.FoundWinner();
							break;
						case "ENAROW": {
							int index = Convert.ToInt32(tokens[3]);
							string answer = tokens[4];
							obstaViewerControl.OpenWord(index, answer);
						}
							break;
						case "DISROW": {
							int index = Convert.ToInt32(tokens[3]);
							obstaViewerControl.CloseWord(index);
						}
							break;
					}
					break;
				case "TT":
					switch (tokens[2]) {
						case "INTRO":
							ChangeScene("PLAIN");
							plainControl.Play("TT_Intro.mp4");
							break;
						case "OPENING":
							accelViewerControl.Opening();
							break;
						case "LOAD":
							int turn = Convert.ToInt32(tokens[3]);
							string question = tokens[4];
							string attach = tokens[5];
							accelViewerControl.Prepare(question, attach, turn);
							break;
						case "PLAY":
							accelViewerControl.Run();
							//accelViewerControl.StartTimer();
							break;
					}
					break;
				case "VD":
					switch (tokens[2]) {
						case "INTRO":
							ChangeScene("PLAIN");
							plainControl.Play("VD_Intro.mp4");
							break;
						case "OPENING":
							finishViewerControl.Opening();
							break;
						case "SCENE":
							finishViewerControl.ChangeScene(tokens[3]);
							break;
						case "CHOOSING": {
							int player = Convert.ToInt32(tokens[3]);
							finishViewerControl.Choosing(player);
						}
							break;
						case "CHOSEN":
							int[] difficulty = new int[3];
							for (int i = 0; i < 3; i++)
								difficulty[i] = Convert.ToInt32(tokens[i + 3]);
							finishViewerControl.Chosen(difficulty);
							break;
						case "TURN":
							int turn = Convert.ToInt32(tokens[3]);
							finishViewerControl.SetTurn(turn);
							break;
						case "QUES":
							string question = tokens[3], attach = tokens[4];
							finishViewerControl.ShowQuestion(question, attach);
							break;
						case "STAR":
							finishViewerControl.Star();
							break;
						case "UNLOCK":
							finishViewerControl.Start5s();
							break;
						case "SUCK": { int player = Convert.ToInt32(tokens[3]); finishViewerControl.SomeoneSucking(player); }
							break;
						case "PRAC":
							finishViewerControl.PracticeMode(tokens[3] == "MAIN");
							break;
						case "MEDIA":
							finishViewerControl.ShowMedia();
							break;
						case "TIME":
							finishViewerControl.Run();
							break;
						case "CORRECT":
							finishViewerControl.ResultMusic(true);
							break;
						case "LAST15":
							break;
						case "WRONG":
							finishViewerControl.ResultMusic(false);
							break;
						case "ENDING":
							finishViewerControl.Ending();
							break;
					}
					break;
				case "ANS":
					if (tokens[2] == "ANSWER") {
						string[] answers = new string[4]; int[] times = new int[4];
						for (int i = 0; i < 4; i++) {
							answers[i] = tokens[3 + i];
							times[i] = Convert.ToInt32(tokens[8 + i]);
						}
						ChangeScene("ANSWER");
						answerViewerControl.Run(answers, times);
						obstaViewerControl.SceneReset();
						//accelViewerControl.SceneReset();
					} else if (tokens[2] == "RES"){
						bool[] correct = new bool[4];
						for (int i = 0; i < 4; i++) { correct[i] = Convert.ToInt32(tokens[3 + i]) == 1; }
						ChangeScene("ANSWER");
						answerViewerControl.Conclusion(correct);
					} else if (tokens[2] == "SCENE")
						answerViewerControl.Change(tokens[3]);
					break;
				case "CHP":
					switch (tokens[2]) {
						case "PAR":
							bool[] state = new bool[4];
							for (int i = 0; i < 4; i++) state[i] = (Convert.ToInt32(tokens[3 + i]) == 1) ? true : false;
							tieViewerControl.Register(state);
							break;
						case "SHOW":
							string question = tokens[3], attach = tokens[4];
							tieViewerControl.ShowQuestion(question, attach);
							break;
						case "START":
							tieViewerControl.Start();
							break;
						case "SUCKED":
							int player = Convert.ToInt32(tokens[3]);
							tieViewerControl.SomeoneSucking(player);
							break;
						case "CORRECT":
							tieViewerControl.Correct();
							break;
						case "RESUME":
							tieViewerControl.Resume();
							break;
					};
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
