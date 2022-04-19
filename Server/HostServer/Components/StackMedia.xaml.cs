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

namespace Server.HostServer.Components
{
	/// <summary>
	/// Interaction logic for StackMedia.xaml
	/// </summary>
	public partial class StackMedia : UserControl
	{
		SimpleSocketTcpListener listener;
		public StackMedia(SimpleSocketTcpListener listener)
		{
			InitializeComponent();
			this.listener = listener;
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			stack.Children.Add(new MediaSender(listener));
		}
	}
}
