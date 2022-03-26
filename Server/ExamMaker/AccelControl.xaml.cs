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
	/// Interaction logic for AccelControl.xaml
	/// </summary>
	public partial class AccelControl : UserControl
	{
		public AccelClass accel { get; set; }
		List<StackPanel> stackQues;
		public AccelControl(AccelClass accel)
		{
			InitializeComponent();
			this.accel = accel;

			stackQues = new List<StackPanel>();
			stackQues.Add(st1);
			stackQues.Add(st2);
			stackQues.Add(st3);
			stackQues.Add(st4);

			for (int i = 0; i < 4; i++) 
				stackQues[i].Children.Add(new AccelImageControl(accel.accel[i]));
		}
	}
}
