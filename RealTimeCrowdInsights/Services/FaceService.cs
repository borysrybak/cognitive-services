using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using RealTimeCrowdInsights.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RealTimeCrowdInsights.Services
{
    public class FaceService : IFaceService
    {
        private readonly string _faceServiceClientSubscriptionKey = "x";
        private readonly string _faceServiceClientApiRoot = "y";
        private readonly FaceServiceClient _faceServiceClient;
        private readonly List<FaceAttributeType> _defaultFaceAttributes;

        private int _faceAPICallCount = 0;

        public FaceService()
        {
            _faceServiceClient = new FaceServiceClient(_faceServiceClientSubscriptionKey, _faceServiceClientApiRoot);
            _defaultFaceAttributes = new List<FaceAttributeType>();
            InitializeDefaultFaceAttributes();
        }

        public FaceServiceClient GetFaceServiceClient()
        {
            return _faceServiceClient;
        }

        public Face[] DetectFaces(MemoryStream imageStream, IEnumerable<FaceAttributeType> faceAttributeTypes = null)
        {
            return DetectFacesFromImage(imageStream).Result;
        }

        public Face[] DetectFaces(string imagePath, IEnumerable<FaceAttributeType> faceAttriubuteTypes = null)
        {
            return DetectFacesFromImage(imagePath).Result;
        }

        public Face[] DetectFacesWithDefaultAttributes(MemoryStream imageStream)
        {
            return DetectFacesFromImage(imageStream, _defaultFaceAttributes).Result;
        }

        public Face[] DetectFacesWithDefaultAttributes(string imagePath)
        {
            return DetectFacesFromImage(imagePath, _defaultFaceAttributes).Result;
        }

        public int GetFaceServiceClientAPICallCount()
        {
            return _faceAPICallCount;
        }

        private async Task<Face[]> DetectFacesFromImage(dynamic image, IEnumerable<FaceAttributeType> faceAttributeTypes = null)
        {
            var result = await _faceServiceClient.DetectAsync(image, true, false, faceAttributeTypes);

            _faceAPICallCount++;

            return result;
        }
        private void InitializeDefaultFaceAttributes()
        {
            _defaultFaceAttributes.Add(FaceAttributeType.Age);
            _defaultFaceAttributes.Add(FaceAttributeType.Gender);
            _defaultFaceAttributes.Add(FaceAttributeType.HeadPose);
        }
    }
}
