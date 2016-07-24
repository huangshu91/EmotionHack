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

        public Form1(int maxPointNumber)
        {
            InitializeComponent();
            m_maxPointCount = maxPointNumber;
            initializeChart(combinedChart, "Aggregated Emotion", Color.Blue);
            initializeChart(angerChart, "Anger", Color.Red);
            initializeChart(happinessChart, "Happiness", Color.Green);
            initializeChart(surpriseChart, "Surprise", Color.Orange);
            initializeChart(sadnessChart, "Sadness", Color.DimGray);
            initializeChart(contemptChart, "Contempt", Color.MediumPurple);
            initializeChart(neutralChart, "Neutral", Color.Brown);
            initializeChart(disgustChart, "Disgust", Color.Yellow);
            initializeChart(fearChart, "Fear", Color.RosyBrown);

            Series anger = combinedChart.Series.Add("Anger");
            combinedChart.Series["Anger"].ChartType =
                    SeriesChartType.FastLine;
            combinedChart.Series["Anger"].Color = Color.Red;
            Series happiness = combinedChart.Series.Add("Happiness");
            combinedChart.Series["Happiness"].ChartType =
                    SeriesChartType.FastLine;
            combinedChart.Series["Happiness"].Color = Color.Green;
            Series surprise = combinedChart.Series.Add("Surprise");
            combinedChart.Series["Surprise"].ChartType =
                    SeriesChartType.FastLine;
            combinedChart.Series["Surprise"].Color = Color.Orange;
            Series sadness = combinedChart.Series.Add("Sadness");
            combinedChart.Series["Sadness"].ChartType =
                    SeriesChartType.FastLine;
            combinedChart.Series["Sadness"].Color = Color.DimGray;
            Series contempt = combinedChart.Series.Add("Contempt");
            combinedChart.Series["Contempt"].ChartType =
                    SeriesChartType.FastLine;
            combinedChart.Series["Contempt"].Color = Color.MediumPurple;
            Series neutral = combinedChart.Series.Add("Neutral");
            combinedChart.Series["Neutral"].ChartType =
                    SeriesChartType.FastLine;
            combinedChart.Series["Neutral"].Color = Color.Brown;
            Series disgust = combinedChart.Series.Add("Disgust");
            combinedChart.Series["Disgust"].ChartType =
                    SeriesChartType.FastLine;
            combinedChart.Series["Disgust"].Color = Color.Yellow;
            Series fear = combinedChart.Series.Add("Fear");
            combinedChart.Series["Fear"].ChartType =
                    SeriesChartType.FastLine;
            combinedChart.Series["Fear"].Color = Color.RosyBrown;
        }

        public async Task<List<List<EmotionScore>>> GetScores()
        {
            using (var dbAccess = new SQLDataLayer())
            {
                return await dbAccess.WithDataLayerAsync(async db => await db.GetFullScoreHistory());
            }
        }

        public void ShowGraphs()
        {
            //Scores[,] emoScoresAll = emoScoresList;
            double aveAnger = 0, aveContempt = 0, aveDisgust = 0, aveFear = 0,
                aveHappiness = 0, aveNeutral = 0, aveSadness = 0, aveSurprise = 0;

            List<List<EmotionScore>> scores = GetScores().Result;

            foreach (List<EmotionScore> scorePerTime in scores)
            {
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
                combinedChart.Series["Aggregated Emotion"].Points.AddY
                    (aggregateEmotionPoint(aveAnger, aveContempt, aveDisgust,
                    aveFear, aveHappiness, aveNeutral, aveSadness, aveSurprise));
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
            chart.Series.Clear();

            //Setting the boundaries for the x axis so it never autoresizes.
            //TODO: will change the -4 and 4 according to emotion API's return values.
            chart.ChartAreas[0].AxisY.Minimum = -4;
            chart.ChartAreas[0].AxisY.Maximum = 4;
            chart.ChartAreas[0].AxisX.Minimum = 1;
            chart.ChartAreas[0].AxisX.Maximum = m_maxPointCount;

            Series series = chart.Series.Add(seriesName);
            chart.Series[seriesName].ChartType =
                    SeriesChartType.FastLine;
            chart.Series[seriesName].Color = color;
        }

        private void Aggregated_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
