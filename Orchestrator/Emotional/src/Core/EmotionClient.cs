using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using DataStore;
using Emotional.Utility;
using Emotional.Models;

namespace Emotional.Core
{
    public class EmotionClient : IEmotionClient
    {
        private EmoHttpClient client { get; set; }
        private IDataLayer DbLayer { get; set; }

        public OrderedDictionary history { get; private set;  }
        public OrderedDictionary aggHistory { get; private set; }

        private int ExecutionId { get; set; }

        public EmotionClient()
        {
            client = new EmoHttpClient();
            DbLayer = new SQLDataLayer();
            history = new OrderedDictionary();
            aggHistory = new OrderedDictionary();
        }

        public async Task<int> BeginExecution(VideoExecution vid)
        {
            //ExecutionId = await DbLayer.GetExecutionContext(vid);

            ExecutionId = await DbLayer.WithDataLayerAsync<int>(async db => await db.GetExecutionContext(vid));
            //return 0;
            return ExecutionId;
        }

        public async Task FinishExecution()
        {
            //var result = await DbLayer.FinishExecution(history, ExecutionId);

            var result = await DbLayer.WithDataLayerAsync<bool>(async db => await db.FinishExecution(history, ExecutionId));

            if (!result)
            {
                LogWriter.Instance.WriteToLog(string.Format("was not able to finish execution for executionId {0}", ExecutionId));
            }
        }

        public async Task<EmotionScore> GetEmotion(Stream stream, double time)
        {
            DateTime start = DateTime.UtcNow;
            var scores = await client.GetEmotion(stream);
            scores.startTime = start;
            scores.endTime = DateTime.UtcNow;
            scores.executionId = ExecutionId;
            scores.timeStamp = time;

            history.Add(time, scores);
            return scores;
        }

        public async Task<EmotionScore> GetEmotion(MemoryStream stream, double time)
        {
            DateTime start = DateTime.UtcNow;
            var scores = await client.GetEmotion(stream);
            scores.startTime = start;
            scores.endTime = DateTime.UtcNow;
            scores.executionId = ExecutionId;
            scores.timeStamp = time;

            history.Add(time, scores);
            return scores;
        }

        public Task<EmotionScore> GetDummyEmotion(double time, MemoryStream stream = null)
        {
            double defVal = 0.5;
            Scores score = new Scores()
            {
                anger = defVal,
                contempt = defVal,
                disgust = defVal,
                fear = defVal,
                happiness = defVal,
                neutral = defVal,
                sadness = defVal,
                surprise = defVal,
            };

            EmotionScore res = new EmotionScore()
            {
                timeStamp = time,
                startTime = DateTime.UtcNow,
                endTime = DateTime.UtcNow,
                executionId = this.ExecutionId,
                scores = score,
            };

            return Task.FromResult<EmotionScore>(res);
        }
    }
}
