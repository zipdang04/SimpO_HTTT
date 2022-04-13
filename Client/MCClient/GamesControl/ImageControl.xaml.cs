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

namespace Client.MCClient.GamesControl
{
	/// <summary>
	/// Interaction logic for ImageControl.xaml
	/// </summary>
	public partial class ImageControl : UserControl
	{
		string path = "";
		double width, height;
		List<Rectangle> rects = new List<Rectangle>();

		public ImageControl()
		{
			InitializeComponent();
			rects = new List<Rectangle>();

			rects.Add(rectUL);
			rects.Add(rectUR);
			rects.Add(rectDL);
			rects.Add(rectDR);
			rects.Add(rectTT);
		}

		public void setImage(string fileName)
		{
			try {
				Dispatcher.Invoke(() => {
					path = Directory.GetCurrentDirectory() + @"\Resources\Media\" + fileName;
					image.Source = new BitmapImage(new Uri(path));

					for (int i = 0; i < 5; i++) rects[i].Visibility = Visibility.Visible;
					reprocess();
				});
			}
			catch {
				this.path = "";
			};
		}

		void reprocess()
		{
			width = image.RenderSize.Width / 2.0;
			height = image.RenderSize.Height / 2.0;
			for (int i = 0; i < 5; i++)
			{
				rects[i].Width = width;
				rects[i].Height = height;
			}
			for (int i = 0; i < 4; i++)
			{
				double addW = width, addH = height;
				if (i % 2 == 0) addW = -addW;
				if (i / 2 == 0) addH = -addH;
				rects[i].Margin = new Thickness(addW, addH, 0, 0);
			}
		}

		private void image_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			reprocess();
		}

		public void erase(int index)
		{
			Dispatcher.Invoke(() => {rects[index].Visibility = Visibility.Hidden;});
		}
	}
}
