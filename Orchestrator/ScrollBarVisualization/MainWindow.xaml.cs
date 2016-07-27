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

            Model.TimelineModel.SetHistory(History);
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
            Model.PieViewModel.UpdateScore(emo);
        }

        private void playbackSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!_dragStarted)
            {
                UpdateVisuals(playbackSlider.Value);
            }
        }

        private void playbackSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            UpdateVisuals(playbackSlider.Value);
            this._dragStarted = false;
        }

        private void playbackSlider_DragStarted(object sender, DragStartedEventArgs e)
        {
            this._dragStarted = true;
        }

        private void UpdateVisuals(double newPosition)
        {
            SeekPlaybackToNewPosition(newPosition);
            UpdateData((EmotionScore)History[newPosition * 1000]);
        }

        private void SeekPlaybackToNewPosition(double newPosition)
        {
            playbackMediaElement.Play();
            playbackMediaElement.Position = TimeSpan.FromMilliseconds((1000 * newPosition * SamplingRate) - 50);
            //HACK: putting a sleep here for 100 ms, so that the video actually renders before doing the pause
            Thread.Sleep(50);
            playbackMediaElement.Pause();
        }
    }
}
