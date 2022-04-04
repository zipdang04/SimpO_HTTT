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

			timer = new DispatcherTimer(); timer.Interval = TimeSpan.FromMilliseconds(10);

			answersControl = new AnswersControl(listener, playerClass, "TT");
			stackPanel.Children.Add(answersControl);
			pointsControl = new PointsControl(playerClass);
			stackPanel.Children.Add(pointsControl);
			answersControl.IsEnabled = false;
			pointsControl.IsEnabled = false;

			btnPlay.IsEnabled = false;
		}

		void timer_Tick(object? sender, EventArgs e)
		{
			timeRemaining--;
			lblTime.Content = timeRemaining.ToString();
			if (timeRemaining == 0){
				timer.Stop();
				answersControl.IsEnabled = true;
			}
		}

		void Prepare(int turn)
		{
			btnPlay.IsEnabled = true;
			timeRemaining = (turn + 1) * 1000;
			
			OQuestion question = accelClass.accelQuestions[turn].question;
			questionBox.displayQA(question.question, question.answer);
			answersControl.Reset();
		}

		private void btnTT1_Click(object sender, RoutedEventArgs e)
		{
			Prepare(1);
		}
	}
}
