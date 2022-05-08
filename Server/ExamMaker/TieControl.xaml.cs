using Server.ExamMaker.Components;
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

namespace Server.ExamMaker
{
	/// <summary>
	/// Interaction logic for TieControl.xaml
	/// </summary>
	public partial class TieControl : UserControl
	{
		public TieBreaker tieBreaker { get; set; }
		public TieControl(TieBreaker tieBreaker)
		{
			InitializeComponent();
			this.tieBreaker = tieBreaker;
			for (int i = 0; i < TieBreaker.QUES_CNT; i++)
			{
				QuestionControl questionControl = new QuestionControl(tieBreaker.questions[i]);
				questionControl.lblQ.Content = i + 1;
				stQuestions.Children.Add(questionControl);
			}
		}
	}
}
