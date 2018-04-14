using System;
using UnityEngine;

namespace Sentiment
{
    public struct EmotionAnalysis
    {
        public EmotionState SentimentState;
        public EmotionSelector EmotionSelector;

        public Emotion CurrentEmotion
        {
            get { return this.EmotionSelector.Select(this.SentimentState); }
        }
    }

    public struct EmotionSelector
    {
        float[] threshold;

        public EmotionSelector(float[] threshold)
        {
            this.threshold = null;
            if (threshold == null) { return; }
            if (threshold.Length != (int)Emotion.Count)
            {
                throw new ArgumentException("Must be null or length equal Emotion.Count", nameof(threshold));
            }

            this.threshold = new float[(int)Emotion.Count];
            for (Emotion e = Emotion.First; e < Emotion.Count; ++e)
            {
                this.threshold[(int)e] = Mathf.Clamp01(threshold[(int)e]);
            }
        }

        public float this[Emotion emotion]
        {
            get { return this.threshold != null ? this.threshold[(int)emotion] : 0; }
        }

        public Emotion Select(EmotionState state)
        {
            float highestConfidence = 0;
            Emotion selectedEmotion = Emotion.Count;
            for (Emotion e = Emotion.First; e < Emotion.Count; ++e)
            {
                if (state[e] <= this[e]) { continue; }
                if (state[e] <= highestConfidence) { continue; }

                highestConfidence = state[e];
                selectedEmotion = e;
            }

            return selectedEmotion;
        }
    }
}
