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

using Server.QuestionClass;
using Server.ExamMaker.Components;
namespace Server.ExamMaker
{
	/// <summary>
	/// Interaction logic for StartControl.xaml
	/// </summary>
	public partial class StartControl : UserControl
	{
		public StartClass start { get; set; }
		List<StackPanel> stackQues;
		public StartControl(StartClass start)
		{
			InitializeComponent();
			this.start = start;

			stackQues = new List<StackPanel>();
			stackQues.Add(st1);
			stackQues.Add(st2);
			stackQues.Add(st3);

			for (int i = 0; i < 3; i++)
				for (int j = 0; j < StartClass.QUES_CNT; j++)
				{
					QuestionControl tmp = new QuestionControl(start.questions[i][j]);
					tmp.lblQ.Content = (j + 1).ToString();
					stackQues[i].Children.Add(tmp);
				}
		}
	}
}
