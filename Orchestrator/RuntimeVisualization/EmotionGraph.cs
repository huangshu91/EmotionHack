namespace RuntimeVisualization
{
    using System;
    using System.Timers;

    using OxyPlot;
    using OxyPlot.Series;

    using Emotional.Models;

    public class EmotionGraph
    {
        private int _displaySpan = 10;
        public EmotionGraph()
        {
            this.MyModel = new PlotModel { Title = "Example 1" };
            //this.MyModel.PlotAreaBackground = BackgroundBaseColor.ToOxyColor();
            this.MyModel.Background = OxyColors.White;

            Line = new LineSeries
            {
                StrokeThickness = 2,
                CanTrackerInterpolatePoints = false,
                Title = "Value",
                Smooth = true
            };

            this.MyModel.Series.Add(Line);
            this.MyModel.Axes.Clear();
            var t = this.MyModel.Axes.Count;

            //this.MyModel.IsLegendVisible = false;
            this.MyModel.IsLegendVisible = true;

            Timer myTimer = new Timer();
            myTimer.Elapsed += new ElapsedEventHandler(UpdateModel);
            myTimer.Interval = 1000; // 1000 ms is one second
            myTimer.Start();
        }

        public void UpdateModel(object source, ElapsedEventArgs e)
        {
            //MyModel.Series.Clear();
            Line.Points.Add(new DataPoint(testx++, testy));
            testy *= -1;
            if (testx >= _displaySpan) {
                Line.Points.RemoveAt(0);
            }
            MyModel.InvalidatePlot(true);
        }

        public void UpdateScore(EmotionScore emo)
        {
            //display the score in some way            
            Line.Points.Add(new DataPoint(testx++, testy));
            testy *= -1;

            MyModel.InvalidatePlot(true);
        }

        public int testx = 0;
        public int testy = 1;
        public LineSeries Line { get; set; }
        public PlotModel MyModel { get; private set; }
    }
}
