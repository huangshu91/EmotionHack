using System;
using System.Data;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using DataStore;
using Emotional.Models;

namespace EmotionalTest
{
    [TestClass]
    public class EmotionalDbTest : DataAccessLayerBase
    {
        private string _connection = @"Server=tcp:emotiondb.database.windows.net,1433;Database=EmotionHackPPE;User ID=emotionhack@emotiondb;Password=@NYTH!NG123;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";

        public EmotionalDbTest()
        {
            this.ConnectionString = _connection;
        }

        [TestMethod]
        public async Task StartStopExecution()
        {
            IDataLayer db = new SQLDataLayer();
            int vHeight = 200;
            int vWidth = 200;
            string vName = "unitTestName";

            VideoExecution ve = new VideoExecution()
            {
                height = vHeight,
                width = vWidth,
                fileName = vName
            };

            int executionId = await db.GetExecutionContext(ve);

            var query1 = @"SELECT * FROM [emo].[ExecutionInstance] WHERE Id = @exeId;";

            using (var reader = await this.ExecuteReaderAsync(query1, false, "@exeId", executionId))
            {
                if (reader.Read())
                {
                    var fname = reader["Filename"] as string;
                    var fwidth = Convert.ToInt32(reader["Width"]);
                    var fheight = Convert.ToInt32(reader["Height"]);

                    Assert.AreEqual(fname, vName);
                    Assert.AreEqual(vHeight, fheight);
                    Assert.AreEqual(vWidth, fwidth);
                }
            }

            double scoreValue = 0.5;
            DateTime now = DateTime.UtcNow;

            EmotionScore score = new EmotionScore()
            {
                startTime = now,
                endTime = now,
                scores = new Scores()
                {
                    anger = scoreValue,
                    contempt = scoreValue,
                    disgust = scoreValue,
                    fear = scoreValue,
                    happiness = scoreValue,
                    neutral = scoreValue,
                    sadness = scoreValue,
                    surprise = scoreValue
                },
                executionId = executionId,
            };

            OrderedDictionary od = new OrderedDictionary();
            od.Add(now, score);

            var finish = await db.FinishExecution(od, executionId);

            Assert.AreEqual(finish, true);

            var query2 = @"SELECT *, COUNT(*) as NumRows FROM [emo].[EmotionScore] WHERE ExecutionId = @exeId ORDER BY TimeStamp ASC;";

            using (var reader = await this.ExecuteReaderAsync(query2, false, "@exeId", executionId))
            {
                if (reader.Read())
                {
                    var numRows = Convert.ToInt32(reader["NumRows"]);
                    Assert.AreEqual(numRows, 1);

                    var start = Convert.ToDateTime(reader["StartTime"]);
                    var end = Convert.ToDateTime(reader["EndTime"]);
                    var time = Convert.ToDateTime(reader["TimeStamp"]);

                    Assert.AreEqual(start, now);
                    Assert.AreEqual(end, now);
                    Assert.AreEqual(time, now);

                    var angerScore = Convert.ToDouble(reader["Anger"]);

                    Assert.AreEqual(angerScore, scoreValue);
                }
            }
        }
    }
}
