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

namespace Client.PlayerClient.GamesControl
{
	/// <summary>
	/// Interaction logic for ImageControl.xaml
	/// </summary>
	public partial class ImageControl : UserControl
	{
		string path;
		double width, height;
		List<Rectangle> rects;

		public ImageControl(string path = "")
		{
			InitializeComponent();
			rects = new List<Rectangle>();

			rects.Append(rectUL);
			rects.Append(rectUR);
			rects.Append(rectUL);
			rects.Append(rectUR);
			rects.Append(rectTT);

			path = "";
		}

		public void setImage(string fileName)
		{
			try {
				path = Directory.GetCurrentDirectory() + @"\Resources\" + fileName;
				image.Source = new BitmapImage(new Uri(path));
				width = image.RenderSize.Width / 2.0;
				height = image.RenderSize.Height / 2.0;

				for (int i = 0; i < 5; i++) {
					rects[i].Width = width;
					rects[i].Height = height;
					rects[i].Visibility = Visibility.Visible;
				}
				for (int i = 0; i < 4; i++) {
					double addW = width, addH = height;
					if (i % 2 == 0) addW = -addW;
					if (i / 2 == 0) addH = -addH;
					rects[i].Margin = new Thickness(addW, addH, 0, 0);
				}
			}
			catch {
				this.path = "";
			};
		}

		public void erase(int index)
		{
			rects[index].Visibility = Visibility.Hidden;
		}
	}
}
