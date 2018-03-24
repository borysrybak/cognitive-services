using OpenCvSharp.Extensions;
using RealTimeCrowdInsights.Interfaces;
using RealTimeCrowdInsights.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoFrameAnalyzer;

namespace RealTimeCrowdInsights.Services
{
    public class VideoFrameAnalyzerService : IVideoFrameAnalyzerService
    {
        private readonly IOpenCVService _openCVService;
        private readonly FrameGrabber<LiveCameraResult> _frameGrabber;

        public VideoFrameAnalyzerService(IOpenCVService openCVService)
        {
            _frameGrabber = new FrameGrabber<LiveCameraResult>();
            _openCVService = openCVService;
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
        }

        public void StartProcessing()
        {
            StartProcessingCamera();
        }

        private List<string> LoadCameraList()
        {
            var result = new List<string>();
            var numberOfCameras = _frameGrabber.GetNumCameras();
            if(numberOfCameras == 0)
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

                // The callback may occur on a different thread, so we must use the
                // MainWindow.Dispatcher when manipulating the UI. 
                System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    // Display the image in the left pane.
                    var x = e.Frame.Image.ToBitmapSource();
                }));

                // See if auto-stop should be triggered. 
                //if (Properties.Settings.Default.AutoStopEnabled && (DateTime.Now - _startTime) > Properties.Settings.Default.AutoStopTime)
                //{
                //    _grabber.StopProcessingAsync();
                //}
            };
        }
        private void SetUpListenerNewResultFromAPICall()
        {
            _frameGrabber.NewResultAvailable += (s, e) =>
            {
                System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    if (e.TimedOut)
                    {
                        //MessageArea.Text = "API call timed out.";
                    }
                    else if (e.Exception != null)
                    {
                        //string apiName = "";
                        //string message = e.Exception.Message;
                        //var faceEx = e.Exception as FaceAPIException;
                        //var emotionEx = e.Exception as Microsoft.ProjectOxford.Common.ClientException;
                        //var visionEx = e.Exception as Microsoft.ProjectOxford.Vision.ClientException;
                        //if (faceEx != null)
                        //{
                        //    apiName = "Face";
                        //    message = faceEx.ErrorMessage;
                        //}
                        //else if (emotionEx != null)
                        //{
                        //    apiName = "Emotion";
                        //    message = emotionEx.Error.Message;
                        //}
                        //else if (visionEx != null)
                        //{
                        //    apiName = "Computer Vision";
                        //    message = visionEx.Error.Message;
                        //}
                        //MessageArea.Text = string.Format("{0} API call failed on frame {1}. Exception: {2}", apiName, e.Frame.Metadata.Index, message);
                    }
                    else
                    {
                        var x = e.Analysis;
                    }
                }));
            };
        }
        private async void StartProcessingCamera()
        {
            // How often to analyze. 
            //_frameGrabber.TriggerAnalysisOnInterval(Properties.Settings.Default.AnalysisInterval);
            //await _frameGrabber.StartProcessingCameraAsync(CameraList.SelectedIndex);
        }
    }
}
