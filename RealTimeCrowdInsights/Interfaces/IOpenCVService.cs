using OpenCvSharp;

namespace RealTimeCrowdInsights.Interfaces
{
    public interface IOpenCVService
    {
        CascadeClassifier DefaultFrontalFaceDetector();
    }
}
