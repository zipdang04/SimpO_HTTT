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

using Server.Information;

namespace Client.Viewer.GamesControl
{
    /// <summary>
    /// Interaction logic for StartViewerControl.xaml
    /// </summary>
    public partial class StartViewerControl : UserControl
    {
        PlayerClass playerClass;
        int player = -1;
        public StartViewerControl(PlayerClass playerClass)
        {
            InitializeComponent();
            this.playerClass = playerClass;
            qnpBox.SetContext(playerClass);
            mediaCorrect.Source = new Uri(HelperClass.PathString("Effects", "KD_Correct.m4a")); mediaCorrect.BeginInit();
            mediaWrong.Source = new Uri(HelperClass.PathString("Effects", "KD_Wrong.m4a")); mediaWrong.BeginInit();
        }

        public void StartPlayer(int player)
		{
            this.player = player;
            string attach = HelperClass.PathString("Effects", string.Format("KD_{0}_Start.mp4", player + 1));

            Dispatcher.Invoke(() => {
                qnpBox.SetHiddenAll();
                media.Source = new Uri(attach);
                media.Play();
            });
		}

        public void RunPlayer()
		{
            string attach = HelperClass.PathString("Effects", string.Format("KD_{0}_Run.mp4", player + 1));

            Dispatcher.Invoke(() => {
                qnpBox.SetChosenOne(player);
                media.Source = new Uri(attach);
                media.Play();
            });
        }
        public void Correct() { Dispatcher.Invoke(() => { mediaCorrect.Position = TimeSpan.Zero; mediaCorrect.Play(); }); }
        public void Wrong() { Dispatcher.Invoke(() => { mediaWrong.Position = TimeSpan.Zero; mediaWrong.Play(); }); }

        public void DisplayQuestion(string question, string attach)
		{
            qnpBox.SetQuestion(question);
		}
    }
}
