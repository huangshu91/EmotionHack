using System;
using System.Data;
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
        }
    }
}
