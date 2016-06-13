using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            axWindowsMediaPlayer1.URL =
            @"C:\Oneweek\Sample short video for testing.mp4";
        }
    }
}
