using Emotional.Core;
using Emotional.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebCam;
using RuntimeVisualization;
using Visualization;
using SliderPlaybackVisualization;

namespace CamEmoOrc
{
    public class BasicOrchestrator : IOrchestrator
    {
        private IEmotionClient _EmoClient;
        private AccessCamera _Camera;
        private TimeSpan _sampleRate;
        private bool _togglePlay;
        private Task _OrcInstance;
        private RuntimeWindow _runtimeVisual;

        private double sampleTime = 0;

        public BasicOrchestrator(double samplingRate)
        {
            _sampleRate = TimeSpan.FromSeconds(samplingRate);
            _EmoClient = new EmotionClient();
            _Camera = new AccessCamera();
            _togglePlay = false;
        }

        public async Task<int> Start(VideoExecution videoExecution, RuntimeWindow realTimeVisualizer)
        {
            _runtimeVisual = realTimeVisualizer;
            sampleTime = 0;
            return await Task.Factory.StartNew<int>(() => StartExecution(videoExecution));
        }

        private int StartExecution(VideoExecution videoExecution)
        {
            int executionId = -1;
            try
            {
                _togglePlay = true;
                executionId = _EmoClient.BeginExecution(videoExecution).Result;
                _OrcInstance = Task.Factory.StartNew(() => { CaptureAndSend(); });
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to start the orchestration execution!", ex);
            }
            return executionId;
        }

        private void CaptureAndSend()
        {
            _Camera.cam_Start();
            while (_togglePlay)
            {
                MemoryStream pic = _Camera.cam_TakePic();
                sampleTime += _sampleRate.TotalMilliseconds;

                if (pic != null && pic.Length > 0)
                {
                    EmotionScore score = _EmoClient.GetEmotion(pic, sampleTime).Result;
                    //var score = _EmoClient.GetDummyEmotion(sampleTime, null).Result;

                    //Update scores to 0 value in case it is null. Doing this so that execution doesn't break
                    score.scores = score.scores ?? new Scores();

                    //use EmotionScore data to draw RuntimeVisualization
                    _runtimeVisual.UpdateData(score, pic);

                }

                Thread.Sleep(_sampleRate);
            }
            _Camera.cam_Stop();
        }

        public void ShowPostPlaybackVisualizations(VideoExecution videoExection)
        {
            Task.Factory.StartNew(() => { ShowFinalvisualizationTask(); });
        }

        private void ShowFinalvisualizationTask()
        {
            var result = _EmoClient.GetHistory().Result;

            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            Form1 form = new Form1(result.Count);
            form.ShowGraphs(result);
            form.ShowDialog();
        }

        public void FinishExecution()
        {
            _togglePlay = false;
            Task.Factory.StartNew(() => { _EmoClient.FinishExecution(); });
        }
    }
}
