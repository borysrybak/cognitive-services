using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FaceAPITutorial.Services
{
    public class MouseBehaviourService
    {
        private readonly FaceAPIService _faceAPIService = FaceAPIService.Instance;

        private static MouseBehaviourService _instance;
        public static MouseBehaviourService Instance
        {
            get
            {
                return _instance ?? (_instance = new MouseBehaviourService());
            }
        }

        /// <summary>
        /// Showing face description if mouse position is over a face rectangle.
        /// </summary>
        /// <param name="faces"></param>
        /// <param name="faceDescriptions"></param>
        /// <param name="resizeFactor"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public string FaceDescriptionFromMousePosition(MouseMovePayload param)
        {
            return GetFaceDescriptionFromMousePosition(param);
        }

        private string GetFaceDescriptionFromMousePosition(MouseMovePayload param)
        {
            var result = string.Empty;

            var faces = _faceAPIService.DetectedFaces();
            var faceDescriptions = _faceAPIService.DetectedFacesDescriptions();
            var resizeFactor = _faceAPIService.GetResizeFactor();

            try
            {
                // If the REST call has not completed, return from this method.
                if (faces != null)
                {
                    var FacePhotoImage = (Image)param.Sender;

                    // Find the mouse position relative to the image.
                    var mouseXY = param.Args.GetPosition(FacePhotoImage);

                    var imageSource = FacePhotoImage.Source;
                    var bitmapSource = (BitmapSource)imageSource;

                    // Scale adjustment between the actual size and displayed size.
                    var scale = FacePhotoImage.ActualWidth / (bitmapSource.PixelWidth / resizeFactor);

                    // Check if this mouse position is over a face rectangle.
                    var mouseOverFace = false;

                    for (var i = 0; i < faces.Length; ++i)
                    {
                        var fr = faces[i].FaceRectangle;
                        var left = fr.Left * scale;
                        var top = fr.Top * scale;
                        var width = fr.Width * scale;
                        var height = fr.Height * scale;

                        // Display the face description for this face if the mouse is over this face rectangle.
                        if (mouseXY.X >= left && mouseXY.X <= left + width && mouseXY.Y >= top && mouseXY.Y <= top + height)
                        {
                            result = faceDescriptions[i];
                            mouseOverFace = true;
                            break;
                        }
                    }

                    // If the mouse is not over a face rectangle.
                    if (!mouseOverFace)
                        result = "Place the mouse pointer over a face to see the face description.";
                }
            }
            // Catch and display other errors.
            catch (Exception e)
            {
                var message = e.Message;
            }

            return result;
        }
    }

    public class MouseBehaviour
    {
        public static readonly DependencyProperty MouseMoveCommandProperty =
            DependencyProperty.RegisterAttached("MouseMoveCommand", typeof(ICommand), typeof(MouseBehaviour), new FrameworkPropertyMetadata(new PropertyChangedCallback(MouseMoveCommandChanged)));

        private static void MouseMoveCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)d;
            element.MouseMove += new MouseEventHandler(ElementMouseMove);
        }

        static void ElementMouseMove(object sender, MouseEventArgs e)
        {
            var mouseMovePayload = new MouseMovePayload();
            mouseMovePayload.Sender = sender;
            mouseMovePayload.Args = e;
            var element = (FrameworkElement)sender;
            var command = GetMouseMoveCommand(element);
            command.Execute(mouseMovePayload);
        }

        public static void SetMouseMoveCommand(UIElement element, ICommand value)
        {
            element.SetValue(MouseMoveCommandProperty, value);
        }

        public static ICommand GetMouseMoveCommand(UIElement element)
        {
            return (ICommand)element.GetValue(MouseMoveCommandProperty);
        }
    }

    public class MouseMovePayload
    {
        public object Sender { get; set; }
        public MouseEventArgs Args { get; set; }
    }
}
