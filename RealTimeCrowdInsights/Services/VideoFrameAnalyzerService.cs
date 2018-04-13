using Caliburn.Micro;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using RealTimeFaceInsights.Events;
using RealTimeFaceInsights.Interfaces;
using RealTimeFaceInsights.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using VideoFrameAnalyzer;

namespace RealTimeFaceInsights.Services
{
    public class VideoFrameAnalyzerService : IVideoFrameAnalyzerService
    {
        private readonly TimeSpan _analysisInterval = new TimeSpan(0, 0, 0, 1, 0);
        private readonly IEventAggregator _eventAggregator;
        private readonly IVisualizationService _visualizationService;
        private readonly IOpenCVService _openCVService;
        private readonly IFaceService _faceService;
        private readonly FrameGrabber<LiveCameraResult> _frameGrabber;
        private readonly CascadeClassifier _localFaceDetector;

        private LiveCameraResult _currentLiveCameraResult;

        public VideoFrameAnalyzerService(IEventAggregator eventAggregator, IVisualizationService visualizationService, IOpenCVService openCVService, IFaceService faceService)
        {
            _frameGrabber = new FrameGrabber<LiveCameraResult>();
            _eventAggregator = eventAggregator;
            _visualizationService = visualizationService;
            _openCVService = openCVService;
            _faceService = faceService;
            _localFaceDetector = _openCVService.DefaultFrontalFaceDetector();
        }

        public List<string> GetAvailableCameraList()
        {
            return LoadCameraList();
        }

        public void InitializeFrameGrabber()
        {
            SetUpListenerNewFrame();
            SetUpListenerNewResultFromAPICall();
            _openCVService.DefaultFrontalFaceDetector();
            _frameGrabber.AnalysisFunction = _faceService.FacesAnalysisFunction;
        }

        public void StartProcessing()
        {
            StartProcessingCamera();
        }

        public void StopProcessing()
        {
            StopProcessingCamera();
        }

        private List<string> LoadCameraList()
        {
            var result = new List<string>();
            var numberOfCameras = _frameGrabber.GetNumCameras();
            if (numberOfCameras == 0)
            {
                //TODO: Listen from ShellViewModel for message about "No cameras found!"
            }
            var cameras = Enumerable.Range(0, numberOfCameras).Select(i => string.Format("Camera {0}", i + 1));
            foreach (var camera in cameras)
            {
                result.Add(camera);
            }

            return result;
        }
        private void SetUpListenerNewFrame()
        {
            _frameGrabber.NewFrameProvided += (s, e) =>
            {
                var detectedFacesRectangles = _localFaceDetector.DetectMultiScale(e.Frame.Image);
                e.Frame.UserData = detectedFacesRectangles;

                Application.Current.Dispatcher.BeginInvoke((System.Action)(() =>
                {
                    var frameImage = e.Frame.Image.ToBitmapSource();
                    var resultImage = _visualizationService.Visualize(e.Frame, _currentLiveCameraResult);
                    _eventAggregator.PublishOnUIThread(new FrameImageProvidedEvent() { FrameImage = frameImage });
                    _eventAggregator.PublishOnUIThread(new ResultImageAvailableEvent() { ResultImage = resultImage });
                }));
            };
        }
        private void SetUpListenerNewResultFromAPICall()
        {
            _frameGrabber.NewResultAvailable += (s, e) =>
            {
                Application.Current.Dispatcher.BeginInvoke((System.Action)(() =>
                {
                    if (e.TimedOut)
                    {
                        //MessageArea.Text = "API call timed out.";
                    }
                    else if (e.Exception != null)
                    {
                        //MessageArea.Text = "API Exception Message.";
                    }
                    else
                    {
                        _currentLiveCameraResult = e.Analysis;

                        if (_currentLiveCameraResult.Faces.Length > 0)
                        {
                            var faceAttributes = _currentLiveCameraResult.Faces[0].FaceAttributes;
                            _eventAggregator.PublishOnUIThread(new FaceAttributesResultEvent() { FaceAttributesResult = faceAttributes });
                        }
                    }
                }));
            };
        }
        private async void StartProcessingCamera()
        {
            _frameGrabber.TriggerAnalysisOnInterval(_analysisInterval);
            await _frameGrabber.StartProcessingCameraAsync();
        }
        private async void StopProcessingCamera()
        {
            await _frameGrabber.StopProcessingAsync();
        }
    }
}
