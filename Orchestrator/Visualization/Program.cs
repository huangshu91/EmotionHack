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
            Form1 graph = new Form1(10);

            Scores s1 = new Scores();
            s1.anger = 1;
            graph.addEmoPoint(s1);

            Scores s2 = new Scores();
            s2.anger = 2;
            graph.addEmoPoint(s2);

            Scores s3 = new Scores();
            s3.happiness = 1;
            graph.addEmoPoint(s3);

            Scores s4 = new Scores();
            s4.happiness = 1.4;
            graph.addEmoPoint(s4);

            Scores s5 = new Scores();
            s5.happiness = 1.9;
            graph.addEmoPoint(s5);

            Application.Run(graph);
        }
    }
}
