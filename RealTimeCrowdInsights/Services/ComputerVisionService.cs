using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using RealTimeCrowdInsights.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeCrowdInsights.Services
{
    public class ComputerVisionService : IComputerVisionService
    {
        private readonly string _visionServiceClientSubscriptionKey = "x";
        private readonly string _visionServiceClientApiRoot = "y";
        private readonly VisionServiceClient _visionServiceClient;

        private int _visionAPICallCount = 0;

        public ComputerVisionService()
        {
            _visionServiceClient = new VisionServiceClient(_visionServiceClientSubscriptionKey, _visionServiceClientApiRoot);
        }

        public VisionServiceClient GetVisionServiceClient()
        {
            return _visionServiceClient;
        }

        public Tag[] GetTags(MemoryStream imageStream)
        {
            return AnalyzeImageBySpecificVisualFeatures(imageStream, VisualFeature.Tags).Result;
        }

        public Tag[] GetTags(string imagePath)
        {
            return AnalyzeImageBySpecificVisualFeatures(imagePath, VisualFeature.Tags).Result;
        }

        public int GetVisionServiceClientAPICallCount()
        {
            return _visionAPICallCount;
        }

        private async Task<Tag[]> AnalyzeImageBySpecificVisualFeatures(dynamic image, params VisualFeature[] visualFeatures)
        {
            var result = new Tag[0];
            var analysisResult = new AnalysisResult();
            var visualFeaturesLength = visualFeatures.Length;
            var featuresList = new List<string>();
            foreach (var feature in visualFeatures)
            {
                var featureAsString = Enum.GetName(typeof(VisualFeature), feature);
                featuresList.Add(featureAsString);
            }
            var features = featuresList.ToArray();

            analysisResult = await _visionServiceClient.AnalyzeImageAsync(image, features);
            result = analysisResult.Tags;

            _visionAPICallCount++;

            return result;
        }
    }
}
