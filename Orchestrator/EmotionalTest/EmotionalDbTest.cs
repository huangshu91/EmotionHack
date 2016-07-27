using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
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
        //SqlConnection conn;

        public EmotionalDbTest()
        {
            ConnectionString = _connection;
        }

        [TestMethod]
        public async Task GetScoreHistory()
        {
            //IDataLayer dbAccess = new SQLDataLayer(_connection);
            //var result = await dbAccess.WithDataLayerAsync(async db => await db.GetFullScoreHistory());

            //Assert.AreNotEqual(result.Count, 0);
        }

        [TestMethod]
        public async Task StartStopExecution()
        {
            IDataLayer dbAccess = new SQLDataLayer(_connection);
            int vHeight = 200;
            int vWidth = 200;
            string vName = "unitTestName";

            VideoExecution ve = new VideoExecution()
            {
                Height = vHeight,
                Width = vWidth,
                FileName = vName
            };

            int executionId = await dbAccess.WithDataLayerAsync<int>(async db => await db.GetExecutionContext(ve));

            var query1 = @"SELECT * FROM [emo].[ExecutionInstance] WHERE Id = @exeId;";

            this.Open();
            using (var reader = await ExecuteReaderAsync(query1, false, "@exeId", executionId))
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
            this.Close();

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

            var finish = await dbAccess.WithDataLayerAsync<bool>(async db => await db.FinishExecution(od, executionId));

            Assert.AreEqual(finish, true);

            var query2 = @"SELECT * FROM [emo].[EmotionScore] WHERE ExecutionId = @exeId ORDER BY TimeStamp ASC;";

            this.Open();
            using (var reader = await this.ExecuteReaderAsync(query2, false, "@exeId", executionId))
            {
                if (reader.Read())
                {
                    var start = Convert.ToDateTime(reader["StartTime"]);
                    var end = Convert.ToDateTime(reader["EndTime"]);
                    var time = Convert.ToDateTime(reader["TimeStamp"]);

                    var angerScore = Convert.ToDouble(reader["Anger"]);

                    Assert.AreEqual(angerScore, scoreValue);
                }
            }
            this.Close();
        }
    }
}
