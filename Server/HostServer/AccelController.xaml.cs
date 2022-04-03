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
using System.Windows.Threading;

namespace Server.HostServer
{
	/// <summary>
	/// Interaction logic for AccelController.xaml
	/// </summary>
	public partial class AccelController : UserControl
	{
		SimpleSocketListener listener;
		PlayerClass playerClass;
		AccelClass accelClass;
		PlayerNetwork playerNetwork;
		AnswersControl answersControl;
		PointsControl pointsControl;

		DispatcherTimer timer;
		int timeRemaining = 0;

		public AccelController(SimpleSocketListener listener, AccelClass accelClass, PlayerClass playerClass, PlayerNetwork playerNetwork)
		{
			InitializeComponent();
			this.listener = listener; this.playerClass = playerClass;
			this.accelClass = accelClass; this.playerNetwork = playerNetwork;

			answersControl = new AnswersControl(listener, playerClass);
			stackPanel.Children.Add(answersControl);
			pointsControl = new PointsControl(playerClass);
			stackPanel.Children.Add(pointsControl);
			answersControl.IsEnabled = false;
			pointsControl.IsEnabled = false;

			btnPlay.IsEnabled = false;
		}

		void Prepare(int turn)
		{
			btnTT1.IsEnabled = false;
			btnTT2.IsEnabled = false;
			btnTT3.IsEnabled = false;
			btnTT4.IsEnabled = false;
			timeRemaining = (turn + 1) * 1000;
			
			OQuestion question = accelClass.accelQuestions[turn].question;
			questionBox.displayQA(question.question, question.answer);

		}

		private void btnTT1_Click(object sender, RoutedEventArgs e)
		{
			Prepare(1);
		}
	}
}
