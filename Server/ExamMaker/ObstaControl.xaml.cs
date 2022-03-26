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
	/// Interaction logic for ObstaControl.xaml
	/// </summary>
	public partial class ObstaControl : UserControl
	{
		ObstacleClass obsta;
		public ObstaControl(ObstacleClass obsta)
		{
			InitializeComponent();
			this.obsta = obsta;

			for (int i = 0; i < ObstacleClass.QUES_NO; i++) {
				QuestionControl tmp = new QuestionControl(obsta.questions[i]);
				tmp.lblQ.Content = i + 1;
				stackPanel.Children.Add(tmp);
			}

			DataContext = obsta;
		}
	}
}
