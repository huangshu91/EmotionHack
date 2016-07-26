using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emotional.Models
{
    /// <summary>
    /// Definition of the currently playing video instance (including the user).
    /// </summary>
    public class VideoExecution
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public string FileName { get; set; }

        public string FullPath { get; set; }

        public TimeSpan VideoLength { get; set; }
    }
}
