// SineCurve.cs by Charles Petzold, September 2008
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Petzold.TextOnPath
{
    public class SineCurve : Shape
    {
        // Private fields
        PolyLineSegment polyLineSegment;
        PathGeometry pathGeometry;

        // Dependency properties
        public static readonly DependencyProperty PeriodProperty =
            DependencyProperty.Register("Period", 
                                        typeof(double), 
                                        typeof(SineCurve), 
                                        new FrameworkPropertyMetadata(360.0, OnRedrawPropertyChanged));

        public static readonly DependencyProperty AmplitudeProperty =
            DependencyProperty.Register("Amplitude",
                                        typeof(double),
                                        typeof(SineCurve),
                                        new FrameworkPropertyMetadata(96.0, OnRedrawPropertyChanged));

        public static readonly DependencyProperty PhaseProperty =
            DependencyProperty.Register("Phase",
                                        typeof(double),
                                        typeof(SineCurve),
                                        new FrameworkPropertyMetadata(0.0, OnRedrawPropertyChanged));

        public static readonly DependencyProperty CyclesProperty =
            DependencyProperty.Register("Cycles",
                                        typeof(double),
                                        typeof(SineCurve),
                                        new FrameworkPropertyMetadata(1.0, OnRedrawPropertyChanged));

        public static readonly DependencyProperty OriginProperty =
            DependencyProperty.Register("Origin",
                                        typeof(Point),
                                        typeof(SineCurve),
                                        new FrameworkPropertyMetadata(new Point(0, 96), OnRedrawPropertyChanged));

        private static readonly DependencyPropertyKey PathFigureKey =
            DependencyProperty.RegisterReadOnly("PathFigure",
                                                typeof(PathFigure),
                                                typeof(SineCurve),
                                                new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty PathFigureProperty =
            PathFigureKey.DependencyProperty;

        // Constructor
        public SineCurve()
        {
            polyLineSegment = new PolyLineSegment();
            PathFigure = new PathFigure(new Point(), new PathSegment[] { polyLineSegment }, false);
            pathGeometry = new PathGeometry(new PathFigure[] { PathFigure });
            OnRedrawPropertyChanged(new DependencyPropertyChangedEventArgs());
        }

        // Public CLR Properties
        public double Period
        {
            set { SetValue(PeriodProperty, value); }
            get { return (double)GetValue(PeriodProperty); }
        }

        public double Amplitude
        {
            set { SetValue(AmplitudeProperty, value); }
            get { return (double)GetValue(AmplitudeProperty); }
        }

        public double Phase
        {
            set { SetValue(PhaseProperty, value); }
            get { return (double)GetValue(PhaseProperty); }
        }

        public double Cycles
        {
            set { SetValue(CyclesProperty, value); }
            get { return (double)GetValue(CyclesProperty); }
        }

        public Point Origin
        {
            set { SetValue(OriginProperty, value); }
            get { return (Point)GetValue(OriginProperty); }
        }

        public PathFigure PathFigure
        {
            protected set { SetValue(PathFigureKey, value); }
            get { return (PathFigure)GetValue(PathFigureProperty); }
        }

        // Property-changed handlers
        static void OnRedrawPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as SineCurve).OnRedrawPropertyChanged(args);
        }

        void OnRedrawPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            PathFigure.Segments.Clear();
            polyLineSegment.Points.Clear();

            for (double x = Origin.X; x <= Origin.X + Period * Cycles; x++)
            {
                double degrees = Phase + 360 * x / Period;
                double radians = Math.PI * degrees / 180;
                double y = Origin.Y - Amplitude * Math.Sin(radians);

                if (x == Origin.X)
                    PathFigure.StartPoint = new Point(x, y);
                else
                    polyLineSegment.Points.Add(new Point(x, y));
            }

            PathFigure.Segments.Add(polyLineSegment);
        }

        // Required DefiningGeometry override
        protected override Geometry DefiningGeometry
        {
            get
            {
                return pathGeometry;
            }
        }
    }
}