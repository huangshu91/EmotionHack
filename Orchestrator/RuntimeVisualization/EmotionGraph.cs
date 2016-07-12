namespace RuntimeVisualization
{
    using System;
    using System.Timers;

    using OxyPlot;
    using OxyPlot.Series;

    public class EmotionGraph
    {
        public EmotionGraph()
        {
            this.MyModel = new PlotModel { Title = "Example 1" };

            Line = new LineSeries
            {
                StrokeThickness = 2,
                CanTrackerInterpolatePoints = false,
                Title = "Value",
                Smooth = false
            };

            this.MyModel.Series.Add(Line);
            this.MyModel.Axes.Clear();
            var t = this.MyModel.Axes.Count;

            this.MyModel.IsLegendVisible = false;

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

            MyModel.InvalidatePlot(true);
        }

        public int testx = 0;
        public int testy = 1;
        public LineSeries Line { get; set; }
        public PlotModel MyModel { get; private set; }
    }
}
