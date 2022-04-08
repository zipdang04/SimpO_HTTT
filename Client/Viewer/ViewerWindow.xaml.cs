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
using System.Windows.Shapes;

using System.IO;
using System.IO.Compression;
using Server.Information;
using SimpleSockets;
using SimpleSockets.Client;
using Client.PlayerClient.GamesControl;
using Client.PlayerClient.GamesControl.Components;
using Server.QuestionClass;
using Client.Viewer.GamesControl;

namespace Client.Viewer
{
    /// <summary>
    /// Interaction logic for ViewerWindow.xaml
    /// </summary>
    public partial class ViewerWindow : Window
    {
        LogInWindow logInWindow;
        SimpleSocketClient client;

        StartViewerControl startViewerControl;
        public ViewerWindow(LogInWindow logInWindow, SimpleSocketClient client)
        {
            InitializeComponent();
            this.logInWindow = logInWindow; this.client = client;

            startViewerControl = new StartViewerControl();
        }
	}
}
