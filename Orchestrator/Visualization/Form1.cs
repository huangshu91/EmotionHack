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
            combinedChart.Series.Clear();
            angerChart.Series.Clear();

            //Setting the boundaries for the x axis so it never autoresizes.
            //TODO: will change the -4 and 4 according to emotion API's return values.
            combinedChart.ChartAreas[0].AxisY.Minimum = -4;
            combinedChart.ChartAreas[0].AxisY.Maximum = 4;
            combinedChart.ChartAreas[0].AxisX.Minimum = 1;
            combinedChart.ChartAreas[0].AxisX.Maximum = m_maxPointCount;

            angerChart.ChartAreas[0].AxisY.Minimum = -4;
            angerChart.ChartAreas[0].AxisY.Maximum = 4;
            angerChart.ChartAreas[0].AxisX.Minimum = 1;
            angerChart.ChartAreas[0].AxisX.Maximum = m_maxPointCount;

            Series series = this.combinedChart.Series.Add("Aggregated Emotion");
            combinedChart.Series["Aggregated Emotion"].ChartType =
                    SeriesChartType.FastLine;
            combinedChart.Series["Aggregated Emotion"].Color = Color.Blue;

            Series angerSeries = this.angerChart.Series.Add("Anger");
            angerChart.Series["Anger"].ChartType = SeriesChartType.FastLine;
            angerChart.Series["Anger"].Color = Color.Red;
        }

        public void ShowAggregatedGraph(Scores[,] emoScoresList)
        {
            Scores [,] emoScoresAll = emoScoresList;
            double aveAnger = 0, aveContempt = 0, aveDisgust = 0, aveFear = 0,
                aveHappiness = 0, aveNeutral = 0, aveSadness = 0, aveSurprise = 0;
            for (int j = 0; j < emoScoresAll.GetLength(1); j++)
            {
                for (int i = 0; i < emoScoresAll.GetLength(0); i++)
                {
                    aveAnger += emoScoresAll[i, j].anger;
                    aveContempt += emoScoresAll[i, j].contempt;
                    aveDisgust += emoScoresAll[i, j].disgust;
                    aveFear += emoScoresAll[i, j].fear;
                    aveHappiness += emoScoresAll[i, j].happiness;
                    aveNeutral += emoScoresAll[i, j].neutral;
                    aveSadness += emoScoresAll[i, j].sadness;
                    aveSurprise += emoScoresAll[i, j].surprise;
                }
                aveAnger /= emoScoresAll.GetLength(0);
                aveContempt /= emoScoresAll.GetLength(0);
                aveDisgust /= emoScoresAll.GetLength(0);
                aveFear /= emoScoresAll.GetLength(0);
                aveHappiness /= emoScoresAll.GetLength(0);
                aveNeutral /= emoScoresAll.GetLength(0);
                aveSadness /= emoScoresAll.GetLength(0);
                aveSurprise /= emoScoresAll.GetLength(0);
                combinedChart.Series["Aggregated Emotion"].Points.AddY
                    (aggregateEmotionPoint(aveAnger, aveContempt, aveDisgust,
                    aveFear, aveHappiness, aveNeutral, aveSadness, aveSurprise));
                angerChart.Series["Anger"].Points.AddY(aveAnger);
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
    }
}
