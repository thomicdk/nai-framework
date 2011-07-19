// TextOnPathElement.cs by Charles Petzold, September 2008
// TextOnPathElement.cs by Charles Petzold, September 2008
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Petzold.TextOnPath
{
    public class TextOnPathElement : TextOnPathBase
    {
        Typeface typeface;
        List<FormattedText> formattedChars = new List<FormattedText>();
        double pathLength;
        double textLength;

        public TextOnPathElement()
        {
            typeface = new Typeface(FontFamily, FontStyle,
                                    FontWeight, FontStretch);
        }

        protected override void OnFontPropertyChanged(
                            DependencyPropertyChangedEventArgs args)
        {
            typeface = new Typeface(FontFamily, FontStyle,
                                    FontWeight, FontStretch);
            OnTextPropertyChanged(args);
        }

        protected override void OnForegroundPropertyChanged(
                            DependencyPropertyChangedEventArgs args)
        {
            OnTextPropertyChanged(args);
        }

        protected override void OnTextPropertyChanged(
                            DependencyPropertyChangedEventArgs args)
        {
            formattedChars.Clear();
            textLength = 0;

            foreach (char ch in Text)
            {
                FormattedText formattedText =
                    new FormattedText(ch.ToString(),
                            CultureInfo.CurrentCulture,
                            FlowDirection.LeftToRight, typeface, 100,
                            Foreground);

                formattedChars.Add(formattedText);
                textLength +=
                    formattedText.WidthIncludingTrailingWhitespace;
            }
            InvalidateMeasure();
            InvalidateVisual();
        }

        protected override void OnPathPropertyChanged(
                            DependencyPropertyChangedEventArgs args)
        {
            pathLength = GetPathFigureLength(PathFigure);
            
            InvalidateMeasure();
            InvalidateVisual(); 
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (PathFigure == null)
                return MeasureOverride(availableSize);

            Rect rect = new PathGeometry(
                new PathFigure[] { PathFigure }).Bounds;

            return (Size)rect.BottomRight;
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (pathLength == 0 || textLength == 0)
                return;

            double scalingFactor = pathLength / textLength;
            double progress = 0;
            PathGeometry pathGeometry = 
                new PathGeometry(new PathFigure[] { PathFigure });

            foreach (FormattedText formText in formattedChars)
            {
                double width = scalingFactor * 
                            formText.WidthIncludingTrailingWhitespace;
                double baseline = scalingFactor * formText.Baseline;
                progress += width / 2 / pathLength;
                Point point, tangent;

                pathGeometry.GetPointAtFractionLength(progress, 
                                                out point, out tangent);
                dc.PushTransform(
                    new TranslateTransform(point.X - width / 2, 
                                           point.Y - baseline));
                dc.PushTransform(
                    new RotateTransform(Math.Atan2(tangent.Y, tangent.X)
                            * 180 / Math.PI, width / 2, baseline));
                dc.PushTransform(
                    new ScaleTransform(scalingFactor, scalingFactor));

                dc.DrawText(formText, new Point(0, 0));
                dc.Pop();
                dc.Pop();
                dc.Pop();

                progress += width / 2 / pathLength;
            }
        }
    }
}
