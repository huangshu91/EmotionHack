using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emotional.Models
{
    public static class ModelUtility
    {
        public static double ProcessScorePositive(EmotionScore emo)
        {
            //weights
            double Weight_happiness = 1;
            double Weight_surprise = 0.9;
            double Weight_neutral = 0;
            double Weight_sadness = -1;
            double Weight_anger = -1;
            double Weight_disgust = 0;
            double Weight_fear = 0;
            double Weight_contempt = 0;

            double AggEmoScore = 0.0;

            //weighted scores
            double happiness = emo.scores.happiness * Weight_happiness;
            double surprise = emo.scores.surprise * Weight_surprise;
            double neutral = emo.scores.neutral * Weight_neutral;
            double sadness = emo.scores.sadness * Weight_sadness;
            double anger = emo.scores.anger * Weight_anger;
            double disgust = emo.scores.disgust * Weight_disgust;
            double contempt = emo.scores.contempt * Weight_contempt;
            double fear = emo.scores.fear * Weight_fear;

            double positiveEmo_max = Math.Max(happiness, Math.Max(surprise, neutral));
            double negativeEmo_min = Math.Min(sadness, Math.Min(anger, Math.Min(disgust, Math.Min(fear, contempt))));
            AggEmoScore = Math.Abs(positiveEmo_max) >= Math.Abs(negativeEmo_min) ? positiveEmo_max : negativeEmo_min;
            return AggEmoScore;
        }

        public static double ProcessScoreNegative(EmotionScore emo)
        {
            return -0.5 * ModelUtility.ProcessScorePositive(emo);
        }
    }
}
