using FaceAPITutorial.Services;
using FaceAPITutorial.Utils;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FaceAPITutorial
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly FaceAPIService _faceAPIService = FaceAPIService.Instance;
        private readonly MouseBehaviourService _mouseBehaviourService = MouseBehaviourService.Instance;

        private string _windowTitle;
        public string WindowTitle
        {
            get { return _windowTitle; }
            set
            {
                _windowTitle = value;
                OnPropertyRaised("WindowTitle");
            }
        }

        private BitmapImage _facePhoto;
        public BitmapImage FacePhoto
        {
            get { return _facePhoto; }
            set
            {
                _facePhoto = value;
                OnPropertyRaised("FacePhoto");
            }
        }

        /// <summary>
        /// Displays the image and calls Detect Faces. 
        /// </summary>
        private ICommand _browseImageCommand;
        public ICommand BrowseImageCommand
        {
            get { return _browseImageCommand ?? (_browseImageCommand = new CommandHandler(param => ExecuteBrowseImage(param), true)); }
        }

        /// <summary>
        /// Displays the face description when the mouse is over a face rectangle. 
        /// </summary>
        private ICommand _mouseMoveCommand;
        public ICommand MouseMoveCommand
        {
            get
            {
                return _mouseMoveCommand ?? (_mouseMoveCommand = new CommandHandler(param => ExecuteMouseMove(param), true));
            }
            set { _mouseMoveCommand = value; }
        }

        private string _faceDescriptionStatusBar;
        public string FaceDescriptionStatusBar
        {
            get { return _faceDescriptionStatusBar; }
            set
            {
                _faceDescriptionStatusBar = value;
                OnPropertyRaised("FaceDescriptionStatusBar");
            }
        }
        
        //TODO: Refactor needed. Null checking, Encapsulation.
        private async void ExecuteBrowseImage(object param)
        {
            // Get the image file to scan from the user.
            var openDlg = new Microsoft.Win32.OpenFileDialog();
            openDlg.Filter = "JPEG Image(*.jpg)|*.jpg";
            bool? result = openDlg.ShowDialog();
            if (!(bool)result) return;

            // Display the image file.
            var filePath = openDlg.FileName;
            var fileUri = new Uri(filePath);
            var bitmapSource = new BitmapImage();
            bitmapSource.BeginInit();
            bitmapSource.CacheOption = BitmapCacheOption.None;
            bitmapSource.UriSource = fileUri;
            bitmapSource.EndInit();
            FacePhoto = bitmapSource;

            // Detect any faces in the image.
            WindowTitle = "Detecting...";
            await _faceAPIService.UploadAndDetectFaces(filePath);
            var numberOfFaces = _faceAPIService.NumberOfDetectedFaces();
            WindowTitle = String.Format("Detection Finished. {0} face(s) detected", numberOfFaces);

            if (numberOfFaces > 0)
            {
                // Prepare to draw rectangles around the faces.
                var visual = new DrawingVisual();
                var drawingContext = visual.RenderOpen();
                drawingContext.DrawImage(bitmapSource, new Rect(0, 0, bitmapSource.Width, bitmapSource.Height));
                var dpi = bitmapSource.DpiX;
                _faceAPIService.SetResizeFactor(96 / dpi);
                var resizeFactor = _faceAPIService.GetResizeFactor();
                var faces = _faceAPIService.DetectedFaces();
                var faceDescriptions = _faceAPIService.DetectedFacesDescriptions();

                for (int i = 0; i < numberOfFaces; ++i)
                {
                    var face = faces[i];

                    // Draw a rectangle on the face.
                    drawingContext.DrawRectangle(
                        Brushes.Transparent,
                        new Pen(Brushes.Red, 2),
                        new Rect(
                            face.FaceRectangle.Left * resizeFactor,
                            face.FaceRectangle.Top * resizeFactor,
                            face.FaceRectangle.Width * resizeFactor,
                            face.FaceRectangle.Height * resizeFactor
                            )
                    );

                    // Store the face description.
                    faceDescriptions[i] = _faceAPIService.FaceDescription(face);
                }

                drawingContext.Close();

                // Display the image with the rectangle around the face.
                var faceWithRectBitmap = new RenderTargetBitmap(
                    (int)(bitmapSource.PixelWidth * resizeFactor),
                    (int)(bitmapSource.PixelHeight * resizeFactor),
                    96,
                    96,
                    PixelFormats.Pbgra32);

                faceWithRectBitmap.Render(visual);

                var bitmapImage = new BitmapImage();
                var bitmapEncoder = new JpegBitmapEncoder();
                bitmapEncoder.Frames.Add(BitmapFrame.Create(faceWithRectBitmap));

                using (var stream = new MemoryStream())
                {
                    bitmapEncoder.Save(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                }


                FacePhoto = new BitmapImage();
                FacePhoto = bitmapImage;

                // Set the status bar text.
                FaceDescriptionStatusBar = "Place the mouse pointer over a face to see the face description.";
            }
        }
        private void ExecuteMouseMove(object param)
        {
            FaceDescriptionStatusBar = _mouseBehaviourService.FaceDescriptionFromMousePosition((MouseMovePayload)param);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyRaised(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
