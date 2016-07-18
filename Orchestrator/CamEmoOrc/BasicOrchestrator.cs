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

namespace CamEmoOrc
{
    public class BasicOrchestrator : IOrchestrator
    {
        private IEmotionClient _EmoClient;
        private AccessCamera _Camera;
        private TimeSpan _sampleRate;
        private bool _togglePlay;
        private Task _OrcInstance;

        public BasicOrchestrator(double samplingRate)
        {
            _sampleRate = TimeSpan.FromSeconds(samplingRate);
            _EmoClient = new EmotionClient();
            _Camera = new AccessCamera();
            _togglePlay = false;
        }

        public async Task<int> Start(VideoExecution videoExecution, object realTimeVisualizer)
        {
            return await Task.Factory.StartNew<int>(() => StartExecution(videoExecution, realTimeVisualizer));
        }

        private int StartExecution(VideoExecution videoExecution, object realTimeVisualizer)
        {
            int executionId = -1;
            try
            {
                _togglePlay = true;
                executionId = _EmoClient.BeginExecution(videoExecution).Result;
                _OrcInstance = Task.Factory.StartNew(() => { CaptureAndSend(); });
                //_OrcInstance.Start();
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

                if (pic != null && pic.Length > 0)
                {
                    EmotionScore emoScore = _EmoClient.GetEmotion(pic, DateTime.Now).Result;
                }
                /// DO SOMETHING WITH THE SCORE!!!
                Thread.Sleep(_sampleRate);
            }
            _Camera.cam_Stop();
        }

        public async Task<OrderedDictionary> Stop()
        {
            return await new Task<OrderedDictionary>(() => { return new OrderedDictionary(); });

        }
    }
}
