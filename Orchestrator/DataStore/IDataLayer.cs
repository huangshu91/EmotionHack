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
        Task<long> SetExecutionContext();

        Task<bool> InsertApiLatencyTelemetry(DateTime timestamp);
    }
}
