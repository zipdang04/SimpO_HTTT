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

namespace Client.PlayerClient.GamesControl
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
			if (done){
				txtAnswer.Text = "";
				txtAnswer.IsEnabled = false;
			}
		}

		public void ShowQuestion(int label, string question, string attach)
		{
			Dispatcher.Invoke(() => {
				lblTemp.Content = "Hàng ngang " + (label + 1).ToString();
				lblCC.Content = "Số Chữ cái: " + cntLetter[label].ToString();
				questionBox.DisplayQuestion(question, attach);
			});
		}

		public void StartTimer()
		{
			Dispatcher.Invoke(() => {
				txtAnswer.Text = "";
				txtAnswer.IsEnabled = true;
				txtAnswer.Focus();
			});
			timer.Start();
		}
		
		public void ResetGame(string imagePath, int[] cntLetter)
		{
			this.cntLetter = cntLetter;
			Dispatcher.Invoke(() => {
				btnBell.IsEnabled = true;
				txtAnswer.IsEnabled = false;
				lblAnswer.Content = "";

			});
			timer.Stop();
			this.imagePath = imagePath;
			setImage();
		}
		public void Keyword(int cnt)
		{
			Dispatcher.Invoke(() => { lblCNV.Content = string.Format("CNV có {0} ký tự", cnt); });
		}

		private void btnBell_Click(object sender, RoutedEventArgs e)
		{
			client.SendMessage("OLPA VCNV BELL");
			btnBell.IsEnabled = false;
		}

		private void txtAnswer_KeyDown(object sender, KeyEventArgs e)
		{
			if (txtAnswer.IsEnabled == true && e.Key == Key.Enter) {
				int time = timer.getTime();
				string answer = txtAnswer.Text.ToUpper();
				client.SendMessage(String.Format("OLPA VCNV ANSWER {0} {1}", time, HelperClass.MakeString(answer)));
				txtAnswer.Text = ""; // để tính lại
				lblAnswer.Content = string.Format("{0} ({1:0.00})", answer, time / 100.0);
			}
		}
	}
}
