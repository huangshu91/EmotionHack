using Emotional.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamEmoOrc
{
    using RuntimeVisualization;

    /// <summary>
    /// Orchestrates the capturing of images from the Camera Client and obtaining scores from the emotional client
    /// </summary>
    public interface IOrchestrator
    {
        /// <summary>
        /// Starts the orchestration
        /// </summary>
        /// <param name="videoExecution">The video execution instance</param>
        /// <param name="realTimeVisualizer">The object that is showing the visualizations of the realtime scores</param>
        /// <returns>id of the current execution</returns>
        Task<int> Start(VideoExecution videoExecution, RuntimeWindow realTimeVisualizer);

        /// <summary>
        /// Finish the execution and upload scores.
        /// </summary>
        void FinishExecution();

        /// <summary>
        /// Gets the complete history of the video under exection
        /// </summary>
        /// <returns></returns>
        void ShowPostPlaybackVisualizations(VideoExecution videoExection);
    }
}
