using System.Collections.Generic;

namespace RealTimeFaceInsights.Interfaces
{
    public interface IVideoFrameAnalyzerService
    {
        List<string> GetAvailableCameraList();
        void InitializeFrameGrabber();
        void StartProcessing(string selectedCamera);
        void StopProcessing();
    }
}
