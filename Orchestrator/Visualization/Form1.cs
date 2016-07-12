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
        private ArrayList emoScoreList = new ArrayList();
        private Scores currentScore = null;

        public Form1(int maxPointNumber)
        {
            InitializeComponent();
            m_maxPointCount = 0;
            m_maxPointCount = maxPointNumber;
            movingChart.Series.Clear();

            //Setting the boundaries for the x axis so it never autoresizes.
            movingChart.ChartAreas[0].AxisY.Minimum = -4;
            movingChart.ChartAreas[0].AxisY.Maximum = 4;
            movingChart.ChartAreas[0].AxisX.Minimum = 0;
            movingChart.ChartAreas[0].AxisX.Maximum = m_maxPointCount;
            Series series = this.movingChart.Series.Add("Aggregated Emotion");
            movingChart.Series["Aggregated Emotion"].ChartType =
                    SeriesChartType.FastLine;
            movingChart.Series["Aggregated Emotion"].Color = Color.Blue;
        }

        public void addEmoPoint(Scores s)
        {
            emoScoreList.Add(s);
            currentScore = s;
            ShowAggregatedGraph();
        }

        public  void ShowAggregatedGraph()
        {
            //foreach (Scores s in emoScoreList )
            //{
            //    movingChart.Series["Aggregated Emotion"].Points.AddY
            //                    (aggregateEmotionPoint(s));
            //}
            movingChart.Series["Aggregated Emotion"].Points.AddY
                                (aggregateEmotionPoint(currentScore));
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
    }
}
