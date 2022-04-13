using Server.Information;
using Server.QuestionClass;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Client.MCClient.GamesControl.Components
{
	/// <summary>
	/// Interaction logic for QuestionControl.xaml
	/// </summary>
	public partial class QuestionControl : UserControl
	{
		public QuestionControl()
		{
			InitializeComponent();
		}
		public void DisplayQuestion(OQuestion ques)
		{
			txtQuestion.Text = ques.question;
			txtAnswer.Text = ques.answer;
			try {
				media.Source = new Uri(HelperClass.PathString("Media", ques.attach));
				media.Position = TimeSpan.Zero; media.Play();
			} catch { }
		}
	}
}
