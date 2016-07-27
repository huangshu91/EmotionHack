using CamEmoOrc;
using Emotional.Models;
using Microsoft.Win32;
using SliderPlaybackVisualization;
using System;
using System.Windows;
using System.Windows.Input;

namespace WPFMediaPlayer
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _playState, _startExecution, _videoLoaded;
        private IOrchestrator _Orchestrator;
        private RuntimeVisualization.RuntimeWindow _Runtime;
        private VideoExecution _videoExecutionInstance;

        public MainWindow()
        {
            _playState = false;
            _startExecution = false;
            _videoLoaded = false;
            InitializeComponent();
            _Orchestrator = new BasicOrchestrator(Settings.Default.SamplingRate);
            _Runtime = new RuntimeVisualization.RuntimeWindow();
        }

        #region media player options

        private void mediaElement_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.O:
                    OpenFile();
                    break;
                case Key.P:
                    PlayPause();
                    break;
                case Key.S:
                    Stop();
                    break;
                case Key.F:
                    WindowState = WindowState.Maximized;
                    WindowStyle = WindowStyle.None;
                    break;
                case Key.G:
                    WindowState = WindowState.Normal;
                    WindowStyle = WindowStyle.SingleBorderWindow;
                    break;
                case Key.L:
                    ShowPostPlaybackForms();
                    break;
            }
        }

        private void ShowPostPlaybackForms()
        {
            _Orchestrator.ShowPostPlaybackVisualizations(_videoExecutionInstance);

            SliderWindow _slider = new SliderWindow(_Orchestrator.GetExecutionScores());
            _slider.LoadVideoExecution(_videoExecutionInstance);
            _slider.Show();
        }

        private void Stop()
        {
            FinishExecution();
        }

        private void PlayPause()
        {
            if (!_startExecution && _videoLoaded)
            {
                _startExecution = true;

                _Orchestrator.Start(_videoExecutionInstance, _Runtime);
            }

            if (_videoLoaded)
            {
                if (!_playState) mediaElement.Play();
                else mediaElement.Pause();
                _playState = !_playState;
                //Setting the video execution duration here, since it's only available after the video is playing
                if (_playState)
                {
                    //HACK to wait until the timespan is actually available
                    while (!mediaElement.NaturalDuration.HasTimeSpan) ;
                    _videoExecutionInstance.VideoLength = mediaElement.NaturalDuration.TimeSpan;
                }
            }
        }

        private void OpenFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.DefaultExt = "*.*";
            ofd.Filter = "Media (*.*)|*.*";
            ofd.ShowDialog();

            try
            {
                mediaElement.Source = new Uri(ofd.FileName);
                _videoLoaded = true;
            }
            catch
            {
                _videoLoaded = false;
                //IGNORE whatever just happened
                return;
            }

            #region setup video execution instance

            string fullPath = mediaElement.Source.ToString();
            int pos = fullPath.LastIndexOf("/") + 1;
            string filename = fullPath.Substring(pos, fullPath.Length - pos);

            _videoExecutionInstance = new VideoExecution()
            {
                FileName = filename,
                FullPath = fullPath,
                Height = (int)mediaElement.ActualHeight,
                Width = (int)mediaElement.ActualWidth
            };

            _Runtime.Show();

            #endregion
        }

        #endregion

        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            FinishExecution();
        }

        private void FinishExecution()
        {
            //Add check if already stopped, if needed
            mediaElement.Stop();

            if (_startExecution)
            {
                _startExecution = false;
                _Orchestrator.FinishExecution();
            }
            _playState = false;
            _videoLoaded = false;
            _Runtime.Finish();

            mediaElement.Source = null;
        }
    }
}
