namespace SliderPlaybackVisualization
{
    using Emotional.Models;
    using OxyPlot;
    using OxyPlot.Axes;
    using System.Collections;
    using OxyPlot.Series;
    using System.Collections.Specialized;

    class TimelineModel
    {
        public PlotModel MyModel { get; set; }

        private LineSeries positive { get; set; }

        private LineSeries negative { get; set; }

        private OrderedDictionary History { get; set; }

        public TimelineModel()
        {
            this.MyModel = new PlotModel { Title = "LineGraph" };

            this.MyModel.Background = OxyColors.Transparent;//.White;
            this.MyModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = -1.5,
                Maximum = 1.5,
                TextColor = OxyColors.White,
                AxislineColor = OxyColors.White,
                TicklineColor = OxyColors.White,
            }
            );
            this.MyModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                TextColor = OxyColors.White,
                AxislineColor = OxyColors.White,
                TicklineColor = OxyColors.White,
            }
            );

            positive = new LineSeries
            {
                StrokeThickness = 2,
                CanTrackerInterpolatePoints = false,
                Title = "Value",
                Smooth = true
            };

            negative = new LineSeries
            {
                StrokeThickness = 2,
                CanTrackerInterpolatePoints = false,
                Title = "Value",
                Smooth = true,
                Color = OxyColors.Orchid
            };

            this.MyModel.Series.Add(negative);
            this.MyModel.Series.Add(positive);

            this.MyModel.IsLegendVisible = false;
        }

        public void SetHistory(OrderedDictionary history)
        {
            History = history;
            foreach (DictionaryEntry timeScore in history)
            {
                var timeStamp = (double)timeScore.Key;
                var score = (EmotionScore)timeScore.Value;
                var pos = ModelUtility.ProcessScorePositive(score);
                var neg = ModelUtility.ProcessScoreNegative(score);
                
                positive.Points.Add(new DataPoint(timeStamp, pos));
                negative.Points.Add(new DataPoint(timeStamp++, neg));
            }
        }
    }

}
