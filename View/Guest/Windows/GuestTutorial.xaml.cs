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

namespace BookingApp.View.Guest.Windows
{
    /// <summary>
    /// Interaction logic for GuestTutorial.xaml
    /// </summary>
    public partial class GuestTutorial : Window
    {
        public GuestTutorial()
        {
            InitializeComponent();
            //videoPlayer.Source = new Uri("../../Resource/Video/proba.mp4", UriKind.RelativeOrAbsolute);
            string videoPath = "../../../Resources/Video/proba.mp4";
            videoPlayer.Source = new Uri(videoPath, UriKind.RelativeOrAbsolute);
        }

        private void PlayVideo()
        {
            videoPlayer.Play();
        }

        private void PauseVideo()
        {
            videoPlayer.Pause();
        }
        private void StopVideo()
        {
            videoPlayer.Stop();
        }
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            PlayVideo();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            PauseVideo();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            StopVideo();
        }
    }
}
