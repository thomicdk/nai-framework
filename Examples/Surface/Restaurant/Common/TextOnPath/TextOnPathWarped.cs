// TextOnPathWarped.cs by Charles Petzold, September 2008
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Petzold.TextOnPath
{
    public class TextOnPathWarped : TextOnPathBase
    {
        // Fields
        Typeface typeface;
        double pathLength;
        double textLength;
        double baseline;
        PathGeometry flattenedTextPathGeometry;
        PathGeometry warpedTextPathGeometry;
        delegate Point ProcessPoint(Point pointSrc);

        // Dependency properties
        public static readonly DependencyProperty ShiftToOriginProperty =
            DependencyProperty.Register("ShiftToOrigin", 
                                        typeof(bool), 
                                        typeof(TextOnPathWarped), 
                                        new FrameworkPropertyMetadata(false, 
                                                                      OnShiftPropertyChanged)); 

        // Constructor
        public TextOnPathWarped()
        {
            typeface = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
        }

        // Properties
        public bool ShiftToOrigin
        {
            set { SetValue(ShiftToOriginProperty, value); }
            get { return (bool)GetValue(ShiftToOriginProperty); }
        }

        // Methods
        protected override void OnPathPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            if (PathFigure == null)
                return;

            pathLength = GetPathFigureLength(PathFigure);
            GenerateWarpedGeometry();
        }

        protected override void OnFontPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            typeface = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
            OnTextPropertyChanged(args);
        }

        protected override void OnForegroundPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            InvalidateVisual();
        }

        protected override void OnTextPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            if (String.IsNullOrEmpty(Text))
            {
                flattenedTextPathGeometry = null;
                return;
            }

            FormattedText formattedText =
                new FormattedText(Text, CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight, typeface, 100, Foreground);

            textLength = formattedText.Width;
            baseline = formattedText.Baseline;
            Geometry formattedTextGeometry = formattedText.BuildGeometry(new Point());
            flattenedTextPathGeometry = PathGeometry.CreateFromGeometry(formattedTextGeometry).GetFlattenedPathGeometry();
            warpedTextPathGeometry = flattenedTextPathGeometry.CloneCurrentValue();
            GenerateWarpedGeometry();
        }

        static void OnShiftPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as TextOnPathWarped).GenerateWarpedGeometry();
        }

        void GenerateWarpedGeometry()
        {
            if (PathFigure == null || flattenedTextPathGeometry == null)
                return;

            if (pathLength == 0 || textLength == 0)
                return;

            WarpAllPoints(warpedTextPathGeometry, flattenedTextPathGeometry);
            InvalidateMeasure();
        }

        void WarpAllPoints(PathGeometry warpedTextPathGeometry, PathGeometry flattenedTextPathGeometry)
        {
            PathGeometry pathGeometry = new PathGeometry(new PathFigure[] { PathFigure });
            double scalingFactor = pathLength / textLength;

            LoopThroughAllFlattenedPathPoints(warpedTextPathGeometry, flattenedTextPathGeometry, delegate(Point pointSrc)
            {
                double fractionLength = Math.Max(0, Math.Min(1, pointSrc.X / textLength));
                double offsetFromBaseline = scalingFactor * (baseline - pointSrc.Y);
                Point pathPoint, pathTangent;

                pathGeometry.GetPointAtFractionLength(fractionLength, out pathPoint, out pathTangent);
                double angle = Math.Atan2(pathTangent.Y, pathTangent.X);

                Point pointDst = new Point();
                pointDst.X = pathPoint.X + offsetFromBaseline * Math.Sin(angle);
                pointDst.Y = pathPoint.Y - offsetFromBaseline * Math.Cos(angle);

                return pointDst;
            });

            if (ShiftToOrigin)
            {
                Rect boundsRect = warpedTextPathGeometry.Bounds;

                LoopThroughAllFlattenedPathPoints(warpedTextPathGeometry, warpedTextPathGeometry, delegate(Point pointSrc)
                {
                    Point pointDst = new Point();
                    pointDst.X = pointSrc.X - boundsRect.Left;
                    pointDst.Y = pointSrc.Y - boundsRect.Top;
                    return pointDst;
                });
            }
        }

        void LoopThroughAllFlattenedPathPoints(PathGeometry pathGeometryDst, PathGeometry pathGeometrySrc, ProcessPoint callback)
        {
            for (int fig = 0; fig < pathGeometrySrc.Figures.Count; fig++)
            {
                PathFigure pathFigureSrc = pathGeometrySrc.Figures[fig];
                PathFigure pathFigureDst = pathGeometryDst.Figures[fig];
                pathFigureDst.StartPoint = callback(pathFigureSrc.StartPoint);

                for (int seg = 0; seg < pathFigureSrc.Segments.Count; seg++)
                {
                    PathSegment pathSegmentSrc = pathFigureSrc.Segments[seg];
                    PathSegment pathSegmentDst = pathFigureDst.Segments[seg];

                    if (pathSegmentSrc is LineSegment)
                    {
                        LineSegment lineSegmentSrc = pathSegmentSrc as LineSegment;
                        LineSegment lineSegmentDst = pathSegmentDst as LineSegment;
                        lineSegmentDst.Point = callback(lineSegmentSrc.Point);
                    }
                    else if (pathSegmentSrc is PolyLineSegment)
                    {
                        PointCollection pointsSrc = (pathSegmentSrc as PolyLineSegment).Points;
                        PointCollection pointsDst = (pathSegmentDst as PolyLineSegment).Points;

                        for (int index = 0; index < pointsSrc.Count; index++)
                        {
                            pointsDst[index] = callback(pointsSrc[index]);
                        }
                    }
                }
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (warpedTextPathGeometry != null)
                return (Size) warpedTextPathGeometry.Bounds.BottomRight;

            return MeasureOverride(availableSize);
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (warpedTextPathGeometry != null)
                dc.DrawGeometry(Foreground, null, warpedTextPathGeometry);
        }
    }
}