using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Data.SqlClient;

using Emotional.Models;

namespace DataStore
{
    /// <summary>
    /// Implementation of the Data layer for SQL databases
    /// </summary>
    public class SQLDataLayer : DataAccessLayerBase, IDataLayer
    {
        private string _connection = @"Server=tcp:emotiondb.database.windows.net,1433;Database=EmotionHackPPE;User ID=emotionhack@emotiondb;Password=@NYTH!NG123;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";

        /// <summary>
        /// Sets up the connection to the SQL database
        /// </summary>
        /// <param name="databaseConnection"></param>
        public SQLDataLayer(string databaseConnection = null)
        {
            this.ConnectionString = databaseConnection ?? _connection;
        }

        /// <summary>
        /// A helper method to be used when Retry is needed.
        /// </summary>
        /// <param name="protectedFunction">A method or lambda expression invoked which will be retried if SQL Exceptions occur</param>
        /// <returns>Task</returns>
        public async Task WithDataLayerAsync(Func<SQLDataLayer, Task> protectedFunction)
        {
            await this.WithDataLayerAsync<int>(async x =>
            {
                await protectedFunction(this);
                return 0;
            });
        }

        /// <summary>
        /// A helper method to be used when Retry is needed.
        /// </summary>
        /// <typeparam name="T">The result returned by WithDataLayer from the protected method provided</typeparam>
        /// <param name="protectedFunction">A method or lambda expression invoked which will be retried if SQL Exceptions occur</param>
        /// <returns>What the protected method returns</returns>
        public async Task<T> WithDataLayerAsync<T>(Func<SQLDataLayer, Task<T>> protectedFunction)
        {
            return await this.WithDataLayerAsync<T, SQLDataLayer>(protectedFunction);
        }

        public async Task<int> GetExecutionContext(VideoExecution video)
        {
            string query = @"INSERT INTO [emo].[ExecutionInstance] (StartTime, FileName, Width, Height) 
                               VALUES (GETUTCDATE(), @FileName, @Width, @Height);  
                               SELECT CONVERT(int, SCOPE_IDENTITY());";

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
                               SET EndTime = GETUTCDATE()
                               WHERE Id = @ExecutionId;";

            await ExecuteCommandAsync(query1, false, "@ExecutionId", executionId);

            string query2 = @"INSERT INTO [emo].[EmotionScore] 
                                (ExecutionId, Anger, Contempt, Disgust, Fear, Happiness, Neutral, 
                                 Sadness, Surprise, StartTime, EndTime, TimeStamp) VALUES {0}";

            List<string> values = new List<string>();

            foreach (DictionaryEntry entry in scores)
            {
                string val = @"({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, '{9}', '{10}', {11})";
                EmotionScore score = entry.Value as EmotionScore;
                val = string.Format(val, executionId, score.scores.anger, score.scores.contempt, score.scores.disgust, score.scores.fear, 
                                         score.scores.happiness, score.scores.neutral, score.scores.sadness, score.scores.surprise, 
                                         score.startTime, score.endTime, entry.Key);

                values.Add(val);
            }

            if (values.Count == 0)
            {
                return false;;
            }

            var valuesString = String.Join(",", values);
            query2 = string.Format(query2, valuesString);

            await ExecuteCommandAsync(query2, false);

            return true;
        }

        public async Task<List<EmotionScore>> GetScoresByExecutionId(int executionId)
        {
            string query = @"SELECT * FROM [emo].[EmotionScore] WHERE Id = @ExecutionId;";
            List<EmotionScore> results = new List<EmotionScore>();

            using (var reader = await this.ExecuteReaderAsync(query, false, "@ExecutionId", executionId))
            {
                while (reader.Read())
                {
                    EmotionScore score = new EmotionScore(reader);
                    results.Add(score);
                }
            }

            return results;
        }

        public async Task<List<EmotionScore>> GetScoresFilteredBy(int something)
        {
            throw new NotImplementedException();
        }

        public async Task<List<List<EmotionScore>>> GetFullScoreHistory()
        {
            string query = @"SELECT * FROM [emo].[EmotionScore] ORDER BY [TimeStamp], [ExecutionId];";

            List<List<EmotionScore>> results = new List<List<EmotionScore>>();
            List<EmotionScore> scoreExe = null;
            using (var reader = await this.ExecuteReaderAsync(query, false))
            {
                int prevExe = 0;
                while (reader.Read())
                {
                    EmotionScore score = new EmotionScore(reader);
                    if (score.executionId != prevExe)
                    {
                        if (prevExe != 0)
                        {
                            results.Add(scoreExe);
                        }

                        scoreExe = new List<EmotionScore>();
                        prevExe = score.executionId;
                    }
                    scoreExe.Add(score);
                }
            }

            return results;
        }
    }
}
