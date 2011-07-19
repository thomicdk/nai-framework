using System.Windows;
using System.Windows.Media;
using Microsoft.Surface.Presentation.Controls;
using System;

namespace NAI.UI.Helpers
{
    public class TransformHelper
    {
        private static SurfaceWindow _surfaceWindow;

        // Transform in relation to the root SurfaceWindow
        public static GeneralTransform GetTransformGlobal(Visual visual)
        {
            SurfaceWindow window = GetSurfaceWindow(visual);
            if (window != null)
            {
                try
                {
                    return visual.TransformToAncestor(window);
                }
                catch (InvalidOperationException)
                {
                    return null;
                }
            }
            return null;
            //return visual.TransformToAncestor(Application.Current.MainWindow);
        }

        public static Rect GetVisualBoundingBoxGlobal(Visual visual)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(visual);
            GeneralTransform gt = GetTransformGlobal(visual);
            if (gt != null)
            {
                return gt.TransformBounds(bounds);
            }
            return new Rect();
        }

        // Find the root SurfaceWindow by recrusively search upwards
        // in the visual tree
        private static SurfaceWindow GetSurfaceWindow(DependencyObject target)
        {
            if (_surfaceWindow != null) return _surfaceWindow;
            else if (target == null) return null;
            else if (target is SurfaceWindow)
            {
                _surfaceWindow = target as SurfaceWindow;
                return _surfaceWindow;
            }
            else return GetSurfaceWindow(VisualTreeHelper.GetParent(target));
        }

    }
}
