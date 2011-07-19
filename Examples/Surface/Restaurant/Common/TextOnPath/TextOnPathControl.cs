// TextOnPathControl.cs by Charles Petzold, September 2008
using System;
// TextOnPathControl.cs by Charles Petzold, September 2008
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Petzold.TextOnPath
{
    public class TextOnPathControl : UserControl
    {
        // Fields
        Panel mainPanel;
        const double FONTSIZE = 100;

        // Dependency properties
        public static readonly DependencyProperty PathFigureProperty =
            DependencyProperty.Register("PathFigure",
                typeof(PathFigure),
                typeof(TextOnPathControl),
                new FrameworkPropertyMetadata(OnPathPropertyChanged));

        public static readonly DependencyProperty TextProperty =
            TextBlock.TextProperty.AddOwner(typeof(TextOnPathControl),
                new FrameworkPropertyMetadata(OnTextPropertyChanged));

        // Properties
        public PathFigure PathFigure
        {
            set { SetValue(PathFigureProperty, value); }
            get { return (PathFigure)GetValue(PathFigureProperty); }
        }

        public string Text
        {
            set { SetValue(TextProperty, value); }
            get { return (string)GetValue(TextProperty); }
        }

        // Constructors
        static TextOnPathControl()
        {
            FontFamilyProperty.OverrideMetadata(typeof(TextOnPathControl),
                new FrameworkPropertyMetadata(OnFontPropertyChanged));
            FontStyleProperty.OverrideMetadata(typeof(TextOnPathControl),
                new FrameworkPropertyMetadata(OnFontPropertyChanged));
            FontWeightProperty.OverrideMetadata(typeof(TextOnPathControl),
                new FrameworkPropertyMetadata(OnFontPropertyChanged));
            FontStretchProperty.OverrideMetadata(typeof(TextOnPathControl),
                new FrameworkPropertyMetadata(OnFontPropertyChanged));
        }

        public TextOnPathControl()
        {
            mainPanel = new Canvas();
            Content = mainPanel;
        }

        // Property-changed handlers
        static void OnFontPropertyChanged(DependencyObject obj,
                                DependencyPropertyChangedEventArgs args)
        {
            (obj as TextOnPathControl).OrientTextOnPath();
        }

        static void OnPathPropertyChanged(DependencyObject obj,
                                DependencyPropertyChangedEventArgs args)
        {
            (obj as TextOnPathControl).OrientTextOnPath();
        }

        static void OnTextPropertyChanged(DependencyObject obj,
                                DependencyPropertyChangedEventArgs args)
        {
            TextOnPathControl ctrl = obj as TextOnPathControl;
            ctrl.mainPanel.Children.Clear();

            if (String.IsNullOrEmpty(ctrl.Text))
                return;

            foreach (Char ch in ctrl.Text)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = ch.ToString();
                textBlock.FontSize = FONTSIZE;
                ctrl.mainPanel.Children.Add(textBlock);
            }
            ctrl.OrientTextOnPath();
        }

        void OrientTextOnPath()
        {
            double pathLength = 
                TextOnPathBase.GetPathFigureLength(PathFigure);
            double textLength = 0;

            foreach (UIElement child in mainPanel.Children)
            {
                child.Measure(new Size(Double.PositiveInfinity,
                                       Double.PositiveInfinity));
                textLength += child.DesiredSize.Width;
            }

            if (pathLength == 0 || textLength == 0)
                return;

            double scalingFactor = pathLength / textLength;
            PathGeometry pathGeometry =
                new PathGeometry(new PathFigure[] { PathFigure });
            double baseline =
                        scalingFactor * FONTSIZE * FontFamily.Baseline;
            double progress = 0;

            foreach (UIElement child in mainPanel.Children)
            {
                double width = scalingFactor * child.DesiredSize.Width;
                progress += width / 2 / pathLength;
                Point point, tangent;

                pathGeometry.GetPointAtFractionLength(progress, 
                                                out point, out tangent);

                TransformGroup transformGroup = new TransformGroup();

                transformGroup.Children.Add(
                    new ScaleTransform(scalingFactor, scalingFactor));
                transformGroup.Children.Add(
                    new RotateTransform(Math.Atan2(tangent.Y, tangent.X)
                        * 180 / Math.PI, width / 2, baseline));
                transformGroup.Children.Add(
                    new TranslateTransform(point.X - width / 2,
                                           point.Y - baseline));

                child.RenderTransform = transformGroup;
                progress += width / 2 / pathLength;
            }
        }
    }
}
