using System;
using System.Windows.Forms;
using Emotional.Core;

namespace Orchestrator
{
    public partial class VideoPlayerForm : Form
    {
        public VideoPlayerForm()
        {
            InitializeComponent();
        }

        private void VideoPlayerForm_Load(object sender, EventArgs e)
        {
            IEmotionClient client = new EmotionClient();

            axWindowsMediaPlayer1.settings.autoStart = false;
            axWindowsMediaPlayer1.URL =
            @"C:\Oneweek\Sample short video for testing.mp4";
        }
    }
}
