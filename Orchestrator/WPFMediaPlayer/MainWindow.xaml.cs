using CamEmoOrc;
using Emotional.Models;
using Microsoft.Win32;
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
        private RuntimeVisualization.MainWindow _Runtime;
        private VideoExecution _videoExecutionInstance;

        public MainWindow()
        {
            _playState = false;
            _startExecution = false;
            _videoLoaded = false;
            InitializeComponent();
            _Orchestrator = new BasicOrchestrator(Settings.Default.SamplingRate);
            _Runtime = new RuntimeVisualization.MainWindow();
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
                    ShowFinalScoresForm();
                    break;
            }
        }

        private void ShowFinalScoresForm()
        {
            _Orchestrator.ShowFinalVisualization();
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

                _Runtime.Show();

                _Orchestrator.Start(_videoExecutionInstance, _Runtime);
            }

            if (_videoLoaded)
            {
                if (!_playState) mediaElement.Play();
                else mediaElement.Pause();
                _playState = !_playState;
            }
        }

        private void OpenFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.DefaultExt = "*.*";
            ofd.Filter = "Media (*.*)|*.*";
            ofd.ShowDialog();
            mediaElement.Source = new Uri(ofd.FileName);
            _videoLoaded = true;

            #region setup video execution instance

            string fullPath = mediaElement.Source.ToString();
            int pos = fullPath.LastIndexOf("/") + 1;
            string filename = fullPath.Substring(pos, fullPath.Length - pos);

            _videoExecutionInstance = new VideoExecution()
            {
                FileName = filename,
                FullPath = fullPath,
                Height = (int)mediaElement.ActualHeight,
                Width = (int)mediaElement.ActualWidth,
                VideoLength = mediaElement.NaturalDuration.TimeSpan
            };

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
            _Runtime.Hide();

            mediaElement.Source = null;
        }
    }
}
