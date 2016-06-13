using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore
{
    /// <summary>
    /// Implementation of the Data layer for SQL databases
    /// </summary>
    public class SQLDataLayer : IDataLayer
    {
        private string connection = @"Server=tcp:emotiondb.database.windows.net,1433;Database=EmotionHack;User ID=emotionhack@emotiondb;Password=@NYTH!NG123;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";

        public long executionId = 0;

        /// <summary>
        /// Sets up the connection to the SQL database
        /// </summary>
        /// <param name="databaseConnection"></param>
        public SQLDataLayer(string databaseConnection = null)
        {
            throw new NotImplementedException();
        }

        public async Task<long> SetExecutionContext()
        {
            throw new NotImplementedException();
        }

        private async Task<bool> InsertSingleDataInputThroughQuery(string insertQuery)
        {
            throw new NotImplementedException();
        }

        private async Task<T> QueryData<T>(string query)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertApiLatencyTelemetry(DateTime timestamp)
        {
            throw new NotImplementedException();
        }
    }
}
