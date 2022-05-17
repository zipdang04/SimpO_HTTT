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
        MediaElement mediaStart = new MediaElement(), mediaRun = new MediaElement();
        //MediaElement[] mediaStart = new MediaElement[4];
        //MediaElement[] mediaRun = new MediaElement[4];
        MediaPlayer mediaOpening = new MediaPlayer(), 
                    mediaCorrect = new MediaPlayer(), 
                    mediaWrong = new MediaPlayer(), 
                    mediaBlank = new MediaPlayer(), 
                    mediaDone = new MediaPlayer();
        public StartViewerControl(PlayerClass playerClass)
        {
            InitializeComponent();
            this.playerClass = playerClass;
            qnpBox.SetContext(playerClass);

            mediaStart.LoadedBehavior = MediaState.Manual;
            mediaRun.LoadedBehavior = MediaState.Manual;
            grid.Children.Add(mediaStart);
			grid.Children.Add(mediaRun);

			mediaOpening.Open(new Uri(HelperClass.PathString("Effects", "KD_Opening.mpeg")));
            mediaOpening.Play(); mediaOpening.Stop();
            mediaCorrect.Open(new Uri(HelperClass.PathString("Effects", "KD_Correct.m4a"))); 
            mediaCorrect.Play(); mediaCorrect.Stop();
            mediaWrong.Open(new Uri(HelperClass.PathString("Effects", "KD_Wrong.m4a"))); 
            mediaWrong.Play(); mediaWrong.Stop();
            mediaBlank.Open(new Uri(HelperClass.PathString("Effects", "Blank.mp3"))); 
            mediaBlank.Play(); mediaBlank.Stop();
            mediaDone.Open(new Uri(HelperClass.PathString("Effects", "KD_Done.m4a"))); 
            mediaDone.Play(); mediaDone.Stop();
            Grid.SetZIndex(qnpBox, 100);
        }

        public void TurnOff()
		{
            for (int i = 0; i < 4; i++) {
                mediaStart.Visibility = Visibility.Collapsed;
                mediaRun.Visibility = Visibility.Collapsed;
            }
		}
        public void StartPlayer(int player)
		{
            this.player = player;
            //string attach = HelperClass.PathString("Effects", string.Format("KD_{0}_Start.mp4", player + 1));
            Dispatcher.Invoke(() => {
                mediaStart.Source = new Uri(HelperClass.PathString("Effects", string.Format("KD_{0}_Start.mp4", player + 1)));
                mediaRun.Source = new Uri(HelperClass.PathString("Effects", string.Format("KD_{0}_Run.mp4", player + 1)));
                mediaRun.Play(); mediaRun.Stop();
                media.Visibility = Visibility.Hidden;
                TurnOff();
                qnpBox.SetHiddenAll();
                //media.Source = new Uri(attach);
                //media.Play();
                mediaStart.Position = TimeSpan.Zero;
                mediaStart.Visibility = Visibility.Visible;
                mediaStart.Play();
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
                media.Visibility = Visibility.Visible;
                mediaRun.Position = TimeSpan.Zero;
                mediaRun.Visibility = Visibility.Visible;
                mediaRun.Play();
            });
        }
        public void Opening() { Dispatcher.Invoke(() => { mediaOpening.Position = TimeSpan.Zero; mediaOpening.Play(); }); }

		private void media_Opened(object sender, RoutedEventArgs e)
		{
            Dispatcher.Invoke(() =>
            {
                // media.Position = TimeSpan.Zero;
                media.Play();
            });
		}

		public void Correct() { Dispatcher.Invoke(() => { mediaCorrect.Position = TimeSpan.Zero; mediaCorrect.Play(); }); }
        public void Wrong() { Dispatcher.Invoke(() => { mediaWrong.Position = TimeSpan.Zero; mediaWrong.Play(); }); }
        public void Blank() { Dispatcher.Invoke(() => { mediaBlank.Position = TimeSpan.Zero; mediaBlank.Play(); }); }
        public void Done() { 
            Dispatcher.Invoke(() => { 
                mediaDone.Position = TimeSpan.Zero; mediaDone.Play();
                media.Source = null;
                TurnOff(); qnpBox.SetHiddenAll();
            }); 
        }

        public void DisplayQuestion(string question, string attach)
		{
            qnpBox.SetQuestion(question);
            Dispatcher.Invoke(() => {
				try {
                    media.Source = new Uri(HelperClass.PathString("Media", attach));
                    media.Play();
                } catch { 
                };
			});
        }
    }
}
