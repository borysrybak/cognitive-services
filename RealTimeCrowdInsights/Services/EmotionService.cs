using Microsoft.ProjectOxford.Emotion;
using RealTimeCrowdInsights.Interfaces;

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
    }
}
