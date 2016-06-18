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
    /// Defines methods to query, insert, and modify data in the data store
    /// </summary>
    public interface IDataLayer
    {
        Task<int> GetExecutionContext(VideoExecution video);

        Task<bool> FinishExecution(OrderedDictionary scores, int executionId);
    }
}
