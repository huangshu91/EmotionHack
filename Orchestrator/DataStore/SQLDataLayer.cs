using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emotional.Models;

namespace DataStore
{
    /// <summary>
    /// Implementation of the Data layer for SQL databases
    /// </summary>
    public class SQLDataLayer : IDataLayer
    {
        private string connection = @"Server=tcp:emotiondb.database.windows.net,1433;Database=EmotionHack;User ID=emotionhack@emotiondb;Password=@NYTH!NG123;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
        
        /// <summary>
        /// Sets up the connection to the SQL database
        /// </summary>
        /// <param name="databaseConnection"></param>
        public SQLDataLayer(string databaseConnection = null)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetExecutionContext(VideoExecution video)
        {
            string qstring = @"INSERT INTO [emo].[ExecutionInstance] (FileName, Width, Height) VALUES ('{0}', {1}, {2})";
            qstring = string.Format(qstring, video.fileName, video.width, video.height);

            var result = await QueryData<int>(qstring);

            return result;
        }

        private async Task<bool> InsertSingleDataInputThroughQuery(string insertQuery)
        {
            throw new NotImplementedException();
        }

        private async Task<T> QueryData<T>(string query)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertScores(OrderedDictionary scores, int executionId)
        {
            //end execution
            string qstring = @"UPDATE [emo].[ExecutionInstance] SET EndTime = GETUTCNOW() WHERE Id = {0};";

            string query = string.Format(qstring, executionId);

            //bulk insert all the scores
            throw new NotImplementedException();
        }
    }
}
