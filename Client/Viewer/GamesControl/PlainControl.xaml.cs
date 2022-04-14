using Server.Information;
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

namespace Client.Viewer.GamesControl
{
    /// <summary>
    /// Interaction logic for PlainControl.xaml
    /// </summary>
    public partial class PlainControl : UserControl
    {
        public PlainControl()
        {
            InitializeComponent();
        }
        public void Play(string attach)
		{
            attach = HelperClass.PathString("Effects", attach);
            Dispatcher.Invoke(() => {
                media.Source = new Uri(attach);
                media.Play();
            });
		}
        public void Media(string attach)
		{
            attach = Directory.GetCurrentDirectory() + @"\Resources\" + attach;
            Dispatcher.Invoke(() => {
                media.Source = new Uri(attach);
                media.Play();
            });
        }
    }
}
