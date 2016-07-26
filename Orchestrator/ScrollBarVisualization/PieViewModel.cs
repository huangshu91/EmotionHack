namespace ScrollBarVisualization
{
    using Emotional.Models;
    using OxyPlot;
    using OxyPlot.Series;

    public class PieViewModel
    {
        public PlotModel MyModel { get; private set; }

        public PieViewModel()
        {
            MyModel = new PlotModel { Title = "Pie Sample1" };

            dynamic seriesP1 = new PieSeries { StrokeThickness = 2.0, InsideLabelPosition = 0.8, AngleSpan = 360, StartAngle = 0 };

            seriesP1.Slices.Add(new PieSlice("Africa", 1030) { IsExploded = false, Fill = OxyColors.PaleVioletRed });
            seriesP1.Slices.Add(new PieSlice("Americas", 929) { IsExploded = true });
            seriesP1.Slices.Add(new PieSlice("Asia", 4157) { IsExploded = true });
            seriesP1.Slices.Add(new PieSlice("Europe", 739) { IsExploded = true });
            seriesP1.Slices.Add(new PieSlice("Oceania", 35) { IsExploded = true });

            MyModel.Series.Add(seriesP1);
        }

        public void UpdateData(EmotionScore emo)
        {
            //Model.UpdateScore(emo);
        }
    }

}
