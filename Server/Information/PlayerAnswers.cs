using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Information
{
	public class PlayerAnswers
	{
		public ObservableCollection<string> answers { get; set; }
		public ObservableCollection<int> times { get; set; } // milliseconds
		public PlayerAnswers()
		{
			answers = new ObservableCollection<string> { "", "", "", "" };
			times = new ObservableCollection<int> { 0, 0, 0, 0 };
		}
		public void Reset()
		{
			for (int i = 0; i < 4; i++) {
				answers[i] = ""; times[i] = 0;
			}
		}
	}
}
