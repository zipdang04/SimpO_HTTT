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
using System.Windows.Threading;
using Server.HostServer.Components;

namespace Client.MCClient.GamesControl
{
	/// <summary>
	/// Interaction logic for ObstaPlayerControl.xaml
	/// </summary>
	public partial class ObstaPlayerControl : UserControl
	{
		SimpleSocketClient client;

		string imagePath;

		Simer timer;
		const int timeLimit = 1500;
		int[] cntLetter = new int[5];

		public ObstaPlayerControl(SimpleSocketClient client)
		{
			InitializeComponent();
			this.client = client;
			timer = new Simer(timeLimit);
			timer.Tick += timer_Tick;
			setImage();
		}

		void setImage()
		{
			imageControl.setImage(imagePath);
		}
		public void Erase(int idx) {
			imageControl.erase(idx);
		}

		void timer_Tick(int time, bool done)
		{
			lblTime.Content = string.Format("{0:0.00}", time / 100.0);
		}

		public void ShowQuestion(int label, OQuestion question)
		{
			Dispatcher.Invoke(() => {
				lblTemp.Content = "Hàng ngang " + (label + 1).ToString();
				lblCC.Content = "Số Chữ cái: " + cntLetter[label].ToString();
				questionBox.DisplayQuestion(question);
			});
		}

		public void StartTimer()
		{
			timer.Start();
		}
		
		public void ResetGame(string imagePath, int[] cntLetter)
		{
			this.cntLetter = cntLetter;
			timer.Stop();
			this.imagePath = imagePath;
			setImage();
		}
		public void Keyword(int cnt)
		{
			Dispatcher.Invoke(() => { lblCNV.Content = string.Format("CNV có {0} ký tự", cnt); });
		}
	}
}
