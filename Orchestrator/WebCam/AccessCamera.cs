using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing.Imaging;
using System.IO;

namespace WebCam
{
    public class AccessCamera
    {
        MemoryStream ms;
        private FilterInfoCollection webcam;
        private VideoCaptureDevice cam;

        /// <summary>
        /// Identifies the web cam available on the machine and sets up the AccessCamera object
        /// </summary>
        public AccessCamera()
        {
            ms = new System.IO.MemoryStream();
            webcam = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            cam = new VideoCaptureDevice(webcam[0].MonikerString);
            cam.NewFrame += new NewFrameEventHandler(cam_NewFrame);
        }

        /// <summary>
        /// Turns on the web cam for recording and capturing images
        /// </summary>
        public void cam_Start()
        {
            cam.Start();
        }


        private void cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bit = (Bitmap)eventArgs.Frame.Clone();
            bit.Save(ms, ImageFormat.Jpeg);
            ms.Position = 0;
        }

        /// <summary>
        /// shuts down the web cam
        /// </summary>
        public void cam_Stop()
        {
            if (cam.IsRunning)
            {
                cam.Stop();
            }
        }

        /// <summary>
        /// Captures an image from the web cam and returns it a a mem stream
        /// </summary>
        /// <returns></returns>
        public MemoryStream cam_TakePic()
        {
            return ms;
        }
    }
}
