using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System.IO;

namespace RealTimeFaceInsights.Interfaces
{
    public interface IComputerVisionService
    {
        VisionServiceClient GetVisionServiceClient();
        Tag[] GetTags(MemoryStream imageStream);
        Tag[] GetTags(string imagePath);
        int GetVisionServiceClientAPICallCount();
    }
}
