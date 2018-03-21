using Microsoft.ProjectOxford.Common;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Emotion;
using RealTimeCrowdInsights.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace RealTimeCrowdInsights.Services
{
    public class EmotionService : IEmotionService
    {
        private readonly string _emotionServiceClientSubscriptionKey = "x";
        private readonly string _emotionServiceClientApiRoot = "y";
        private readonly EmotionServiceClient _emotionServiceClient;

        private int _emotionAPICallCount = 0;

        public EmotionService()
        {
            _emotionServiceClient = new EmotionServiceClient(_emotionServiceClientSubscriptionKey, _emotionServiceClientApiRoot);
        }

        public EmotionServiceClient GetEmotionServiceClient()
        {
            return _emotionServiceClient;
        }

        public Emotion[] RecognizeEmotions(MemoryStream imageStream)
        {
            return RecognizeEmotionsFromImage(imageStream).Result;
        }

        public Emotion[] RecognizeEmotions(string imagePath)
        {
            return RecognizeEmotionsFromImage(imagePath).Result;
        }

        public Emotion[] RecognizeEmotionsWithLocalFaceDetections(MemoryStream memoryStream, Rectangle[] faceRectangles)
        {
            return RecognizeEmotionsFromImage(memoryStream, faceRectangles).Result;
        }

        public Emotion[] RecognizeEmotionsWithLocalFaceDetections(string imagePath, Rectangle[] faceRectangles)
        {
            return RecognizeEmotionsFromImage(imagePath, faceRectangles).Result;
        }

        public int GetEmotionServiceClientAPICallCount()
        {
            return _emotionAPICallCount;
        }

        private async Task<Emotion[]> RecognizeEmotionsFromImage(dynamic image, Rectangle[] faceRectangles = null)
        {
            var result = await _emotionServiceClient.RecognizeAsync(image, faceRectangles);

            _emotionAPICallCount++;

            return result;
        }
    }
}
