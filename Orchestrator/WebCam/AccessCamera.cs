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

namespace WebCam_test
{
    public class AccessCamera
    {
        MemoryStream ms;
        private FilterInfoCollection webcam;
        private VideoCaptureDevice cam;


        public AccessCamera()
        {
            ms = new System.IO.MemoryStream();
            webcam = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            cam = new VideoCaptureDevice(webcam[0].MonikerString);
            cam.NewFrame += new NewFrameEventHandler(cam_NewFrame);
        }

        public void cam_Start()
        {
            cam.Start();
        }

        void cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bit = (Bitmap)eventArgs.Frame.Clone();
            bit.Save(ms, ImageFormat.Jpeg);
            ms.Position = 0;
        }

        public void cam_Stop()
        {
            if (cam.IsRunning)
            {
                cam.Stop();
            }
        }

        public MemoryStream cam_TakePic()
        {
            return ms;
        }
    }
}
