using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamEmoOrc
{
    /// <summary>
    /// Orchestrates the capturing of images from the Camera Client and obtaining scores from the emotional client
    /// </summary>
    public interface IOrchestrator
    {
        /// <summary>
        /// Starts the orchestration
        /// </summary>
        /// <param name="realTimeVisualizer">The object that is showing the visualizations of the realtime scores</param>
        /// <returns>id of the current execution</returns>
        Task<int> Start(object realTimeVisualizer);

        /// <summary>
        /// Stops the Orchestration
        /// </summary>
        /// <returns>The entire set of scores for the current execution</returns>
        Task<OrderedDictionary> Stop();
    }
}
