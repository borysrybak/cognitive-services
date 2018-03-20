using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FaceAPITutorial.Services
{
    public class FaceAPIService
    {
        // Provide Key & Address
        #region Subscribtion Key & API Root Address
        private readonly string _subscriptionKey = "<FaceAPISubscriptionKey>";
        private readonly string _apiRootAddress = "<FaceAPIRootAddress>";
        #endregion Subscribtion Key & API Root Address

        private readonly IFaceServiceClient _faceServiceClient;
        private IEnumerable<FaceAttributeType> _faceAttributes;
        private Face[] _faces;
        private string[] _faceDescriptions;
        private double _resizeFactor;

        private static FaceAPIService _instance;
        public static FaceAPIService Instance
        {
            get
            {
                return _instance ?? (_instance = new FaceAPIService());
            }
        }

        private FaceAPIService()
        {
            _faceServiceClient = new FaceServiceClient(_subscriptionKey, _apiRootAddress);
            _faceAttributes = new FaceAttributeType[] { FaceAttributeType.Gender, FaceAttributeType.Age, FaceAttributeType.Smile, FaceAttributeType.Emotion, FaceAttributeType.Glasses, FaceAttributeType.Hair };
        }

        /// <summary>
        /// Uploads the image file and calls Detect Faces.
        /// </summary>
        /// <param name="imageFilePath"></param>
        /// <returns></returns>
        public async Task<Face[]> UploadAndDetectFaces(string imageFilePath)
        {
            return await GetDetectedFaces(imageFilePath);
        }

        /// <summary>
        /// Returns a string that describes the given face.
        /// </summary>
        /// <param name="face"></param>
        /// <returns></returns>
        public string FaceDescription(Face face)
        {
            return GetFaceDescription(face);
        }

        /// <summary>
        /// Returns a number of detected faces from uploaded image.
        /// </summary>
        /// <returns></returns>
        public int NumberOfDetectedFaces()
        {
            return GetNumberOfDetectedFaces();
        }

        /// <summary>
        /// Returns an array of detected faces from uploaded image.
        /// </summary>
        /// <returns></returns>
        public Face[] DetectedFaces()
        {
            return GetDetectedFaces();
        }

        /// <summary>
        /// Returns an array of detected faces descriptions.
        /// </summary>
        /// <returns></returns>
        public string[] DetectedFacesDescriptions()
        {
            return GetDetectedFacesDescriptions();
        }

        /// <summary>
        /// Set resize factor for drawing properly rectangles around the faces.
        /// </summary>
        /// <param name="resizeFactor"></param>
        public void SetResizeFactor(double resizeFactor)
        {
            _resizeFactor = resizeFactor;
        }

        /// <summary>
        /// Get resize factore value.
        /// </summary>
        /// <returns></returns>
        public double GetResizeFactor()
        {
            return _resizeFactor;
        }

        private async Task<Face[]> GetDetectedFaces(string imageFilePath)
        {
            // Call the Face API.
            try
            {
                using (Stream imageFileStream = File.OpenRead(imageFilePath))
                {
                    _faces = await _faceServiceClient.DetectAsync(imageFileStream, returnFaceId: true, returnFaceLandmarks: false, returnFaceAttributes: _faceAttributes);
                    InitializeFaceDescriptionsStringArray();
                    return _faces;
                }
            }
            // Catch and display Face API errors.
            catch (FaceAPIException f)
            {
                var errorMessage = f.ErrorMessage;
                var errorCode = f.ErrorCode;

                return new Face[0];
            }
            // Catch and display all other errors.
            catch (Exception e)
            {
                var message = e.Message;
                return new Face[0];
            }
        }
        private string GetFaceDescription(Face face)
        {
            var sb = new StringBuilder();

            sb.Append("Face: ");
            // Add the gender, age, and smile.
            sb.Append(face.FaceAttributes.Gender);
            sb.Append(", ");
            sb.Append(face.FaceAttributes.Age);
            sb.Append(", ");
            sb.Append(String.Format("smile {0:F1}%, ", face.FaceAttributes.Smile * 100));

            // Add the emotions. Display all emotions over 10%.
            sb.Append("Emotion: ");
            var emotionScores = face.FaceAttributes.Emotion;
            if (emotionScores.Anger >= 0.1f) sb.Append(String.Format("anger {0:F1}%, ", emotionScores.Anger * 100));
            if (emotionScores.Contempt >= 0.1f) sb.Append(String.Format("contempt {0:F1}%, ", emotionScores.Contempt * 100));
            if (emotionScores.Disgust >= 0.1f) sb.Append(String.Format("disgust {0:F1}%, ", emotionScores.Disgust * 100));
            if (emotionScores.Fear >= 0.1f) sb.Append(String.Format("fear {0:F1}%, ", emotionScores.Fear * 100));
            if (emotionScores.Happiness >= 0.1f) sb.Append(String.Format("happiness {0:F1}%, ", emotionScores.Happiness * 100));
            if (emotionScores.Neutral >= 0.1f) sb.Append(String.Format("neutral {0:F1}%, ", emotionScores.Neutral * 100));
            if (emotionScores.Sadness >= 0.1f) sb.Append(String.Format("sadness {0:F1}%, ", emotionScores.Sadness * 100));
            if (emotionScores.Surprise >= 0.1f) sb.Append(String.Format("surprise {0:F1}%, ", emotionScores.Surprise * 100));

            // Add glasses.
            sb.Append(face.FaceAttributes.Glasses);
            sb.Append(", ");

            // Add hair.
            sb.Append("Hair: ");

            // Display baldness confidence if over 1%.
            if (face.FaceAttributes.Hair.Bald >= 0.01f)
                sb.Append(String.Format("bald {0:F1}% ", face.FaceAttributes.Hair.Bald * 100));

            // Display all hair color attributes over 10%.
            var hairColors = face.FaceAttributes.Hair.HairColor;
            foreach (var hairColor in hairColors)
            {
                if (hairColor.Confidence >= 0.1f)
                {
                    sb.Append(hairColor.Color.ToString());
                    sb.Append(String.Format(" {0:F1}% ", hairColor.Confidence * 100));
                }
            }

            return sb.ToString();
        }
        private int GetNumberOfDetectedFaces()
        {
            var numberOfDetectedFaces = 0;
            if (_faces != null) { numberOfDetectedFaces = _faces.Length; }

            return numberOfDetectedFaces;
        }
        private Face[] GetDetectedFaces()
        {
            var detectedFaces = new Face[0];
            if (_faces != null) { detectedFaces = _faces; }

            return detectedFaces;
        }
        private string[] GetDetectedFacesDescriptions()
        {
            var detectedFacesDescriptions = new string[0];
            if (_faceDescriptions != null) { detectedFacesDescriptions = _faceDescriptions; }

            return detectedFacesDescriptions;
        }
        private void InitializeFaceDescriptionsStringArray()
        {
            var numberOfFaces = _faces.Length;
            _faceDescriptions = new string[numberOfFaces];
        }
    }
}
