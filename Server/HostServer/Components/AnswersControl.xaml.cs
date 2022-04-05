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
		List<Label> lblTimes;
		public List<CheckBox> checkBoxes;

		public class Data{
			public PlayerClass names { get; set; }
			public PlayerAnswers answers {get;set;}
			public Data(PlayerClass playerClass, PlayerAnswers playerAnswers){
				names = playerClass; answers = playerAnswers;
			}
		}
		public Data data { get; set; }

		public AnswersControl(PlayerClass playerClass)
		{
			InitializeComponent();
			
			data = new Data(playerClass, new PlayerAnswers());

			lblTimes = new List<Label>();
			lblTimes.Add(lblTime1);
			lblTimes.Add(lblTime2);
			lblTimes.Add(lblTime3);
			lblTimes.Add(lblTime4);

			checkBoxes = new List<CheckBox>();
			checkBoxes.Add(chkBox1);
			checkBoxes.Add(chkBox2);
			checkBoxes.Add(chkBox3);
			checkBoxes.Add(chkBox4);

			DataContext = data;
		}

		public void Reset() {
			data.answers.Reset();
			Dispatcher.Invoke(() =>{
				for (int i = 0; i < 4; i++)
				{
					checkBoxes[i].IsChecked = false;
					lblTimes[i].Content = "";
				}
			}); 
		}

		public void SomeoneAnswering(int player, string answer, int time)
		{
			data.answers.answers[player] = answer;
			data.answers.times[player] = time;
			Dispatcher.Invoke(() => {
				lblTimes[player].Content = time / 100.0;
			});
		}
	}
}
