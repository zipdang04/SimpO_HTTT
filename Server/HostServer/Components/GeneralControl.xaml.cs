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

using Server.Information;
using SimpleSockets.Messaging.Metadata;
using SimpleSockets.Server;

namespace Server.HostServer.Components
{
	/// <summary>
	/// Interaction logic for ScoreControl.xaml
	/// </summary>
	public partial class GeneralControl : UserControl
	{
		//public PlayerClass players { get; set; }
		SimpleSocketListener listener;
		List<TextBox> playersName;
		List<TextBox> playersPoint;
		PlayerClass players;
		public GeneralControl(SimpleSocketListener listener, PlayerClass players)
		{
			InitializeComponent();
			this.listener = listener;
			//this.players = players;
			playersName = new List<TextBox>();
			playersName.Add(txtName1); playersName.Add(txtName2);
			playersName.Add(txtName3); playersName.Add(txtName4);
			playersPoint = new List<TextBox>();
			playersPoint.Add(txtPoint1); playersPoint.Add(txtPoint2);
			playersPoint.Add(txtPoint3); playersPoint.Add(txtPoint4);
			this.players = players;
			DataContext = this.players;
		}

		private void btnChange_Click(object sender, RoutedEventArgs e)
		{
			try {
				IDictionary<int, IClientInfo> clients = listener.GetConnectedClients();
				int fail = 0;
				foreach (KeyValuePair<int, IClientInfo> client in clients) {
					try {
						listener.SendMessage(client.Value.Id, HelperClass.ServerNameCommand(players.names));
						listener.SendMessage(client.Value.Id, HelperClass.ServerPointCommand(players.points));
					}
					catch {
						fail += 1;
					}
				}
				MessageBox.Show(fail > 0 ? ("Có " + fail.ToString() + " máy lỗi") : "Thành công toàn bộ");
			}
			catch {
				MessageBox.Show("Lỗi");
			}
		}
	}
}
