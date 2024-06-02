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
using System.Windows.Threading;

namespace BookingApp.View.Guest.Windows
{
    /// <summary>
    /// Interaction logic for GuestTutorial.xaml
    /// </summary>
    public partial class GuestTutorial : Window
    {
        private DispatcherTimer timer;
        public GuestTutorial()
        {
            InitializeComponent();
            InitializeVideoPlayer();
            //videoPlayer.Source = new Uri("../../Resource/Video/proba.mp4", UriKind.RelativeOrAbsolute);
            string videoPath = "../../../Resources/Video/proba.mp4";
            videoPlayer.Source = new Uri(videoPath, UriKind.RelativeOrAbsolute);
        }
        private void InitializeVideoPlayer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (videoPlayer.NaturalDuration.HasTimeSpan)
            {
                timelineSlider.Maximum = videoPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                timelineSlider.Value = videoPlayer.Position.TotalSeconds;
            }
        }
        private void PlayVideo()
        {
            videoPlayer.Play();
            timer.Start();
        }

        private void PauseVideo()
        {
            videoPlayer.Pause();
            timer.Stop();
        }
        private void StopVideo()
        {
            videoPlayer.Stop();
            timer.Stop();
            timelineSlider.Value = 0;
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
        private void RewindButton_Click(object sender, RoutedEventArgs e)
        {
            videoPlayer.Position = videoPlayer.Position.Subtract(TimeSpan.FromSeconds(10));
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            videoPlayer.Position = videoPlayer.Position.Add(TimeSpan.FromSeconds(10));
        }

        private void TimelineSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (videoPlayer.NaturalDuration.HasTimeSpan && timelineSlider.IsMouseCaptureWithin)
            {
                videoPlayer.Position = TimeSpan.FromSeconds(timelineSlider.Value);
            }
        }
        private void VideoPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            videoPlayer.Stop();
            videoPlayer.Position = TimeSpan.Zero;
            timelineSlider.Value = 0;
        }
    }
}
