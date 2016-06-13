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

        public EmotionClient()
        {
            client = new EmoHttpClient();
        }

        public async Task<EmotionScore> GetEmotion(Stream stream)
        {
            return await GetEmotion(stream);
        }

        public async Task<EmotionScore> GetEmotion(MemoryStream stream)
        {
            return await GetEmotion(stream);
        }
    }
}
