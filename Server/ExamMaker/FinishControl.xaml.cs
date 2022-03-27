using Server.QuestionClass;
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

using Server.ExamMaker.Components;
namespace Server.ExamMaker
{
	/// <summary>
	/// Interaction logic for FinishControl.xaml
	/// </summary>
	public partial class FinishControl : UserControl
	{
		public FinishClass finish;
		List<StackPanel> stackQues;
		public FinishControl(FinishClass finish)
		{
			InitializeComponent();
			this.finish = finish;

			stackQues = new List<StackPanel>();
			stackQues.Add(st1);
			stackQues.Add(st2);
			stackQues.Add(st3);

			for (int pts_type = 0; pts_type < 3; pts_type++)
				for (int player = 0; player < 4; player++)
					for (int turn = 0; turn < 3; turn++) {
						QuestionControl ques = new QuestionControl(finish.questions[player][pts_type][turn]);
						string label = "TS" + (player + 1).ToString() + "-" + (turn + 1).ToString();
						ques.lblQ.Content = label;
						stackQues[pts_type].Children.Add(ques);
					}
		}
	}
}
