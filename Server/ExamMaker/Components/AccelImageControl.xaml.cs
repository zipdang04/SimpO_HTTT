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
namespace Server.ExamMaker.Components
{
	/// <summary>
	/// Interaction logic for AccelImageControl.xaml
	/// </summary>
	public partial class AccelImageControl : UserControl
	{
		public AccelQuestion accel{get; set;}
		QuestionControl questionControl;
		public AccelImageControl(AccelQuestion accel)
		{
			InitializeComponent();
			this.accel = accel;
			questionControl = new QuestionControl(accel.question);
			tmp.Children.Add(questionControl);
			for (int i = 0; i < AccelQuestion.MAX_IMG; i++) {
				ImageControl imageControl = new ImageControl(accel.images[i]);
				imageControl.lblQ.Content = (i + 1).ToString();
				stack.Children.Add(imageControl);
			}
			DataContext = accel;
		}
	}
}
