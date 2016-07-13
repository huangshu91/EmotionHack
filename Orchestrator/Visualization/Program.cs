using Emotional.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Visualization
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 graph = new Form1(3);

            Scores s11 = new Scores();
            s11.anger = 1;

            Scores s12 = new Scores();
            s12.anger = 1.6;

            Scores s13 = new Scores();
            s13.happiness = 1;
            s13.anger = 2.1;

            Scores s21 = new Scores();
            s21.happiness = 1.4;
            s21.anger = 0;

            Scores s22 = new Scores();
            s22.happiness = 1.9;
            s22.anger = 2.3;

            Scores s23 = new Scores();
            s23.anger = 1;

            Scores s31 = new Scores();
            s31.anger = 1.6;

            Scores s32 = new Scores();
            s32.anger = 1.6;

            Scores s33 = new Scores();
            s33.anger = 1.6;
            Scores[,] testScores = new Scores [3,3];
            testScores[0, 0] = s11;
            testScores[0, 1] = s12;
            testScores[0, 2] = s13;
            testScores[1, 0] = s21;
            testScores[1, 1] = s22;
            testScores[1, 2] = s23;
            testScores[2, 0] = s31;
            testScores[2, 1] = s32;
            testScores[2, 2] = s33;

            graph.ShowAggregatedGraph(testScores);
            Application.Run(graph);
        }
    }
}
