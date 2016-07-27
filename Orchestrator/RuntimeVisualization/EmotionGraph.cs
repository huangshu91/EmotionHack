namespace RuntimeVisualization
{
    using System;
    using System.Timers;

    using OxyPlot;
    using OxyPlot.Series;
    using OxyPlot.Axes;

    using Emotional.Models;

    public class EmotionGraph
    {
        private int _displaySpan = 10;


        public int testx = 0;
        public int testy = 1;
        public LineSeries positive { get; set; }

        //public LineSeries negative { get; set; }

        public PlotModel MyModel { get; private set; }

        public EmotionGraph()
        {
            this.MyModel = new PlotModel { Title = "LineGraph" };
            //this.PieModel = new PlotModel { Title = "PieGraph" };

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

            //negative = new LineSeries
            //{
            //    StrokeThickness = 2,
            //    CanTrackerInterpolatePoints = false,
            //    Title = "Value",
            //    Smooth = true,
            //    Color = OxyColors.Orchid
            //};

            //this.MyModel.Series.Add(negative);
            this.MyModel.Series.Add(positive);

            this.MyModel.IsLegendVisible = false;

            //Timer myTimer = new Timer();
            //myTimer.Elapsed += new ElapsedEventHandler(UpdateModel);
            //myTimer.Interval = 1000; // 1000 ms is one second
            //myTimer.Start();


        }

        public void UpdateModel(object source, ElapsedEventArgs e)
        {
            double score = testy;
            positive.Points.Add(new DataPoint(testx, score));
            //negative.Points.Add(new DataPoint(testx++, -0.5 * score));

            testy *= -1;
            if (testx >= _displaySpan)
            {
                positive.Points.RemoveAt(0);
                //negative.Points.RemoveAt(0);
            }
            
            this.MyModel.InvalidatePlot(true);
        }
        
        public void UpdateScore(EmotionScore emo)
        {
            //display the score in some way            
            //Line.Points.Add(new DataPoint(testx++, emo.scores.happiness));
            double pos = ModelUtility.ProcessScorePositive(emo);
            //double neg = ModelUtility.ProcessScoreNegative(emo);

            positive.Points.Add(new DataPoint(testx, pos));
            //negative.Points.Add(new DataPoint(testx++, neg));

            if (testx >= _displaySpan)
            {
                positive.Points.RemoveAt(0);
                //negative.Points.RemoveAt(0);
            }

            MyModel.InvalidatePlot(true);
        }
    }
}
