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

namespace Client.PlayerClient.GamesControl.Components
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
		public void DisplayQuestion(string question, string attach = "")
		{
			txtQuestion.Text = question;
			try { media.Source = new Uri(Directory.GetCurrentDirectory() + @"\Resources\Media\" + attach); }catch { }
		}
	}
}
