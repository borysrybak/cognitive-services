using Caliburn.Micro;
using RealTimeCrowdInsights.Models;
using System.Collections.Generic;
using VideoFrameAnalyzer;

namespace RealTimeCrowdInsights.ViewModels
{
    public class ShellViewModel : Screen
    {
        private readonly FrameGrabber<LiveCameraResult> _grabber;

        public ShellViewModel()
        {
            _grabber = new FrameGrabber<LiveCameraResult>();
        }

        public List<string> CameraList
        {
            get { return new List<string> { "foo", "bar" }; }
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
    }
}
