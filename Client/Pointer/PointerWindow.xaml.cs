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

namespace Client.Pointer
{
	/// <summary>
	/// Interaction logic for PointerWindow.xaml
	/// </summary>
	public partial class PointerWindow : Window
	{
		LogInWindow logInWindow;
		SimpleSocketClient client;
		PlayerClass playersInfo;
		public PointerWindow(LogInWindow window, SimpleSocketClient client)
		{
			InitializeComponent();
			logInWindow = window;
			this.client = client;
			playersInfo = new PlayerClass();
			client.MessageReceived += ServerMessageReceived;
			DataContext = playersInfo;
		}

		private void ServerMessageReceived(SimpleSocket a, string msg)
		{
			List<string> tokens = HelperClass.ParseToken(msg);
			int len = tokens.Count;

			switch (tokens[1])
			{
				case "INFO":
					if (tokens[2] == "NAME")
						for (int player = 0; player < 4; player++)
							playersInfo.names[player] = tokens[player + 3];
					else if (tokens[2] == "POINT")
						for (int player = 0; player < 4; player++)
							playersInfo.points[player] = Convert.ToInt32(tokens[player + 3]);
					break;
			}
		}

		private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			DragMove();
		}
	}
}
