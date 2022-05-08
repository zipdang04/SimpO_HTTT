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
using Server.QuestionClass;
using SimpleSockets.Messaging.Metadata;
using Server.Information;
using Server.HostServer.Components;

namespace Server.HostServer
{
	/// <summary>
	/// Interaction logic for TieController.xaml
	/// </summary>
	public partial class TieController : UserControl
	{
		SimpleSocketListener listener;
		TieBreaker tieClass;
		PlayerClass playerClass { get; set; }
		public PointsControl pointsControl;

		Simer timer;

		CheckBox[] chkBoxes = new CheckBox[4];

		public TieController(SimpleSocketListener listener, TieBreaker tieBreaker, PlayerClass playerClass)
		{
			InitializeComponent();
			this.tieClass = tieBreaker;
			this.listener = listener;
			this.playerClass = playerClass;

			timer = new Simer(TimeSpan.FromSeconds(15));
			for (int i = 0; i < 4; i++) { 

			}
		}
	}
}
