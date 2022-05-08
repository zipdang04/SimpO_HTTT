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
using Microsoft.Win32;

namespace Server.HostServer
{
	/// <summary>
	/// Interaction logic for GeneralControl.xaml
	/// </summary>
	
	public partial class GeneralControl : UserControl
	{
		SimpleSocketTcpListener listener;
		PlayerClass playerInfo;
		PointsEditor pointsEditor;
		PlayerNetwork playerNetwork;

		public GeneralControl(SimpleSocketTcpListener listener, PlayerClass playerInfo, PlayerNetwork playerNetwork)
		{
			InitializeComponent();
			this.listener = listener;
			this.playerInfo = playerInfo;
			this.playerNetwork = playerNetwork;
			pointsEditor = new PointsEditor(listener, playerInfo);
			gridPoints.Children.Add(pointsEditor);
		}

		public void sendMessageToEveryone(string message)
		{
			foreach (KeyValuePair<int, IClientInfo> client in listener.GetConnectedClients())
			{
				listener.SendMessage(client.Value.Id, message);
			}
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

		public void Connect(int position)
		{
			Dispatcher.Invoke(() => { 
				switch (position) {
					case 0: btnKick1.IsEnabled = true; break;
					case 1: btnKick2.IsEnabled = true; break;
					case 2: btnKick3.IsEnabled = true; break;
					case 3: btnKick4.IsEnabled = true; break;
				}
			});
		}
		void Kick(int position)
		{
			if (playerNetwork.clients[position] != null)
				playerNetwork.disconnect(playerNetwork.clients[position]);
		}
		private void btnKick1_Click(object sender, RoutedEventArgs e) { Kick(0); btnKick1.IsEnabled = false; }
		private void btnKick2_Click(object sender, RoutedEventArgs e) { Kick(1); btnKick2.IsEnabled = false; }
		private void btnKick3_Click(object sender, RoutedEventArgs e) { Kick(2); btnKick3.IsEnabled = false; }
		private void btnKick4_Click(object sender, RoutedEventArgs e) { Kick(3); btnKick4.IsEnabled = false; }

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			stackMedia.Children.Add(new StackMedia(listener));
		}

		private void btnSendQues_Click(object sender, RoutedEventArgs e)
		{
			string source = @"Resources/Media", dest = @"Resources/Media.zip";
			if (File.Exists(dest)) File.Delete(dest);
			ZipFile.CreateFromDirectory(source, dest);
			byte[] goddamn = File.ReadAllBytes(dest);
			foreach (KeyValuePair<int, IClientInfo> client in listener.GetConnectedClients())
				listener.SendBytes(client.Value.Id, goddamn);
		}

		private void btnSendImg_Click(object sender, RoutedEventArgs e)
		{ 
		}

		private void btnLoadImg_Click(object sender, RoutedEventArgs e)	
		{
			string dirPath = @"./Resources/PlayerImage";
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if (openFileDialog.ShowDialog() == true)
			{
				try
				{
					HelperClass.ClearDirectory(new DirectoryInfo(dirPath));
					ZipFile.ExtractToDirectory(openFileDialog.FileName, dirPath);
				}
				catch
				{
					MessageBox.Show("Oh shit there's some problem :(", "err", MessageBoxButton.OK);
				}
			}
		}
	}
}
