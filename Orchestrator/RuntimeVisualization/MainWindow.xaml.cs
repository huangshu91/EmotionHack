namespace RuntimeVisualization
{
    using System.Windows;
    using Emotional.Models;
    using System.IO;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class RuntimeWindow : Window
    {
        private EmotionGraph Model;
        private WebcamView secondary;

        public RuntimeWindow()
        {
            //model = new EmotionGraph();
            //DataContext = model;

            InitializeComponent();

            Model = (EmotionGraph) this.DataContext;

            SourceInitialized += (s, a) =>
            {
                secondary = new WebcamView();
                secondary.Owner = this;
                secondary.Show();
            };
        }

        public void UpdateData(EmotionScore emo, MemoryStream cam)
        {
            Model.UpdateScore(emo);
            secondary.UpdateWebCamView(cam);
        }

        public void Finish()
        {
            this.Hide();
            secondary.Hide();
        }

        public void Start()
        {
            this.Show();
            secondary.Show();
        }
    }
}
