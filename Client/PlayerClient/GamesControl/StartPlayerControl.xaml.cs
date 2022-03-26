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

using SimpleSockets.Client;
using Server.QuestionClass;
using Server.Information;

namespace Client.PlayerClient.GamesControl
{
	/// <summary>
	/// Interaction logic for StartPlayerControl.xaml
	/// </summary>
	public partial class StartPlayerControl : UserControl
	{
		SimpleSocketClient client;

		public StartPlayerControl(SimpleSocketClient client)
		{
			InitializeComponent();
			this.client = client;
		}

		public void ShowQuestion(OQuestion question)
		{
			Dispatcher.Invoke(() => { 
				lblQuestion.Content = question.question;
			});
		}

	}
}
