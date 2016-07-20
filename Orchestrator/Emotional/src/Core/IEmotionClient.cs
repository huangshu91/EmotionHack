using System;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;
using Emotional.Models;

namespace Emotional.Core
{
    public interface IEmotionClient
    {
        /// <summary>
        /// Takes in Info about a video and returns an execution Id.
        /// </summary>
        /// <param name="vid"></param>
        /// <returns></returns>
        Task<int> BeginExecution(VideoExecution vid);
        /// <summary>
        /// Finishes the current exeuction
        /// </summary>
        /// <returns></returns>
        Task FinishExecution();
        /// <summary>
        /// Passes across a snapshot of the image from the web cam to the Emotional API
        /// </summary>
        /// <param name="stream">stream for the snapshot from the web cam</param>
        /// <param name="time">time at which the snapshot was taken</param>
        /// <returns></returns>
        Task<EmotionScore> GetEmotion(Stream stream, DateTime time);
        /// <summary>
        /// Passes across a snapshot of the image from the web cam to the Emotional API
        /// </summary>
        /// <param name="stream">stream for the snapshot from the web cam</param>
        /// <param name="time">time at which the snapshot was taken</param>
        /// <returns></returns>
        Task<EmotionScore> GetEmotion(MemoryStream stream, DateTime time);
        /// <summary>
        /// Dummy call to the Emotional API which fakes the snapshot of image from the web cam
        /// </summary>
        /// <param name="time"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task<EmotionScore> GetDummyEmotion(DateTime time, MemoryStream stream = null);
    }
}