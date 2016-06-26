using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

using Emotional.Models;

namespace DataStore
{
    /// <summary>
    /// Implementation of the Data layer for SQL databases
    /// </summary>
    public class SQLDataLayer : DataAccessLayerBase, IDataLayer
    {
        private string _connection = @"Server=tcp:emotiondb.database.windows.net,1433;Database=EmotionHack;User ID=emotionhack@emotiondb;Password=@NYTH!NG123;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";

        /// <summary>
        /// Sets up the connection to the SQL database
        /// </summary>
        /// <param name="databaseConnection"></param>
        public SQLDataLayer(string databaseConnection = null)
        {
            this.ConnectionString = _connection;
        }

        public async Task<int> GetExecutionContext(VideoExecution video)
        {
            string query = @"INSERT INTO [emo].[ExecutionInstance] (FileName, Width, Height) 
                               VALUES (@FileName, @Width, @Height);  
                               SELECT SCOPE_IDENTITY();";

            var result = await ExecuteScalarAsync<int>(query, false, 
                "@FileName", video.fileName, 
                "@Width", video.width, 
                "@Height", video.height);

            return result;
        }

        public async Task<bool> FinishExecution(OrderedDictionary scores, int executionId)
        {
            //end execution
            string query1 = @"UPDATE [emo].[ExecutionInstance] 
                               SET EndTime = GETUTCNOW()
                               WHERE Id = @ExecutionId;";

            await ExecuteCommandAsync(query1, false, "@ExecutionId", executionId);

            string query2 = @"INSERT INTO [emo].[EmotionScore] 
                                (ExecutionId, Anger, Contempt, Disgust, Fear, Happiness, Neutral, Sadness, Surprise, StartTime, EndTime) VALUES @Values";

            List<string> values = new List<string>();

            foreach (DictionaryEntry entry in scores)
            {
                string val = @"({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10})";
                EmotionScore score = entry.Value as EmotionScore;
                val = string.Format(val, score.scores.anger, score.scores.contempt, score.scores.disgust, score.scores.fear, 
                                         score.scores.happiness, score.scores.neutral, score.scores.sadness, score.scores.surprise, 
                                         score.startTime, score.endTime);

                values.Add(val);
            }

            if (values.Count == 0)
            {
                return false;
            }

            var valuesString = String.Join(",", values);

            await ExecuteCommandAsync(query2, false, "@Values", valuesString);

            return true;
        }

        public async Task<List<EmotionScore>> GetScoresByExecutionId(int executionId)
        {
            return null;
        }
    }
}
