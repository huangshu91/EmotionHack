namespace SliderPlaybackVisualization
{
    using Emotional.Models;
    using OxyPlot;
    using OxyPlot.Series;

    public class PieViewModel
    {
        public PlotModel MyModel { get; private set; }

        public PieViewModel()
        {
            MyModel = new PlotModel();

            var seriesP1 = new PieSeries {
                StrokeThickness = 2.0,
                InsideLabelPosition = 0.6,
                AngleSpan = 360,
                StartAngle = 0,
                InsideLabelColor = OxyColors.Black,
                TextColor = OxyColors.DarkGray,
                Stroke = OxyColors.Gray
            };

            seriesP1.Slices.Add(new PieSlice("No Data Available", 1) { IsExploded = false, Fill = OxyColors.Gray,  });

            MyModel.Series.Add(seriesP1);
        }

        public void UpdateScore(EmotionScore emo)
        {
            if (MyModel.Series.Count > 0) MyModel.Series.RemoveAt(0);
            
            var seriesP1 = new PieSeries
            {
                StrokeThickness = 2.0,
                InsideLabelPosition = 0.6,
                AngleSpan = 360,
                StartAngle = 0,
                InsideLabelColor = OxyColors.Black,
                TextColor = OxyColors.DarkGray,
                Stroke = OxyColors.Gray
            };

            if (emo != null)
            {
                seriesP1.Slices.Add(new PieSlice("Anger", emo.scores.anger) { IsExploded = true, Fill = OxyColors.Red });
                seriesP1.Slices.Add(new PieSlice("Contempt", emo.scores.contempt) { IsExploded = true, Fill = OxyColors.MediumPurple });
                seriesP1.Slices.Add(new PieSlice("Disgust", emo.scores.disgust) { IsExploded = true, Fill = OxyColors.Yellow });
                seriesP1.Slices.Add(new PieSlice("Fear", emo.scores.fear) { IsExploded = true, Fill = OxyColors.Purple });
                seriesP1.Slices.Add(new PieSlice("Happiness", emo.scores.happiness) { IsExploded = true, Fill = OxyColors.LightGreen });
                seriesP1.Slices.Add(new PieSlice("Neutral", emo.scores.neutral) { IsExploded = true, Fill = OxyColors.SandyBrown });
                seriesP1.Slices.Add(new PieSlice("Sadness", emo.scores.sadness) { IsExploded = true, Fill = OxyColors.DimGray });
                seriesP1.Slices.Add(new PieSlice("Surprise", emo.scores.surprise) { IsExploded = true, Fill = OxyColors.Orange });
            }
            else
            {
                seriesP1.Slices.Add(new PieSlice("No Data Available", 1) { IsExploded = false, Fill = OxyColors.Gray, });
            }

            MyModel.Series.Add(seriesP1);
            MyModel.InvalidatePlot(true);
        }
    }

}
