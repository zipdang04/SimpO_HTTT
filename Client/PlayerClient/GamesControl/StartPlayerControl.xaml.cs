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
using System.IO;

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
				txtQuestion.Text = question.question;
				try {
					string path = Directory.GetCurrentDirectory() + @"\Resources\Media\" + question.attach;
					image.Source = new BitmapImage(new Uri(path));
				}
				catch {
					image = new Image();
				}
			});
		}

	}
}
