using System;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using NAI.Properties;
using NAI.UI.Client;
using NAI.Communication.MessageLayer;


namespace NAI.Client.Streaming
{
    class StreamingProcessor
    {
        public bool KeepRunning { get; set; }
        public int FramesPerSecond {get; set; }

        private BitmapSource _bs;
        private ClientTagVisualization _tagVisualization;
        private Visual _screenRectangle;

        private StreamingRectangle _currentStreamRectangle;
        static readonly object _streamRectangleLock = new object();

        private IMessageLayerOutgoing _clientCommunication;
        
        private System.Windows.Forms.Screen _screen;

        public StreamingProcessor(ClientTagVisualization tagVisualization, IMessageLayerOutgoing clientCommunication)
        {
            this._screenRectangle = ((StreamingRectangleUserControl)tagVisualization.MyContent).ScreenRectangle;
            this._tagVisualization = tagVisualization;
            this._clientCommunication = clientCommunication;
            FramesPerSecond = RuntimeSettings.StreamingFrameRate;
            _screen = RuntimeSettings.TabletopScreen;
        }

        public void Run()
        {
            Debug.WriteLineIf(DebugSettings.DEBUG_STREAMING, string.Format("ScreenProcessor thread running @ {0} FPS", FramesPerSecond));
            if (_clientCommunication != null)
            {
                _clientCommunication.SendPictureStreamStart();
                KeepRunning = true;
                DateTime start, end;
                while (KeepRunning)
                {
                    start = DateTime.Now;
                    try
                    {
                        lock (_streamRectangleLock)
                        {
                            if (_currentStreamRectangle != null)
                            {
                                _bs = CaptureNewScreenshot(_currentStreamRectangle.BoundingBox);
                            }
                        }

                        KeepRunning = (bool)_screenRectangle.Dispatcher.Invoke(new UpdateScreenSegmentDelegate(UpdateScreenSegment));
                    }
                    catch (TargetInvocationException) { }
                    end = DateTime.Now;
                    int sleepTime = (int)((1000 / FramesPerSecond) - end.Subtract(start).TotalMilliseconds);
                    sleepTime = Math.Max(5, sleepTime); // Sleep at least 5 ms
                    Thread.Sleep(sleepTime);
                }
                _clientCommunication.SendPictureStreamStop();
            }
            _tagVisualization.UpdateUI();
            Debug.WriteLineIf(DebugSettings.DEBUG_STREAMING, string.Format("ScreenProcessor thread stopped!")); 
        }

        private delegate bool UpdateScreenSegmentDelegate();
        private bool UpdateScreenSegment()
        {
            if (_tagVisualization.Visualizer == null) return false;
            try
            {
                GeneralTransform transform = _screenRectangle.TransformToAncestor(_tagVisualization.Visualizer);
                //GeneralTransform transform = _tagVisualization.Visualizer.TransformToDescendant(_screenRectangle);
                if (transform != null)
                {
                    StreamingRectangle newInfo = StreamingRectangle.FromVisual(_screenRectangle, transform);
                    if (newInfo.BoundingBox.Equals(Int32Rect.Empty)) return false;

                    this._currentStreamRectangle = newInfo;
                    lock (_streamRectangleLock)
                    {
                        _bs = ProcessScreenshot(newInfo);
                        if (_clientCommunication != null && _bs != null)
                        {
                            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                            MemoryStream memStream = new MemoryStream();
                            encoder.Frames.Add(BitmapFrame.Create(_bs));
                            encoder.Save(memStream);
                            encoder.Frames.Clear();
                            _clientCommunication.SendFrame(memStream.ToArray());
                            _bs = null;
                        }
                    }
                }
                return transform != null;
            }
            catch (InvalidOperationException) 
            {
                // Thrown when _tagVisualizer is not an ancestor to _screenRectangle
                return false;
            }
        }

        private BitmapSource ProcessScreenshot(StreamingRectangle segmentInfo)
        {
            // Put the newest screenshot in an image Visual
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Stretch = Stretch.None;
            image.Source = _bs;

            // Setup the necessary transformations (rotate and translate)
            Matrix tMatrixGlobal = segmentInfo.MatrixTransformGlobal;
            Matrix rotateMatrix = new Matrix(tMatrixGlobal.M11, tMatrixGlobal.M12, tMatrixGlobal.M21, tMatrixGlobal.M22, (segmentInfo.BoundingBox.Width / 2), (segmentInfo.BoundingBox.Height / 2));
            rotateMatrix.Invert();
            MatrixTransform rotateTransform = new MatrixTransform(rotateMatrix);
            TranslateTransform translateTransform = new TranslateTransform((segmentInfo.TargetBounds.Width / 2), (segmentInfo.TargetBounds.Height / 2));
            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(rotateTransform);
            transformGroup.Children.Add(translateTransform);
            image.RenderTransform = transformGroup;

            // Force the image to be arranged/layed out by adding it to a canvas
            Canvas container = new Canvas();
            container.Children.Add(image);
            container.Arrange(new Rect(0, 0, segmentInfo.TargetBounds.Width, segmentInfo.TargetBounds.Height));
            
            // Put the image in a bitmap and render it
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)segmentInfo.TargetBounds.Width, (int)segmentInfo.TargetBounds.Height, 96, 96, PixelFormats.Default);
            renderTargetBitmap.Render(image);

            // Clean up before return (to avoid memory leakage)
            container.Children.Remove(image);
            image.RenderTransform = null;
            
            return renderTargetBitmap;
        }

        private BitmapSizeOptions sizeOptions = BitmapSizeOptions.FromEmptyOptions();

        private BitmapSource CaptureNewScreenshot(Int32Rect boundingBox)
        {
            Bitmap bmpScreenshot = new Bitmap(boundingBox.Width, boundingBox.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(_screen.Bounds.X + boundingBox.X, _screen.Bounds.Y + boundingBox.Y, 0, 0, new System.Drawing.Size(boundingBox.Width, boundingBox.Height), CopyPixelOperation.SourceCopy);
            IntPtr hBitmap = bmpScreenshot.GetHbitmap();
            BitmapSource result = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, sizeOptions);
            result.Freeze();
            DeleteObject(hBitmap);
            return result;
        }


        #region WINAPI DLL Imports

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        #endregion

    }
}
