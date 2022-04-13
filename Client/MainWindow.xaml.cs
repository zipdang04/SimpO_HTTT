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

namespace Client
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		PlayerClient.LogInWindow player;
		Viewer.LogInWindow viewer;
		MCClient.LogInWindow mcClient;
		public MainWindow()
		{
			InitializeComponent();
		}

		private void btnClient_Click(object sender, RoutedEventArgs e)
		{
			player = new PlayerClient.LogInWindow(this);
			player.Show(); Hide();
		}

		private void btnViewer_Click(object sender, RoutedEventArgs e)
		{
			viewer = new Viewer.LogInWindow(this);
			viewer.Show(); Hide();
		}

		private void btnMC_Click(object sender, RoutedEventArgs e)
		{
			mcClient = new MCClient.LogInWindow(this);
			mcClient.Show(); Hide();
		}
	}
}
