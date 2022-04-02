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
	/// Interaction logic for AnswersControl.xaml
	/// </summary>
	public partial class AnswersControl : UserControl
	{
		SimpleSocketListener listener;
		PlayerClass playerClass;
		List<Label> lblAnswers;
		List<CheckBox> checkBoxes;
		public AnswersControl(SimpleSocketListener listener, PlayerClass playerClass)
		{
			InitializeComponent();
			this.listener = listener;
			this.playerClass = playerClass;

			lblAnswers = new List<Label>();
			lblAnswers.Add(lblAnswer1);
			lblAnswers.Add(lblAnswer2);
			lblAnswers.Add(lblAnswer3);
			lblAnswers.Add(lblAnswer4);

			checkBoxes = new List<CheckBox>();
			checkBoxes.Add(chkBox1);
			checkBoxes.Add(chkBox2);
			checkBoxes.Add(chkBox3);
			checkBoxes.Add(chkBox4);

			DataContext = playerClass;
		}

		public void sendMessageToEveryone(string message)
		{
			foreach (KeyValuePair<int, IClientInfo> client in listener.GetConnectedClients()) {
				listener.SendMessage(client.Value.Id, message);
			}
		}

		void Reset() {
			Dispatcher.Invoke(() =>{
				for (int i = 0; i < 4; i++){
					lblAnswers[i].Content = "";
					checkBoxes[i].IsChecked = false;
				}
			}); 
		}

		void SendAnswer() {
			string command = "OLPA VD ANSWER {0} {1} {2} {3}";
			for (int i = 0; i < 4; i++){
				object str = lblAnswers[i].Content;
				command = string.Format(command, HelperClass.MakeString((str == null) ? "" : str.ToString()));
			}
		}

		void SendVerification() {
			//
		}
	}
}
