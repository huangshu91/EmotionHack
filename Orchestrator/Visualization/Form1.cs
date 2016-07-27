using Emotional.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Visualization
{
    using DataStore;

    /// <summary>
    /// Definition of the dynamics graph(s) that reflects users' emotions.
    /// </summary>
    public partial class Form1 : Form
    {
        private int m_maxPointCount;
        //An arrayList of array of Scores that contains all emotion data for a certain file

        public Form1(int pointCount)
        {
            m_maxPointCount = pointCount;
            InitializeComponent();

            combinedChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            combinedChart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            combinedChart.BackColor = Color.Black;
            combinedChart.ChartAreas[0].BackColor = Color.Black;

            initializeChart(angerChart, "Anger", Color.Red);
            initializeChart(happinessChart, "Happiness", Color.LightGreen);
            initializeChart(surpriseChart, "Surprise", Color.Orange);
            initializeChart(sadnessChart, "Sadness", Color.DimGray);
            initializeChart(contemptChart, "Contempt", Color.MediumPurple);
            initializeChart(neutralChart, "Neutral", Color.SandyBrown);
            initializeChart(disgustChart, "Disgust", Color.Yellow);
            initializeChart(fearChart, "Fear", Color.Purple);

            combinedChart.Series.Clear();
            combinedChart.ForeColor = Color.White;
            combinedChart.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
            combinedChart.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;

            combinedChart.ChartAreas[0].AxisX.Minimum = 1;
            combinedChart.ChartAreas[0].AxisX.Maximum = m_maxPointCount;

            var chartArea = combinedChart.ChartAreas[0];

            // set view range to [0,max]
            chartArea.AxisX.Minimum = 0;
            chartArea.AxisX.Maximum = m_maxPointCount;

            // enable autoscroll
            chartArea.CursorX.AutoScroll = true;

            // let's zoom to [0,blockSize] 
            int blockSize = 20;
            chartArea.AxisX.ScaleView.Zoomable = true;
            chartArea.AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
            int position = 0;
            chartArea.AxisX.ScaleView.Zoom(position, blockSize);

            // disable zoom-reset button (only scrollbar's arrows are available)
            chartArea.AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;

            // set scrollbar small change to blockSize 
            chartArea.AxisX.ScaleView.SmallScrollSize = blockSize;
            chartArea.AxisX.ScrollBar.ButtonColor = Color.Gray;

            chartArea.AxisX.LineColor = Color.White;
            chartArea.AxisY.LineColor = Color.White;
            chartArea.AxisX.TitleForeColor = Color.White;
            chartArea.AxisY.TitleForeColor = Color.White;
            chartArea.AxisX.InterlacedColor = Color.White;
            chartArea.AxisY.InterlacedColor = Color.White;

            Series anger = combinedChart.Series.Add("Anger");
            combinedChart.Series["Anger"].ChartType =
                    SeriesChartType.StackedArea;
            combinedChart.Series["Anger"].Color = Color.Red;
            Series happiness = combinedChart.Series.Add("Happiness");
            combinedChart.Series["Happiness"].ChartType =
                    SeriesChartType.StackedArea;
            combinedChart.Series["Happiness"].Color = Color.LightGreen;
            Series surprise = combinedChart.Series.Add("Surprise");
            combinedChart.Series["Surprise"].ChartType =
                    SeriesChartType.StackedArea;
            combinedChart.Series["Surprise"].Color = Color.Orange;
            Series sadness = combinedChart.Series.Add("Sadness");
            combinedChart.Series["Sadness"].ChartType =
                    SeriesChartType.StackedArea;
            combinedChart.Series["Sadness"].Color = Color.DimGray;
            Series contempt = combinedChart.Series.Add("Contempt");
            combinedChart.Series["Contempt"].ChartType =
                    SeriesChartType.StackedArea;
            combinedChart.Series["Contempt"].Color = Color.MediumPurple;
            Series neutral = combinedChart.Series.Add("Neutral");
            combinedChart.Series["Neutral"].ChartType =
                    SeriesChartType.StackedArea;
            combinedChart.Series["Neutral"].Color = Color.SandyBrown;
            Series disgust = combinedChart.Series.Add("Disgust");
            combinedChart.Series["Disgust"].ChartType =
                    SeriesChartType.StackedArea;
            combinedChart.Series["Disgust"].Color = Color.Yellow;
            Series fear = combinedChart.Series.Add("Fear");
            combinedChart.Series["Fear"].ChartType =
                    SeriesChartType.StackedArea;
            combinedChart.Series["Fear"].Color = Color.RosyBrown;
        }

        public void ShowGraphs(List<List<EmotionScore>> scores)
        {
            double aveAnger = 0, aveContempt = 0, aveDisgust = 0, aveFear = 0,
                aveHappiness = 0, aveNeutral = 0, aveSadness = 0, aveSurprise = 0;
            foreach (List<EmotionScore> scorePerTime in scores)
            {

                aveAnger = 0;
                aveContempt = 0;
                aveDisgust = 0;
                aveFear = 0;
                aveHappiness = 0;
                aveNeutral = 0;
                aveSadness = 0;
                aveSurprise = 0;
                foreach (EmotionScore userEScore in scorePerTime)
                {
                    Scores userScore = userEScore.scores;
                    aveAnger += userScore.anger;
                    aveContempt += userScore.contempt;
                    aveDisgust += userScore.disgust;
                    aveFear += userScore.fear;
                    aveHappiness += userScore.happiness;
                    aveNeutral += userScore.neutral;
                    aveSadness += userScore.sadness;
                    aveSurprise += userScore.surprise;
                }
                aveAnger /= scorePerTime.Count;
                aveContempt /= scorePerTime.Count;
                aveDisgust /= scorePerTime.Count;
                aveFear /= scorePerTime.Count;
                aveHappiness /= scorePerTime.Count;
                aveNeutral /= scorePerTime.Count;
                aveSadness /= scorePerTime.Count;
                aveSurprise /= scorePerTime.Count;
                combinedChart.Series["Anger"].Points.AddY(aveAnger);
                combinedChart.Series["Happiness"].Points.AddY(aveHappiness);
                combinedChart.Series["Surprise"].Points.AddY(aveSurprise);
                combinedChart.Series["Sadness"].Points.AddY(aveSadness);
                combinedChart.Series["Contempt"].Points.AddY(aveContempt);
                combinedChart.Series["Neutral"].Points.AddY(aveNeutral);
                combinedChart.Series["Disgust"].Points.AddY(aveDisgust);
                combinedChart.Series["Fear"].Points.AddY(aveFear);

                angerChart.Series["Anger"].Points.AddY(aveAnger);
                happinessChart.Series["Happiness"].Points.AddY(aveHappiness);
                surpriseChart.Series["Surprise"].Points.AddY(aveSurprise);
                sadnessChart.Series["Sadness"].Points.AddY(aveSadness);
                contemptChart.Series["Contempt"].Points.AddY(aveContempt);
                neutralChart.Series["Neutral"].Points.AddY(aveNeutral);
                disgustChart.Series["Disgust"].Points.AddY(aveDisgust);
                fearChart.Series["Fear"].Points.AddY(aveFear);
            }
        }

        private double aggregateEmotionPoint(Scores scores)
        {
            double anger = scores.anger;
            double contempt = scores.contempt;
            double disgust = scores.disgust;
            double fear = scores.fear;
            double happiness = scores.happiness;
            double neutral = scores.neutral;
            double sadness = scores.sadness;
            double surprise = scores.surprise;
            double aggregatedEmotion = (-1) * anger + (-1) * contempt + (-1) * disgust + (-1) * fear
                + happiness + 0 * neutral + (-0.5) * sadness + (0) * surprise;
            return aggregatedEmotion;
        }

        private double aggregateEmotionPoint(double anger, double contempt, double disgust, double fear, double happiness,
            double neutral, double sadness, double surprise)
        {
            double aggregatedEmotion = (-1) * anger + (-1) * contempt + (-1) * disgust + (-1) * fear
                + happiness + 0 * neutral + (-0.5) * sadness + (0) * surprise;
            return aggregatedEmotion;
        }

        private void initializeChart(Chart chart, String seriesName, Color color)
        {
            chart.BackColor = System.Drawing.Color.Black;
            chart.Series.Clear();
            chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
            chart.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;

            chart.ChartAreas[0].AxisX.Minimum = 1;
            chart.ChartAreas[0].AxisX.Maximum = m_maxPointCount;

            Series series = chart.Series.Add(seriesName);
            chart.Series[seriesName].ChartType =
                    SeriesChartType.FastLine;
            chart.Series[seriesName].Color = color;
            chart.ChartAreas[0].BackColor = Color.Black;

            var chartArea = chart.ChartAreas[series.ChartArea];

            // set view range to [0,max]
            chartArea.AxisX.Minimum = 0;
            chartArea.AxisX.Maximum = m_maxPointCount;

            // enable autoscroll
            chartArea.CursorX.AutoScroll = true;

            // let's zoom to [0,blockSize] 
            int blockSize = 20;
            chartArea.AxisX.ScaleView.Zoomable = true;
            chartArea.AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
            int position = 0;
            chartArea.AxisX.ScaleView.Zoom(position, blockSize);

            // disable zoom-reset button (only scrollbar's arrows are available)
            chartArea.AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;

            // set scrollbar small change to blockSize 
            chartArea.AxisX.ScaleView.SmallScrollSize = blockSize;
            chartArea.AxisX.LineColor = Color.White;
            chartArea.AxisY.LineColor = Color.White;

            chartArea.AxisX.ScrollBar.ButtonColor = Color.Gray;
        }

        private void Aggregated_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
