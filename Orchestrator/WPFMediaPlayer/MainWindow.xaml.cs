using CamEmoOrc;
using Emotional.Models;
using Microsoft.Win32;
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

namespace WPFMediaPlayer
{
    using Visualization;
    using RuntimeVisualization;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _playState, _startExecution;
        private IOrchestrator _Orchestrator;
        public MainWindow()
        {
            _playState = false;
            _startExecution = false;
            InitializeComponent();
            _Orchestrator = new BasicOrchestrator(Settings.Default.SamplingRate);
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
                case Key.T:
                    Testing();
                    break;
                case Key.L:
                    Show();
                    break;
            }
        }

        private void Show()
        {
            //TODO vivek: Hook up Orchestrator with final visualization(Form)
            //We will want to figure out a way to display a Form from here. Code commented out is how the call would work in Program.cs in visualization 
            var result = _Orchestrator.getHistory();
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Form1 graph = new Form1(result.Count);
            //Application.Run(graph);
            //graph.ShowGraphs(result);
        }

        private void Testing()
        {
            _Orchestrator.FinishExecution();
        }

        private void Stop()
        {
            mediaElement.Stop();
            _playState = false;
        }

        private void PlayPause()
        {
            if (!_startExecution)
            {
                _startExecution = true;
                VideoExecution vidEx = new VideoExecution()
                {
                    fileName = mediaElement.Source.ToString(),
                    height = (int) mediaElement.ActualHeight,
                    width = (int) mediaElement.ActualWidth
                };

                var runtime = new RuntimeVisualization.MainWindow();
                runtime.Show();

                _Orchestrator.Start(vidEx, runtime);
            }

            if (!_playState) mediaElement.Play();
            else mediaElement.Pause();
            _playState = !_playState;
        }

        private void OpenFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.DefaultExt = "*.*";
            ofd.Filter = "Media (*.*)|*.*";
            ofd.ShowDialog();
            mediaElement.Source = new Uri(ofd.FileName);
        }

        #endregion

        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (_startExecution)
            {
                _startExecution = false;
                _Orchestrator.Stop();
            }
        }
    }
}
