﻿using OpenCvSharp;

namespace RealTimeFaceInsights.Utils
{
    public class ImageEncodingParameter
    {
        private static ImageEncodingParam[] _jpegParams = { new ImageEncodingParam(ImwriteFlags.JpegQuality, 60) };
        public static ImageEncodingParam[] JpegParams { get { return _jpegParams; } }
    }
}
