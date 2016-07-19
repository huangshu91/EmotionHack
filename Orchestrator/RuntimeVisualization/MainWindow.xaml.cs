namespace RuntimeVisualization
{
    using System.Windows;
    using Emotional.Models;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        EmotionGraph model;

        public MainWindow()
        {
            model = new EmotionGraph();
            DataContext = model;

            InitializeComponent();
        }

        public void UpdateData(EmotionScore emo)
        {
            model.UpdateScore(emo);
        }
    }
}
