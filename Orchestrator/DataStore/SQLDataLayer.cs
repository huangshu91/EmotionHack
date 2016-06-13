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
    class SQLDataLayer : IDataLayer
    {
        /// <summary>
        /// Sets up the connection to the SQL database
        /// </summary>
        /// <param name="databaseConnection"></param>
        public SQLDataLayer(string databaseConnection)
        {
            throw new NotImplementedException();
        }

        public bool InsertSingleDataInputThroughQuery(string insertQuery)
        {
            throw new NotImplementedException();
        }

        public T QueryData<T>(string query)
        {
            throw new NotImplementedException();
        }
    }
}
