namespace RuntimeVisualization
{
    using System.Windows;
    using Emotional.Models;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private EmotionGraph Model;

        public MainWindow()
        {
            //model = new EmotionGraph();
            //DataContext = model;

            InitializeComponent();

            Model = (EmotionGraph) this.DataContext;
        }

        public void UpdateData(EmotionScore emo)
        {
            Model.UpdateScore(emo);
        }
    }
}
