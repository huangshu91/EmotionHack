using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore
{
    /// <summary>
    /// Defines methods to query, insert, and modify data in the data store
    /// </summary>
    public interface IDataLayer
    {
        /// <summary>
        /// Query's data from the underlying data store
        /// </summary>
        /// <typeparam name="T">The Type of the data to return back</typeparam>
        /// <param name="query">The query to send to the store, to get the desired data</param>
        /// <returns>Data from the store, deserialized to type T</returns>
        T QueryData<T>(string query);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="insertQuery"></param>
        /// <returns></returns>
        bool InsertSingleDataInputThroughQuery(string insertQuery);
    }
}
