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

namespace Client.Viewer.GamesControl
{
	/// <summary>
	/// Interaction logic for ObstaViewerControl.xaml
	/// </summary>
	public partial class ObstaViewerControl : UserControl
	{
		public ObstaViewerControl()
		{
			InitializeComponent();
		}

		public void ResetGame()
		{
			//
		}

		public void Opening() { Dispatcher.Invoke(() => { mediaOpening.Position = TimeSpan.Zero; mediaOpening.Play(); }); }
		public void ChangeScene(string s)
		{
			Dispatcher.Invoke(() => { 
				gridPic.Visibility = Visibility.Hidden;
				gridWord.Visibility = Visibility.Hidden;
				if (s == "PIC") gridPic.Visibility = Visibility.Visible;
				else if (s == "WORD") gridPic.Visibility = Visibility.Hidden;
			});
		}
	}
}
