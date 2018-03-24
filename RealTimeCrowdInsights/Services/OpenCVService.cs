using OpenCvSharp;
using RealTimeCrowdInsights.Enums;
using RealTimeCrowdInsights.Interfaces;

namespace RealTimeCrowdInsights.Services
{
    public class OpenCVService : IOpenCVService
    {
        private readonly CascadeClassifier _cascadeClassifier;

        public OpenCVService()
        {
            _cascadeClassifier = new CascadeClassifier();
        }

        public CascadeClassifier DefaultFrontalFaceDetector()
        {
            return GetLoadedClassifier(HaarCascade.FrontalFaceAlt2);
        }

        private CascadeClassifier GetLoadedClassifier(HaarCascade haarCascade)
        {
            var result = _cascadeClassifier;

            _cascadeClassifier.Load(GetHaarCascadeDataPath(haarCascade));

            return result;
        }
        private string GetHaarCascadeDataPath(HaarCascade haarCascade)
        {
            var result = string.Empty;

            switch (haarCascade)
            {
                case HaarCascade.Eye:
                    break;
                case HaarCascade.EyeTreeEyeglasses:
                    break;
                case HaarCascade.FrontalCatFace:
                    break;
                case HaarCascade.FrontalCatFaceExtended:
                    break;
                case HaarCascade.FrontalFaceAlt:
                    result = "Data/haarcascade_frontalface_alt.xml";
                    break;
                case HaarCascade.FrontalFaceAlt2:
                    result = "Data/haarcascade_frontalface_alt2.xml";
                    break;
                case HaarCascade.FrontalFaceAltTree:
                    result = "Data/haarcascade_frontalface_alt_tree.xml";
                    break;
                case HaarCascade.FrontalFaceDefault:
                    result = "Data/haarcascade_frontalface_default.xml";
                    break;
                case HaarCascade.FulBody:
                    break;
                case HaarCascade.LeftEye2Splits:
                    break;
                case HaarCascade.LowerBody:
                    break;
                case HaarCascade.ProfileFace:
                    break;
                case HaarCascade.RightEye2Splits:
                    break;
                case HaarCascade.Smile:
                    break;
                case HaarCascade.UpperBody:
                    break;
                default:
                    break;
            }

            return result;
        }

    }
}
