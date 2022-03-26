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
		PlayerAnswers playerAnswers { get; set; }
		public AnswersControl(SimpleSocketListener listener)
		{
			InitializeComponent();
			this.listener = listener;
			playerAnswers = new PlayerAnswers();
		}
	}
}
