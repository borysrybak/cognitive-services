using Caliburn.Micro;
using RealTimeCrowdInsights.Interfaces;
using RealTimeCrowdInsights.Models;
using System.Collections.Generic;
using System.Linq;
using VideoFrameAnalyzer;

namespace RealTimeCrowdInsights.ViewModels
{
    public class ShellViewModel : Screen
    {
        private readonly IVideoFrameAnalyzerService _videoFrameAnalyzerService;

        public ShellViewModel(IVideoFrameAnalyzerService videoFrameAnalyzerService)
        {
            _videoFrameAnalyzerService = videoFrameAnalyzerService;
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

        private string _imageSource;
        public string ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                NotifyOfPropertyChange(() => ImageSource);
            }
        }

        public void StartAnalyze()
        {
            _videoFrameAnalyzerService.InitializeFrameGrabber();
            _videoFrameAnalyzerService.StartProcessing();
        }

        public void StopAnalyze()
        {
            
        }
    }
}
