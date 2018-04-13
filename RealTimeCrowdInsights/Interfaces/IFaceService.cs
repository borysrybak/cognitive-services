using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using RealTimeFaceInsights.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using VideoFrameAnalyzer;

namespace RealTimeFaceInsights.Interfaces
{
    public interface IFaceService
    {
        FaceServiceClient GetFaceServiceClient();
        Face[] DetectFaces(MemoryStream imageStream, IEnumerable<FaceAttributeType> faceAttributeTypes = null);
        Face[] DetectFaces(string imagePath, IEnumerable<FaceAttributeType> faceAttriubuteTypes = null);
        Face[] DetectFacesWithDefaultAttributes(MemoryStream imageStream);
        Face[] DetectFacesWithDefaultAttributes(string imagePath);
        int GetFaceServiceClientAPICallCount();
        Task<LiveCameraResult> FacesAnalysisFunction(VideoFrame frame);
        string SummarizeFaceAttributes(FaceAttributes faceAttributes);
        void AddAgeToStatistics(double age);
        double CalculateAverageAge();
    }
}
