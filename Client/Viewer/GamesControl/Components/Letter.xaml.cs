using Server.Information;
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

namespace Client.Viewer.GamesControl.Components
{
	/// <summary>
	/// Interaction logic for Letter.xaml
	/// </summary>
	public partial class Letter : UserControl
	{
		public enum LetterState { NORMAL, CHOOSING, ENABLE, DISABLE };
		public static Uri NORMAL = new Uri(HelperClass.PathString("Effects", "VCNV_LetterNormal.png"));
		public static Uri CHOOSING = new Uri(HelperClass.PathString("Effects", "VCNV_LetterChoosing.png"));
		public static Uri ENABLED = new Uri(HelperClass.PathString("Effects", "VCNV_LetterEnabled.png"));
		public static Uri DISABLED = new Uri(HelperClass.PathString("Effects", "VCNV_LetterDisabled.png"));
		public Letter()
		{
			InitializeComponent();
			SetNormal();
		}
		public Letter(char c): this() { SetChar(c); }

		public void SetChar(char c) { Dispatcher.Invoke(() => { lblChar.Content = Char.ToUpper(c); }); }

		public void SetNull() { Dispatcher.Invoke(() => { background.ImageSource = null; lblChar.Visibility = Visibility.Hidden; Visibility = Visibility.Hidden; }); }
		public void SetNormal() { Dispatcher.Invoke(() => { background.ImageSource = new BitmapImage(NORMAL); lblChar.Visibility = Visibility.Hidden; Visibility = Visibility.Visible; }); }
		public void SetChoosing() { Dispatcher.Invoke(() => { background.ImageSource = new BitmapImage(CHOOSING); lblChar.Visibility = Visibility.Hidden; Visibility = Visibility.Visible; }); }
		public void SetEnabled() { Dispatcher.Invoke(() => { background.ImageSource = new BitmapImage(ENABLED); lblChar.Visibility = Visibility.Visible; Visibility = Visibility.Visible; }); }
		public void SetDisabled() { Dispatcher.Invoke(() => { background.ImageSource = new BitmapImage(DISABLED); lblChar.Visibility = Visibility.Hidden; Visibility = Visibility.Visible; }); }
	}
}
