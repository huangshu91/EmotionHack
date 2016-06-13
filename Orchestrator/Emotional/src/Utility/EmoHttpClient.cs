using System;
using System.IO;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
using System.Diagnostics;

using Emotional.Models;
using DataStore;

namespace Emotional.Utility
{
    class EmoHttpClient
    {
        private HttpClient client { get; set; }

        private const string uri = @"https://api.projectoxford.ai/emotion/v1.0/recognize?";

        private IDataLayer dbLayer = new SQLDataLayer();

        private Stopwatch stopWatch = new Stopwatch();
        
        public EmoHttpClient()
        {
            client = new HttpClient();

            //string apiKey = ConfigurationManager.AppSettings["apiKey"];
            string apiKey = @"f070bcbcc20048eeacd8ad9aa4d11ff6";
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);
        }

        public async Task<EmotionScore> GetEmotion(Stream input)
        {
            byte[] byteIn;
            HttpResponseMessage response;

            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                byteIn = ms.ToArray();
            }

            using (var content = new ByteArrayContent(byteIn))
            {
                content.Headers.Add("Content-Type", "application/octet-stream");
                stopWatch.Start();
                response = await client.PostAsync(uri, content);
                stopWatch.Stop();
            }

            var latency = stopWatch.ElapsedMilliseconds;

            var stringRes = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<EmotionScore>>(stringRes);

            return result[0];
        }

        public async Task<EmotionScore> GetEmotion(MemoryStream input)
        {
            var byteIn = input.ToArray();
            HttpResponseMessage response;
            
            using (var content = new ByteArrayContent(byteIn))
            {
                content.Headers.Add("Content-Type", "application/octet-stream");
                response = await client.PostAsync(uri, content);
            }

            var stringRes = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<EmotionScore>>(stringRes);

            return result[0];
        }

    }
}
