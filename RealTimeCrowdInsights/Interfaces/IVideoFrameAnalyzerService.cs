using System.Collections.Generic;

namespace RealTimeCrowdInsights.Interfaces
{
    public interface IVideoFrameAnalyzerService
    {
        List<string> GetAvailableCameraList();
        void InitializeFrameGrabber();
        void StartProcessing();
    }
}
