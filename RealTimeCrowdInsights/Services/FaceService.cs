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

        public Face[] GetFacesFromImage(MemoryStream imageStream, IEnumerable<FaceAttributeType> faceAttributeTypes = null)
        {
            return FacesFromImageStream(imageStream, faceAttributeTypes).Result;
        }

        public Face[] GetFacesFromImage(string imagePath, IEnumerable<FaceAttributeType> faceAttriubuteTypes = null)
        {
            return FacesFromImagePath(imagePath, faceAttriubuteTypes).Result;
        }

        public Face[] GetFacesWithDefaultAttributesFromImage(MemoryStream imageStream)
        {
            return FacesWithDefaultAttributesFromImage(imageStream).Result;
        }

        public Face[] GetFacesWithDefaultAttributesFromImage(string imagePath)
        {
            return FacesWithDefaultAttributesFromImage(imagePath).Result;
        }

        public int GetFaceServiceClientAPICallCount()
        {
            return _faceAPICallCount;
        }

        private async Task<Face[]> FacesFromImageStream(MemoryStream imageStream, IEnumerable<FaceAttributeType> faceAttributeTypes = null)
        {
            var result = await _faceServiceClient.DetectAsync(imageStream, true, false, faceAttributeTypes);

            _faceAPICallCount++;

            return result;
        }
        private async Task<Face[]> FacesFromImagePath(string imagePath, IEnumerable<FaceAttributeType> faceAttriubuteTypes = null)
        {
            var result = await _faceServiceClient.DetectAsync(imagePath, true, false, faceAttriubuteTypes);

            _faceAPICallCount++;

            return result;
        }
        private async Task<Face[]> FacesWithDefaultAttributesFromImage(MemoryStream imageStream)
        {
            var result = new Face[0];
            var defaultAttributes = _defaultFaceAttributes;

            result = await _faceServiceClient.DetectAsync(imageStream, true, false, defaultAttributes);

            return result;
        }
        private async Task<Face[]> FacesWithDefaultAttributesFromImage(string imagePath)
        {
            var result = new Face[0];
            var defaultAttributes = _defaultFaceAttributes;

            result = await _faceServiceClient.DetectAsync(imagePath, true, false, defaultAttributes);

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
