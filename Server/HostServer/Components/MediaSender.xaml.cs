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
using Server.Information;
using Server.QuestionClass;
using SimpleSockets.Messaging.Metadata;
using SimpleSockets;
using Server.HostServer.Components;
using Microsoft.Win32;
using System.IO;

namespace Server.HostServer.Components
{
	/// <summary>
	/// Interaction logic for MediaSender.xaml
	/// </summary>
	public partial class MediaSender : UserControl
	{
		SimpleSocketTcpListener listener;
		OpenFileDialog openFileDialog = new OpenFileDialog();

		public MediaSender(SimpleSocketTcpListener listener)
		{
			this.listener = listener;
			openFileDialog.Multiselect = false;
			openFileDialog.InitialDirectory = Directory.GetCurrentDirectory() + @"\Resources\Media";
			InitializeComponent();
		}

		public void sendMessageToEveryone(string message)
		{
			foreach (KeyValuePair<int, IClientInfo> client in listener.GetConnectedClients())
			{
				listener.SendMessage(client.Value.Id, message);
			}
		}

		private void btnDialog_Click(object sender, RoutedEventArgs e)
		{
			if (openFileDialog.ShowDialog() == true) {
				txtPath.Text = System.IO.Path.GetRelativePath(Directory.GetCurrentDirectory() + @"\Resources", openFileDialog.FileName);
				btnSend.IsEnabled = true;
			}
		}

		private void btnSend_Click(object sender, RoutedEventArgs e)
		{
			sendMessageToEveryone(String.Format("OLPA PLAY {0}", HelperClass.MakeString(txtPath.Text)));
		}

		private void btnClear_Click(object sender, RoutedEventArgs e)
		{
			txtPath.Text = "";
			btnSend.IsEnabled = false;
		}
	}
}
