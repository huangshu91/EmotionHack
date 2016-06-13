using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emotional.Utility;
using Emotional.Models;

namespace Emotional.Core
{
    public class EmotionClient
    {
        private EmoHttpClient client { get; set; }

        private OrderedDictionary history = new OrderedDictionary();
        private OrderedDictionary aggHistory = new OrderedDictionary();

        public EmotionClient()
        {
            client = new EmoHttpClient();
        }

        public async Task<EmotionScore> GetEmotion(Stream stream, DateTime time)
        {
            var scores = await client.GetEmotion(stream);
            history.Add(time, scores);
            return scores;
        }

        public async Task<EmotionScore> GetEmotion(MemoryStream stream, DateTime time)
        {
            var scores = await client.GetEmotion(stream);
            return scores;
        }
    }
}
