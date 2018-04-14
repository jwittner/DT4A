using System;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Sentiment
{
    public struct EmotionState
    {
        private byte[] source;
        private float[] confidence;

        public EmotionState(byte[] source, float[] confidence)
        {
            if( source == null ) { throw new ArgumentNullException(nameof(source)); }
            this.source = (byte[])source.Clone();

            this.confidence = null;

            if (confidence == null) { return; }
            if (confidence.Length != (int)Emotion.Count)
            {
                throw new ArgumentException("Must be null or length equal Emotion.Count", nameof(confidence));
            }

            this.confidence = new float[(int)Emotion.Count];
            for (Emotion e = Emotion.First; e < Emotion.Count; ++e)
            {
                this.confidence[(int)e] = Mathf.Clamp01(confidence[(int)e]);
            }
        }

        public float this[Emotion emotion]
        {
            get { return this.confidence != null ? this.confidence[(int)emotion] : 0; }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for (Emotion emotion = Emotion.First; emotion < Emotion.Count; ++emotion)
            {
                builder.AppendLine($"{emotion} = {this[emotion]}");
            }

            return builder.ToString();
        }
    }
}