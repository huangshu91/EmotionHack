using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace RuntimeVisualization
{
    /// <summary>
    /// Interaction logic for WebcamView.xaml
    /// </summary>
    public partial class WebcamView : Window
    {
        public WebcamView()
        {
            InitializeComponent();

            // Set the stretch property.
            this.image.Stretch = Stretch.Fill;

            // Set the StretchDirection property.
            this.image.StretchDirection = StretchDirection.Both;
        }

        public void UpdateWebCamView(MemoryStream stream)
        {
            if (stream.CanRead)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    try
                    {
                        var bitImage = new BitmapImage();

                        stream.Position = 0;
                        bitImage.BeginInit();
                        bitImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitImage.StreamSource = stream;
                        bitImage.EndInit();

                        bitImage.Freeze();

                        this.image.Source = bitImage;
                    }
                    finally
                    {

                    }
                }));
            }
        }
    }
}
