﻿using System;
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
			listener.Dispose();
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

			List<string> tokens = HelperClass.ParseToken(msg);
			switch (tokens[1]) {
				case "S":
					int posi = Convert.ToInt32(tokens[2]) - 1;
					if (playerNetwork.connect(posi, client))
						listener.SendMessage(id, "OLPA CONFIRMED");
					else
						listener.SendMessage(id, "OLPA FAILED");
					break;
				case "KD":
					break;
			}
		}

		private void tabGeneral_GotFocus(object sender, RoutedEventArgs e){sendMessageToEveryone("OLPA SCENE POINT");}

		private void tabStart_GotFocus(object sender, RoutedEventArgs e){ sendMessageToEveryone("OLPA SCENE KD"); }
		private void tabObsta_GotFocus(object sender, RoutedEventArgs e){sendMessageToEveryone("OLPA SCENE VCNV");}
		private void tabAccel_GotFocus(object sender, RoutedEventArgs e){sendMessageToEveryone("OLPA SCENE TT");}
		private void tabFinish_GotFocus(object sender, RoutedEventArgs e){sendMessageToEveryone("OLPA SCENE VD");}
	}
}