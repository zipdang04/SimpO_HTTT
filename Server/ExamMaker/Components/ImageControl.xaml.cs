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
	/// Interaction logic for ImageControl.xaml
	/// </summary>
	public partial class ImageControl : UserControl
	{
		public ImageClass image { get; set; }
		public ImageControl(ImageClass image)
		{
			InitializeComponent();
			this.image = image;
			DataContext = image;
		}
	}
}
