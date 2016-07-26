namespace ScrollBarVisualization
{
    using Emotional.Models;
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private PieViewModel Model;

        public MainWindow()
        {
            InitializeComponent();

            //Model = (PieViewModel)this.DataContext;
        }

        public void UpdateData(EmotionScore emo)
        {
            //Model.UpdateData(emo);
        }
    }
}
