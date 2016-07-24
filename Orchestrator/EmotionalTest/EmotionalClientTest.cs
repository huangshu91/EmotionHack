using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Emotional.Core;

namespace EmotionalTest
{
    [TestClass]
    public class EmotionalClientTest
    {
        string TestInput = @"../../TestResources/happybaby_lowres.jpg";

        [TestMethod]
        public async Task GetScores()
        {
            FileStream file = new FileStream(TestInput, FileMode.Open);

            EmotionClient emoClient = new EmotionClient();
            var result = await emoClient.GetEmotion(file, 0);

            Assert.IsNotNull(result.scores); 
        }


    }
}
