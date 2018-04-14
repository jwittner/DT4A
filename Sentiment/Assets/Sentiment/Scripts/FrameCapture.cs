using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

namespace Sentiment
{
    public class FrameCapture : MonoBehaviour
    {
        GestureRecognizer gestureRecognizer;
        public TextMesh TextMesh;
        void Awake()
        {
            gestureRecognizer = new GestureRecognizer();
            gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);
            gestureRecognizer.Tapped += GestureRecognizer_Tapped;
        }

        // Use this for initialization
        void Start()
        {
            gestureRecognizer.StartCapturingGestures();
        }

        private void GestureRecognizer_Tapped(TappedEventArgs obj)
        {
            SaveFrame();
        }
        
        void Update()
        {
            if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.P))
            {
                SaveFrame();
            }
        }

        void SaveFrame()
        {
            var videoFeed = GetComponentInParent<VideoFeed>();
            StartCoroutine(RequestSentiment(videoFeed.CurrentFrame()));
        }

        IEnumerator RequestSentiment(Texture2D source)
        {
            var sr = new SentimentRequest();
            yield return sr.Send(source.EncodeToPNG());

            TextMesh.text = sr.Result.ToString();
        }
    }
}
