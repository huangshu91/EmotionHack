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

        //Task<List<EmotionScore>> GetScoresFilteredBy(int something);

        Task<List<List<EmotionScore>>> GetFullScoreHistory();

        Task<List<EmotionScore>> GetScoresByExecutionId(int executionId);

        Task WithDataLayerAsync(Func<SQLDataLayer, Task> protectedFunction);

        Task<T> WithDataLayerAsync<T>(Func<SQLDataLayer, Task<T>> protectedFunction);
    }
}
