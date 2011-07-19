using Microsoft.Surface.Presentation.Controls;
using System.Threading;
using NAI.Communication;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using NAI.Communication.MessageLayer.Messages;

namespace NAI.UI.Client
{
    /// <summary>
    /// Interaction logic for ScreenCaptureRectangle.xaml
    /// </summary>
    internal partial class StreamingRectangleUserControl : SurfaceUserControl
    {
        private ClientTagVisualization _tagVisualization;

        public StreamingRectangleUserControl()
        {
            InitializeComponent();
        }

        public StreamingRectangleUserControl(ClientTagVisualization tagVisualization, double width, double height)
            : this()
        {
            this._tagVisualization = tagVisualization;
            ScreenRectangle.Width = width;
            ScreenRectangle.Height = height;
        }

        public FrameworkElement GetScreenRectangle()
        {
            return ScreenRectangle;
        }

        public void AddPersonalizedView(UIElement view)
        {
            ScreenRectangle.Child = view;
        }

        public void RemovePersonalizedView()
        {
            ScreenRectangle.Child = null;
        }

        public Point? ConvertToPointTagVisualizer(TagVisualizer visualizer, double localRelativeX, double localRelativeY)
        {
            if (visualizer != null)
            {
                Point local = new Point(ScreenRectangle.Width * localRelativeX, ScreenRectangle.Height * localRelativeY);
                GeneralTransform gt = ScreenRectangle.TransformToVisual(visualizer);
                if (gt != null)
                {
                    return gt.Transform(local);
                }
                Debug.WriteLine("ConvertToPointTagVisualizer: GeneralTransform is null");
            }
            Debug.WriteLine("ConvertToPointTagVisualizer: Returning null");
            return null;
        }
    }          
}
