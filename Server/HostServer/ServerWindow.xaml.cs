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
using System.Text.Json;

using SimpleSockets.Server;
using Server.Information;
using Server.QuestionClass;
using SimpleSockets.Messaging.Metadata;
using SimpleSockets;
using Server.HostServer.Components;

namespace Server.HostServer
{
	/// <summary>
	/// Interaction logic for ServerWindow.xaml
	/// </summary>
	public partial class ServerWindow : Window
	{
		public static readonly string filePath = "Resources/OExam.json";

		MainWindow main;
		SimpleSocketTcpListener listener;
		PlayerClass playerInfo;
		PlayerNetwork playerNetwork;
		WholeExamClass? wholeExam;

		GeneralControl generalControl;
		StartController startController;
		ObstaController obstaController;
		AccelController accelController;
		FinishController finishController;
		
		public void updatePoint()
		{
			//startController.pointsControl.update();
		}

		public ServerWindow(MainWindow main)
		{
			InitializeComponent();
			this.main = main;
			listener = new SimpleSocketTcpListener();
			playerNetwork = new PlayerNetwork();
			playerInfo = new PlayerClass();
			listener.MessageReceived += MessageReceived;
			listener.ClientDisconnected += ClientDisconnected;

			try {
				byte[] jsonString = File.ReadAllBytes(filePath);
				var utf8Reader = new Utf8JsonReader(jsonString);
				wholeExam = JsonSerializer.Deserialize<WholeExamClass>(ref utf8Reader);
				if (wholeExam == null)
					wholeExam = new WholeExamClass();
			}
			catch {
				MessageBox.Show("co van de, khong choi duoc");
				Close();
			}

			generalControl = new GeneralControl(listener, playerInfo);
			gridGeneral.Children.Add(generalControl);
			startController = new StartController(listener, wholeExam.startQuestions, playerInfo, playerNetwork);
			gridStart.Children.Add(startController);
			obstaController = new ObstaController(listener, wholeExam.obstacle, playerInfo, playerNetwork);
			gridObsta.Children.Add(obstaController);
			accelController = new AccelController(listener, wholeExam.acceleration, playerInfo, playerNetwork);
			gridAccel.Children.Add(accelController);
			finishController = new FinishController(listener, wholeExam.finish, playerInfo, playerNetwork);
			gridFinish.Children.Add(finishController);
		}

		public void sendMessageToEveryone(string message)
		{
			foreach (KeyValuePair<int, IClientInfo> client in listener.GetConnectedClients()) {
				listener.SendMessage(client.Value.Id, message);
			}
		}

		private void btnStart_Click(object sender, RoutedEventArgs e)
		{
			listener.StartListening(Convert.ToInt32(txtPort.Text));
			lblIP.Content = "IP: cái này kiểu gì cũng ra" + listener.Ip;
			txtPort.IsEnabled = false;
		}

		private void Window_Unloaded(object sender, RoutedEventArgs e)
		{
			try {listener.Dispose();} catch { }
			Close();
			main.Show();
		}

		private void ClientDisconnected(IClientInfo client, DisconnectReason reason)
		{
			playerNetwork.disconnect(client);
		}

		private void MessageReceived(IClientInfo client, string msg)
		{
			int id = client.Id;
			int player = -1;
			for (int i = 0; i < 4; i++)
				if (playerNetwork.clients[i] != null && playerNetwork.clients[i].Id == id)
					player = i;

			List<string> tokens = HelperClass.ParseToken(msg);
			switch (tokens[1]) {
				case "S":
					int posi = Convert.ToInt32(tokens[2]) - 1;
					if (playerNetwork.connect(posi, client))
						listener.SendMessage(id, "OLPA CONFIRMED");
					else
						listener.SendMessage(id, "OLPA FAILED");
					break;
				case "VCNV":
					switch (tokens[2]) {
						case "BELL":
							obstaController.SomeoneBelling(player);
							break;
						case "ANSWER":
							int time = Convert.ToInt32(tokens[3]);
							string answer = tokens[4];
							obstaController.PlayerAnswering(player, answer, time);
							break;
						case "":
							break;
					}
					break;
				case "TT":
					switch (tokens[2]) {
						case "ANSWER":
							int time = Convert.ToInt32(tokens[3]);
							string answer = "";
							if (tokens.Count == 5) answer = tokens[4];
							accelController.PlayerAnswering(player, answer, time);
						break;
					}
					break;
				case "VD":
					if (tokens[2] == "BELL") {
						finishController.SomeoneSucking(player);
					}
					break;
			}
		}

		private void btnSend_Click(object sender, RoutedEventArgs e)
		{
			string source = @"Resources/Media", dest = @"Resources/Media.zip";
			if (File.Exists(dest)) File.Delete(dest);
			ZipFile.CreateFromDirectory(source, dest);
			byte[] goddamn = File.ReadAllBytes(dest);
			foreach (KeyValuePair<int, IClientInfo> client in listener.GetConnectedClients())
				listener.SendBytes(client.Value.Id, goddamn);
		}

		private void btnIntro_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone("OLPA PLAIN INTRO");
		}

		private void btnOpening_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone("OLPA PLAIN OPENING");
		}

		private void btnPlayerIntro_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone("OLPA PLAIN PLINTRO");
		}

		private void btnPlain_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone("OLPA SCENE PLAIN");
		}

		private void btnPoints_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone("OLPA POINTS");
		}
	}
}
