using Emotional.Core;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

        public BasicOrchestrator(double samplingRate)
        {
            _sampleRate = TimeSpan.FromSeconds(samplingRate);
            _EmoClient = new EmotionClient();
            _Camera = new AccessCamera();
        }

        public int Start(object realTimeVisualizer)
        {
            try
            {
                var Video
                _EmoClient.BeginExecution(new )
            }
            catch
            {
                return 0;
            }
            return new Task<int>(() => { return 0; });
            //throw new NotImplementedException();
        }

        private Task Spin()
        {
            while(true)
            {
                Thread.Sleep()
            }
        }

        public Task<OrderedDictionary> Stop()
        {
            return new Task<OrderedDictionary>(() => { return new OrderedDictionary(); });
            //throw new NotImplementedException();
        }
    }
}
