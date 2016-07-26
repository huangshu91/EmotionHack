namespace SliderPlaybackVisualization
{
    using Emotional.Models;
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SliderWindow : Window
    {
        private PieViewModel Model;
        private TimeSpan videoExecutionDuration;
        private const double SamplingRate = 1;

        public SliderWindow()
        {
            InitializeComponent();

            Model = (PieViewModel)this.DataContext;
        }

        public void LoadVideoExecution(VideoExecution videoExecutionInstance)
        {
            playbackMediaElement.Source = new Uri(videoExecutionInstance.FullPath);
            videoExecutionDuration = videoExecutionInstance.VideoLength;

            //Setup the slider
            playbackSlider.BeginInit();
            playbackSlider.TickFrequency = SamplingRate;
            playbackSlider.Maximum = (videoExecutionDuration.TotalSeconds / SamplingRate);
            playbackSlider.EndInit();
        }

        public void UpdateData(EmotionScore emo)
        {
            //Model.UpdateScore(emo);
        }

        private void playbackSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MoveMediaElementvideoToPosition(playbackSlider.Value);
        }

        private void MoveMediaElementvideoToPosition(double newPosition)
        {
            playbackMediaElement.Position = TimeSpan.FromSeconds(newPosition * SamplingRate);
            playbackMediaElement.Play();
            playbackMediaElement.Pause();
        }
    }
}
