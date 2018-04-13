using System.Collections.Generic;

namespace RealTimeFaceInsights.Interfaces
{
    public interface IVideoFrameAnalyzerService
    {
        List<string> GetAvailableCameraList();
        void InitializeFrameGrabber();
        void StartProcessing();
        void StopProcessing();
    }
}
