using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using CamEmoOrc;
using Emotional.Models;

namespace Orchestrator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new VideoPlayerForm());

            StartExecution().Wait();
            //Console.Read();
        }

        private static async Task StartExecution()
        {
            var orc = new BasicOrchestrator(1);
            var exe = new VideoExecution
            {
                fileName = "",
                height = 100,
                width = 100
            };

            //await orc.Start(exe, null);
        }
    }
}
