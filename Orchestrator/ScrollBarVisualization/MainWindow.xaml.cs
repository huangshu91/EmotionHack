namespace SliderPlaybackVisualization
{
    using Emotional.Models;
    using System;
    using System.Windows;
    using System.Collections.Specialized;
    using System.Windows.Controls.Primitives;
    using System.Windows.Controls;
    using System.Threading;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SliderWindow : Window
    {
        private GroupedModel Model;
        private TimeSpan videoExecutionDuration;
        private OrderedDictionary History;
        private const double SamplingRate = 1;
        private bool _dragStarted = false;

        public SliderWindow()
        {
            InitializeComponent();

            Model = (GroupedModel)this.DataContext;
        }

        public SliderWindow(OrderedDictionary history)
        {
            InitializeComponent();

            History = history;
            Model = (GroupedModel)this.DataContext;
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
            if (!_dragStarted)
            {
                SeekPlaybackToNewPosition(playbackSlider.Value);
            }
        }

        private void playbackSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            SeekPlaybackToNewPosition(playbackSlider.Value);
            this._dragStarted = false;
        }

        private void playbackSlider_DragStarted(object sender, DragStartedEventArgs e)
        {
            this._dragStarted = true;
        }

        private void SeekPlaybackToNewPosition(double newPosition)
        {
            playbackMediaElement.Play();
            playbackMediaElement.Position = TimeSpan.FromMilliseconds((1000 * newPosition * SamplingRate) - 100);
            //HACK: putting a sleep here for 100 ms, so that the video actually renders before doing the pause
            Thread.Sleep(100);
            playbackMediaElement.Pause();
        }
    }
}
