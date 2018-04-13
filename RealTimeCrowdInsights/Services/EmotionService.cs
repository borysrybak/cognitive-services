﻿using Microsoft.ProjectOxford.Common;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Emotion;
using RealTimeFaceInsights.Interfaces;
using RealTimeFaceInsights.Models;
using RealTimeFaceInsights.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VideoFrameAnalyzer;

namespace RealTimeFaceInsights.Services
{
    public class EmotionService : IEmotionService
    {
        private readonly string _emotionServiceClientSubscriptionKey = Properties.Settings.Default.EmotionAPIKey.Trim();
        private readonly string _emotionServiceClientApiRoot = Properties.Settings.Default.EmotionAPIHost;
        private readonly EmotionServiceClient _emotionServiceClient;

        private int _emotionAPICallCount = 0;
        private List<float> _angerArray = new List<float>();
        private List<float> _contemptArray = new List<float>();
        private List<float> _disgustArray = new List<float>();
        private List<float> _fearArray = new List<float>();
        private List<float> _happinessArray = new List<float>();
        private List<float> _neutralArray = new List<float>();
        private List<float> _sadnessArray = new List<float>();
        private List<float> _surpriseArray = new List<float>();

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

        public async Task<LiveCameraResult> EmotionsAnalysisFunction(VideoFrame frame)
        {
            return await SubmitEmotionsAnalysisFunction(frame);
        }

        public string SummarizeEmotionScores(EmotionScores emotionScores)
        {
            return SummarizeEmotion(emotionScores);
        }

        public void AddEmotionScoresToStatistics(EmotionScores emotionScores)
        {
            AddAndCalculateEmotionScoresStatistics(emotionScores);
        }

        public EmotionScores CalculateEmotionScoresStatistics()
        {
            return GetEmotionScoresStatistics();
        }

        private async Task<LiveCameraResult> SubmitEmotionsAnalysisFunction(VideoFrame frame)
        {
            var result = new LiveCameraResult();

            var frameImage = frame.Image.ToMemoryStream(".jpg", ImageEncodingParameter.JpegParams); ;
            var emotions = await RecognizeEmotionsFromImage(frameImage);
            var emotionScores = emotions.Select(e => e.Scores).ToArray();
            result.EmotionScores = emotionScores;

            return result;
        }
        private async Task<Emotion[]> RecognizeEmotionsFromImage(dynamic image, Rectangle[] faceRectangles = null)
        {
            var result = await _emotionServiceClient.RecognizeAsync(image, faceRectangles);

            _emotionAPICallCount++;

            return result;
        }
        private Tuple<string, float> GetDominantEmotion(EmotionScores emotionScores)
        {
            return emotionScores.ToRankedList().Select(kv => new Tuple<string, float>(kv.Key, kv.Value)).First();
        }
        private string SummarizeEmotion(EmotionScores emotionScores)
        {
            var bestEmotion = GetDominantEmotion(emotionScores);
            return string.Format("{0}: {1:N1}", bestEmotion.Item1, bestEmotion.Item2);
        }
        private void AddAndCalculateEmotionScoresStatistics(EmotionScores emotionScores)
        {
            _angerArray.Add(emotionScores.Anger);
            _contemptArray.Add(emotionScores.Contempt);
            _disgustArray.Add(emotionScores.Disgust);
            _fearArray.Add(emotionScores.Fear);
            _happinessArray.Add(emotionScores.Happiness);
            _neutralArray.Add(emotionScores.Neutral);
            _sadnessArray.Add(emotionScores.Sadness);
            _surpriseArray.Add(emotionScores.Surprise);
        }
        private EmotionScores GetEmotionScoresStatistics()
        {
            var result = new EmotionScores();

            result.Anger = _angerArray.Average();
            result.Contempt = _contemptArray.Average();
            result.Disgust = _disgustArray.Average();
            result.Fear = _fearArray.Average();
            result.Happiness = _happinessArray.Average();
            result.Neutral = _neutralArray.Average();
            result.Sadness = _sadnessArray.Average();
            result.Surprise = _surpriseArray.Average();

            return result;
        }
    }
}
