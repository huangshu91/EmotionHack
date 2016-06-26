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
        public int width { get; set; }

        public int height { get; set; }

        public string fileName { get; set; }
    }
}
