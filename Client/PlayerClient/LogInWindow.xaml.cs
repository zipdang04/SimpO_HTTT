using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using SimpleSockets.Client;
using SimpleSockets;

namespace Client.PlayerClient
{
	/// <summary>
	/// Interaction logic for LogInWindow.xaml
	/// </summary>
	public partial class LogInWindow : Window
	{
		MainWindow main;
		SimpleSocketClient client = new SimpleSocketTcpClient();

		int playerPosition;

		public LogInWindow(MainWindow main) {
			InitializeComponent();
			this.main = main;

			client.ConnectedToServer += ConnectedToServer;
			client.MessageReceived += ServerMessageReceived;
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			string serverIP;
			int serverPort;
			try {
				serverIP = txtIP.Text;
				serverPort = Convert.ToInt32(txtPort.Text);
				playerPosition = Convert.ToInt32(txtS.Text);

				IPAddress testIP = IPAddress.Parse(serverIP);
				if (playerPosition < 1 || playerPosition > 4)
					throw new Exception();
			} catch {
				MessageBox.Show("Đầu vào không hợp lệ!");
				return;
			}

			try {
				client.StartClient(serverIP, serverPort);
			} catch {
				MessageBox.Show("Có lỗi trong khi kết nối");
				return;
			}
		}

		private void ConnectedToServer(SimpleSocket a){
			client.SendMessage("OLPA S " + playerPosition.ToString());
		}

		private void ServerMessageReceived(SimpleSocket a, string msg)
		{
			if (msg == "OLPA CONFIRMED") {
				client.MessageReceived -= ServerMessageReceived;

				Dispatcher.Invoke(() => {
					PlayerWindow playerWindow = new PlayerWindow(client);
					playerWindow.Show(); Hide();
				});
			} else
			{
				MessageBox.Show("Kết nối tới máy chủ thất bại: có thể là bị trùng vị trí đứng");
			}
		}

		private void Window_Unloaded(object sender, RoutedEventArgs e)
		{
			main.Show();
		}
	}
}
