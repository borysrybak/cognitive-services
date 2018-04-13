using Microsoft.ProjectOxford.Face.Contract;
using OpenCvSharp;

namespace RealTimeFaceInsights.Interfaces
{
    public interface IOpenCVService
    {
        CascadeClassifier DefaultFrontalFaceDetector();
        void MatchAndReplaceFaces(Face[] faces, Rect[] clientRects);
    }
}
