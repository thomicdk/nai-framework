using System.Windows;
using System.Windows.Media;
using NAI.UI.Helpers;

namespace NAI.Client.Streaming
{
    internal class StreamingRectangle
    {
        // The bounding box of the screen segment in the global coordinate system
        public Int32Rect BoundingBox { get; set; }
        
        // The Matrix used to transform from the global coordinate system to 
        // the local coordinate system
        public Matrix MatrixTransformGlobal { get; set; }
        
        // The bounding box of the screen segment in the local coordinate system
        public Rect TargetBounds { get; set; }

        public static StreamingRectangle FromVisual(Visual target, GeneralTransform transformGlobal)
        {
            StreamingRectangle result = new StreamingRectangle();
            
            Rect bounds = TransformHelper.GetVisualBoundingBoxGlobal(target);
            result.TargetBounds = VisualTreeHelper.GetDescendantBounds(target);
            result.MatrixTransformGlobal = ((MatrixTransform)transformGlobal).Matrix;
            if (Properties.Settings.Default.SimulatorMode)
            {
                bounds.X += (int)Properties.Settings.Default.SimulatorOriginOffset.X;
                bounds.Y += (int)Properties.Settings.Default.SimulatorOriginOffset.Y;
            }
            result.BoundingBox = new Int32Rect((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height);

            return result;
        }
    }
}
