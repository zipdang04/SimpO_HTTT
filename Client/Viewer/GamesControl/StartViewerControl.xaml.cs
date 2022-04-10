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
        MediaElement[] mediaStart = new MediaElement[4];
        MediaElement[] mediaRun = new MediaElement[4];
        public StartViewerControl(PlayerClass playerClass)
        {
            InitializeComponent();
            this.playerClass = playerClass;
            qnpBox.SetContext(playerClass);

            for (int i = 0; i < 4; i++) {
                mediaStart[i] = new MediaElement();
                mediaStart[i].LoadedBehavior = MediaState.Manual;
                mediaStart[i].Source = new Uri(HelperClass.PathString("Effects", string.Format("KD_{0}_Start.mp4", i + 1)));
                mediaStart[i].Play(); mediaStart[i].Stop();
                mediaStart[i].Visibility = Visibility.Collapsed;

                mediaRun[i] = new MediaElement();
                mediaRun[i].LoadedBehavior = MediaState.Manual;
                mediaRun[i].Source = new Uri(HelperClass.PathString("Effects", string.Format("KD_{0}_Run.mp4", i + 1)));
                mediaRun[i].Play(); mediaRun[i].Stop();
                mediaRun[i].Visibility = Visibility.Collapsed;

                grid.Children.Add(mediaStart[i]);
                grid.Children.Add(mediaRun[i]);
            }

            mediaOpening.Source = new Uri(HelperClass.PathString("Effects", "KD_Opening.mpeg"));
            mediaOpening.BeginInit(); mediaOpening.Play(); mediaOpening.Stop();
            mediaCorrect.Source = new Uri(HelperClass.PathString("Effects", "KD_Correct.m4a")); 
            mediaCorrect.BeginInit(); mediaCorrect.Play(); mediaCorrect.Stop();
            mediaWrong.Source = new Uri(HelperClass.PathString("Effects", "KD_Wrong.m4a")); 
            mediaWrong.BeginInit(); mediaWrong.Play(); mediaWrong.Stop();
            mediaBlank.Source = new Uri(HelperClass.PathString("Effects", "Blank.mp3")); 
            mediaBlank.BeginInit(); mediaBlank.Play(); mediaBlank.Stop();
            mediaDone.Source = new Uri(HelperClass.PathString("Effects", "KD_Done.m4a")); 
            mediaDone.BeginInit(); mediaDone.Play(); mediaDone.Stop();
            Grid.SetZIndex(qnpBox, 100);
        }

        public void TurnOff()
		{
            for (int i = 0; i < 4; i++) {
                mediaStart[i].Visibility = Visibility.Collapsed;
                mediaRun[i].Visibility = Visibility.Collapsed;
            }
		}
        public void StartPlayer(int player)
		{
            this.player = player;
            //string attach = HelperClass.PathString("Effects", string.Format("KD_{0}_Start.mp4", player + 1));
            Dispatcher.Invoke(() => {
                TurnOff();
                qnpBox.SetHiddenAll();
                //media.Source = new Uri(attach);
                //media.Play();
                mediaStart[player].Position = TimeSpan.Zero;
                mediaStart[player].Visibility = Visibility.Visible;
                mediaStart[player].Play();
            });
		}

        public void RunPlayer()
		{
            //string attach = HelperClass.PathString("Effects", string.Format("KD_{0}_Run.mp4", player + 1));

            Dispatcher.Invoke(() => {
                TurnOff();
                qnpBox.SetChosenOne(player);
                qnpBox.Visibility = Visibility.Visible;
                //media.Source = new Uri(attach);
                //media.Play();
                mediaRun[player].Position = TimeSpan.Zero;
                mediaRun[player].Visibility = Visibility.Visible;
                mediaRun[player].Play();
            });
        }
        public void Opening() { Dispatcher.Invoke(() => { mediaOpening.Position = TimeSpan.Zero; mediaOpening.Play(); }); }
        public void Correct() { Dispatcher.Invoke(() => { mediaCorrect.Position = TimeSpan.Zero; mediaCorrect.Play(); }); }
        public void Wrong() { Dispatcher.Invoke(() => { mediaWrong.Position = TimeSpan.Zero; mediaWrong.Play(); }); }
        public void Blank() { Dispatcher.Invoke(() => { mediaBlank.Position = TimeSpan.Zero; mediaBlank.Play(); }); }
        public void Done() { 
            Dispatcher.Invoke(() => { 
                mediaDone.Position = TimeSpan.Zero; mediaDone.Play();
                TurnOff(); qnpBox.SetHiddenAll();
            }); 
        }

        public void DisplayQuestion(string question, string attach)
		{
            qnpBox.SetQuestion(question);
		}
    }
}
