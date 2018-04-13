﻿using Caliburn.Micro;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Face.Contract;
using RealTimeFaceInsights.Events;
using RealTimeFaceInsights.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace RealTimeFaceInsights.ViewModels
{
    public class ShellViewModel : Screen, IHandle<FrameImageProvidedEvent>, IHandle<ResultImageAvailableEvent>, IHandle<FaceAttributesResultEvent>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IVideoFrameAnalyzerService _videoFrameAnalyzerService;
        private readonly IVisualizationService _visualizationService;
        private readonly IEmotionService _emotionService;
        private readonly IFaceService _faceService;
        private readonly DispatcherTimer _timer;

        public ShellViewModel(IEventAggregator eventAggregator, IVideoFrameAnalyzerService videoFrameAnalyzerService,
            IVisualizationService visualizationService, IEmotionService emotionService, IFaceService faceService)
        {
            _eventAggregator = eventAggregator;
            _videoFrameAnalyzerService = videoFrameAnalyzerService;
            _visualizationService = visualizationService;
            _emotionService = emotionService;
            _faceService = faceService;
            _videoFrameAnalyzerService.InitializeFrameGrabber();

            _timer = new DispatcherTimer(DispatcherPriority.Render);
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (sender, args) => { CurrentTime = DateTime.Now.ToLongTimeString(); };
            _timer.Start();
        }

        public List<string> CameraList
        {
            get { return _videoFrameAnalyzerService.GetAvailableCameraList(); }
        }

        private string _selectedCameraList;
        public string SelectedCameraList
        {
            get { return _selectedCameraList; }
            set
            {
                _selectedCameraList = value;
                NotifyOfPropertyChange(() => SelectedCameraList);
            }
        }

        private BitmapSource _frameImage;
        public BitmapSource FrameImage
        {
            get { return _frameImage; }
            set
            {
                _frameImage = value;
                NotifyOfPropertyChange(() => FrameImage);
            }
        }

        private BitmapSource _resultImage;
        public BitmapSource ResultImage
        {
            get { return _resultImage; }
            set
            {
                _resultImage = value;
                NotifyOfPropertyChange(() => ResultImage);
            }
        }

        private double _age;
        public double Age
        {
            get { return _age; }
            set
            {
                _age = value;
                NotifyOfPropertyChange(() => Age);
            }
        }

        private string _gender;
        public string Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                NotifyOfPropertyChange(() => Gender);
            }
        }

        private double _roll;
        public double Roll
        {
            get { return _roll; }
            set
            {
                _roll = value;
                NotifyOfPropertyChange(() => Roll);
            }
        }

        private double _yaw;
        public double Yaw
        {
            get { return _yaw; }
            set
            {
                _yaw = value;
                NotifyOfPropertyChange(() => Yaw);
            }
        }

        private double _pitch;
        public double Pitch
        {
            get { return _pitch; }
            set
            {
                _pitch = value;
                NotifyOfPropertyChange(() => Pitch);
            }
        }

        private double _bald;
        public double Bald
        {
            get { return _bald; }
            set
            {
                _bald = value;
                NotifyOfPropertyChange(() => Bald);
            }
        }

        private bool _isInvisible;
        public bool IsInvisible
        {
            get { return _isInvisible; }
            set
            {
                _isInvisible = value;
                NotifyOfPropertyChange(() => IsInvisible);
            }
        }

        private double _black;
        public double Black
        {
            get { return _black; }
            set
            {
                _black = value;
                NotifyOfPropertyChange(() => Black);
            }
        }

        private double _blond;
        public double Blond
        {
            get { return _blond; }
            set
            {
                _blond = value;
                NotifyOfPropertyChange(() => Blond);
            }
        }

        private double _brown;
        public double Brown
        {
            get { return _brown; }
            set
            {
                _brown = value;
                NotifyOfPropertyChange(() => Brown);
            }
        }

        private double _gray;
        public double Gray
        {
            get { return _gray; }
            set
            {
                _gray = value;
                NotifyOfPropertyChange(() => Gray);
            }
        }

        private double _other;
        public double Other
        {
            get { return _other; }
            set
            {
                _other = value;
                NotifyOfPropertyChange(() => Other);
            }
        }

        private double _red;
        public double Red
        {
            get { return _red; }
            set
            {
                _red = value;
                NotifyOfPropertyChange(() => Red);
            }
        }

        private double _unknown;
        public double Unknown
        {
            get { return _unknown; }
            set
            {
                _unknown = value;
                NotifyOfPropertyChange(() => Unknown);
            }
        }

        private double _white;
        public double White
        {
            get { return _white; }
            set
            {
                _white = value;
                NotifyOfPropertyChange(() => White);
            }
        }

        private double _moustache;
        public double Moustache
        {
            get { return _moustache; }
            set
            {
                _moustache = value;
                NotifyOfPropertyChange(() => Moustache);
            }
        }

        private double _beard;
        public double Beard
        {
            get { return _beard; }
            set
            {
                _beard = value;
                NotifyOfPropertyChange(() => Beard);
            }
        }

        private double _sideburns;
        public double Sideburns
        {
            get { return _sideburns; }
            set
            {
                _sideburns = value;
                NotifyOfPropertyChange(() => Sideburns);
            }
        }

        private string _glasses;
        public string Glasses
        {
            get { return _glasses; }
            set
            {
                _glasses = value;
                NotifyOfPropertyChange(() => Glasses);
            }
        }

        private bool _isEyeMakeup;
        public bool IsEyeMakeup
        {
            get { return _isEyeMakeup; }
            set
            {
                _isEyeMakeup = value;
                NotifyOfPropertyChange(() => IsEyeMakeup);
            }
        }

        private bool _isLipMakeup;
        public bool IsLipMakeup
        {
            get { return _isLipMakeup; }
            set
            {
                _isLipMakeup = value;
                NotifyOfPropertyChange(() => IsLipMakeup);
            }
        }

        private string _accessories;
        public string Accessories
        {
            get { return _accessories; }
            set
            {
                _accessories = value;
                NotifyOfPropertyChange(() => Accessories);
            }
        }

        private double _averageAge;
        public double AverageAge
        {
            get { return _averageAge; }
            set
            {
                _averageAge = value;
                NotifyOfPropertyChange(() => AverageAge);
            }
        }

        private int _faceAPICallCount;
        private int FaceAPICallCount
        {
            get { return _faceAPICallCount; }
            set
            {
                _faceAPICallCount = value;
                NotifyOfPropertyChange(() => FaceAPICallCount);
            }
        }

        private float _anger;
        public float Anger
        {
            get { return _anger; }
            set
            {
                _anger = value;
                NotifyOfPropertyChange(() => Anger);
            }
        }

        private float _contempt;
        public float Contempt
        {
            get { return _contempt; }
            set
            {
                _contempt = value;
                NotifyOfPropertyChange(() => Contempt);
            }
        }

        private float _disgust;
        public float Disgust
        {
            get { return _disgust; }
            set
            {
                _disgust = value;
                NotifyOfPropertyChange(() => Disgust);
            }
        }

        private float _fear;
        public float Fear
        {
            get { return _fear; }
            set
            {
                _fear = value;
                NotifyOfPropertyChange(() => Fear);
            }
        }

        private float _happiness;
        public float Happiness
        {
            get { return _happiness; }
            set
            {
                _happiness = value;
                NotifyOfPropertyChange(() => Happiness);
            }
        }

        private float _neutral;
        public float Neutral
        {
            get { return _neutral; }
            set
            {
                _neutral = value;
                NotifyOfPropertyChange(() => Neutral);
            }
        }

        private float _sadness;
        public float Sadness
        {
            get { return _sadness; }
            set
            {
                _sadness = value;
                NotifyOfPropertyChange(() => Sadness);
            }
        }

        private float _surprise;
        public float Surprise
        {
            get { return _surprise; }
            set
            {
                _surprise = value;
                NotifyOfPropertyChange(() => Surprise);
            }
        }

        private float _averageAnger;
        public float AverageAnger
        {
            get { return _averageAnger; }
            set
            {
                _averageAnger = value;
                NotifyOfPropertyChange(() => AverageAnger);
            }
        }

        private float _averageContempt;
        public float AverageContempt
        {
            get { return _averageContempt; }
            set
            {
                _averageContempt = value;
                NotifyOfPropertyChange(() => AverageContempt);
            }
        }

        private float _averageDisgust;
        public float AverageDisgust
        {
            get { return _averageDisgust; }
            set
            {
                _averageDisgust = value;
                NotifyOfPropertyChange(() => AverageDisgust);
            }
        }

        private float _averageFear;
        public float AverageFear
        {
            get { return _averageFear; }
            set
            {
                _averageFear = value;
                NotifyOfPropertyChange(() => AverageFear);
            }
        }

        private float _averageHappiness;
        public float AverageHappiness
        {
            get { return _averageHappiness; }
            set
            {
                _averageHappiness = value;
                NotifyOfPropertyChange(() => AverageHappiness);
            }
        }

        private float _averageNeutral;
        public float AverageNeutral
        {
            get { return _averageNeutral; }
            set
            {
                _averageNeutral = value;
                NotifyOfPropertyChange(() => AverageNeutral);
            }
        }

        private float _averageSadness;
        public float AverageSadness
        {
            get { return _averageSadness; }
            set
            {
                _averageSadness = value;
                NotifyOfPropertyChange(() => AverageSadness);
            }
        }

        private float _averageSurprise;
        public float AverageSurprise
        {
            get { return _averageSurprise; }
            set
            {
                _averageSurprise = value;
                NotifyOfPropertyChange(() => AverageSurprise);
            }
        }

        private ObservableCollection<Rectangle> _emotionBars = new ObservableCollection<Rectangle>();
        public ObservableCollection<Rectangle> EmotionBars
        {
            get { return _emotionBars; }
            set
            {
                _emotionBars = value;
                NotifyOfPropertyChange(() => EmotionBars);
            }
        }

        private ObservableCollection<Rectangle> _hairColor;
        public ObservableCollection<Rectangle> HairColor
        {
            get { return _hairColor; }
            set
            {
                _hairColor = value;
                NotifyOfPropertyChange(() => HairColor);
            }
        }

        private string _currentTime;
        public string CurrentTime
        {
            get { return _currentTime; }
            set
            {
                _currentTime = value;
                NotifyOfPropertyChange(() => CurrentTime);
            }
        }

        private int _currentSessionTimer = 0;
        public int CurrentSessionTimer
        {
            get { return _currentSessionTimer; }
            set
            {
                _currentSessionTimer = value;
                NotifyOfPropertyChange(() => CurrentSessionTimer);
            }
        }

        protected override void OnActivate()
        {
            _eventAggregator.Subscribe(this);
            base.OnActivate();
        }

        protected override void OnDeactivate(bool close)
        {
            _eventAggregator.Unsubscribe(this);
            base.OnDeactivate(close);
        }

        public void StartAnalyze()
        {
            _videoFrameAnalyzerService.StartProcessing();
        }

        public void StopAnalyze()
        {
            _videoFrameAnalyzerService.StopProcessing();
        }

        public void Handle(FrameImageProvidedEvent message)
        {
            FrameImage = message.FrameImage;
        }

        public void Handle(ResultImageAvailableEvent message)
        {
            ResultImage = message.ResultImage;
        }

        public void Handle(FaceAttributesResultEvent message)
        {
            var faceAttributes = message.FaceAttributesResult;
            AssignFaceAttributes(faceAttributes);

            var age = faceAttributes.Age;
            _faceService.AddAgeToStatistics(age);

            var averageAge = _faceService.CalculateAverageAge();
            AssignAverageAge(averageAge);

            var emotionScores = faceAttributes.Emotion;
            GenerateAndPopulateEmotionBar(emotionScores);
            _emotionService.AddEmotionScoresToStatistics(emotionScores);

            var emotionScoresStatistics = _emotionService.CalculateEmotionScoresStatistics();
            AssignEmotionStatistics(emotionScoresStatistics);

            var hairColors = faceAttributes.Hair.HairColor;
            GenerateHairColor(hairColors);

            var faceAPICallCount = _faceService.GetFaceServiceClientAPICallCount();
            AssignFaceAPICallCount(faceAPICallCount);
        }

        private void AssignFaceAttributes(FaceAttributes faceAttributes)
        {
            AssignBasicAttributes(faceAttributes);
            AssignHairAttributes(faceAttributes);
            AssignFacialHairAttributes(faceAttributes);
            AssignAdditionalAttributes(faceAttributes);
            AssignEmotionAttributes(faceAttributes);
        }
        private void AssignBasicAttributes(FaceAttributes faceAttributes)
        {
            Age = faceAttributes.Age;
            Gender = faceAttributes.Gender;

            Roll = faceAttributes.HeadPose.Roll;
            Yaw = faceAttributes.HeadPose.Yaw;
            Pitch = faceAttributes.HeadPose.Pitch;
        }
        private void AssignHairAttributes(FaceAttributes faceAttributes)
        {
            Bald = faceAttributes.Hair.Bald;
            IsInvisible = faceAttributes.Hair.Invisible;

            var hairColors = faceAttributes.Hair.HairColor;
            foreach (var hairColor in hairColors)
            {
                if (hairColor.Color == HairColorType.Black) { Black = hairColor.Confidence; }
                if (hairColor.Color == HairColorType.Blond) { Blond = hairColor.Confidence; }
                if (hairColor.Color == HairColorType.Brown) { Brown = hairColor.Confidence; }
                if (hairColor.Color == HairColorType.Gray) { Gray = hairColor.Confidence; }
                if (hairColor.Color == HairColorType.Other) { Other = hairColor.Confidence; }
                if (hairColor.Color == HairColorType.Red) { Red = hairColor.Confidence; }
                if (hairColor.Color == HairColorType.Unknown) { Unknown = hairColor.Confidence; }
                if (hairColor.Color == HairColorType.White) { White = hairColor.Confidence; }
            }
        }
        private void AssignFacialHairAttributes(FaceAttributes faceAttributes)
        {
            Moustache = faceAttributes.FacialHair.Moustache;
            Beard = faceAttributes.FacialHair.Beard;
            Sideburns = faceAttributes.FacialHair.Sideburns;
        }
        private void AssignAdditionalAttributes(FaceAttributes faceAttributes)
        {
            Glasses = faceAttributes.Glasses.ToString();
            IsEyeMakeup = faceAttributes.Makeup.EyeMakeup;
            IsLipMakeup = faceAttributes.Makeup.LipMakeup;

            var accessories = faceAttributes.Accessories;
            if (accessories.Length > 0)
            {
                var accessoryList = new StringBuilder();
                foreach (var accessory in accessories)
                {
                    var accessoryType = accessory.Type.ToString();
                    var accessoryConfidence = accessory.Confidence.ToString();
                    accessoryList.Append(accessoryType + ": " + accessoryConfidence + ", ");
                }
                Accessories = accessoryList.ToString();
            }
            else
            {
                Accessories = "None";
            }

        }
        private void AssignEmotionAttributes(FaceAttributes faceAttributes)
        {
            Anger = faceAttributes.Emotion.Anger;
            Contempt = faceAttributes.Emotion.Contempt;
            Disgust = faceAttributes.Emotion.Disgust;
            Fear = faceAttributes.Emotion.Fear;
            Happiness = faceAttributes.Emotion.Happiness;
            Neutral = faceAttributes.Emotion.Neutral;
            Sadness = faceAttributes.Emotion.Sadness;
            Surprise = faceAttributes.Emotion.Surprise;
        }
        private void GenerateAndPopulateEmotionBar(EmotionScores emotionScores)
        {
            var bar = _visualizationService.ComposeRectangleBar(emotionScores);
            EmotionBars.Add(bar);
        }
        private void AssignEmotionStatistics(EmotionScores emotionScoresStatistics)
        {
            AverageAnger = emotionScoresStatistics.Anger;
            AverageContempt = emotionScoresStatistics.Contempt;
            AverageDisgust = emotionScoresStatistics.Disgust;
            AverageFear = emotionScoresStatistics.Fear;
            AverageHappiness = emotionScoresStatistics.Happiness;
            AverageNeutral = emotionScoresStatistics.Neutral;
            AverageSadness = emotionScoresStatistics.Sadness;
            AverageSurprise = emotionScoresStatistics.Surprise;
        }
        private void AssignAverageAge(double averageAge)
        {
            AverageAge = averageAge;
        }
        private void GenerateHairColor(HairColor[] hairColors)
        {
            var mixedHairColor = _visualizationService.MixHairColor(hairColors);
            HairColor = new ObservableCollection<Rectangle>(mixedHairColor);
        }
        private void AssignFaceAPICallCount(int faceAPICallCount)
        {
            var FaceAPICallCount = faceAPICallCount;
        }
    }
}
