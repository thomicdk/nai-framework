using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Media;
using System.Diagnostics;
using NAI.Client.Calibration;
using NAI.Client;

namespace NAI.UI.Client
{
    /// <summary>
    /// Interaction logic for CalibrationControl.xaml
    /// </summary>
    internal partial class CalibrationUserControl : SurfaceUserControl
    {
        private const int SliderThickness = 30;
        private const int SliderSeperationSpace = 30;

        public double ScreenWidth { get; private set; }
        public double ScreenHeight { get; private set; }

        
        private double _latestLayoutWidth = -1;
        private double _latestLayoutHeight = -1;
        
        private double _maxScreenWidth;
        private double _maxScreenHeight;
        private Contact _ellipseControlContact;
        private Point _lastContactPoint;
        private ClientTagVisualization _parent;

        // Tag postion relative to the origin (0,0) of the TagVisualization
        public Point TagPosition { get; set; }
        public double TagOrientation { get; set; }

        public CalibrationUserControl()
        {
            InitializeComponent();
            //this.MinWidth = 400;
            //this.MinHeight = 400;
        }

        private CalibrationUserControl(ClientTagVisualization parent, bool doDefaultLayout)
        {
            this._parent = parent;
            InitializeComponent();

            //if (parent.BlobRectangle != null && !parent.BlobRectangle.Equals(new Rect()))
            //{
            //    //this.LayoutTransform = new RotateTransform(parent.BlobOrientation);
            //    LayoutControl(parent.BlobRectangle.Width, parent.BlobRectangle.Height);
            //}
            //else 
            if (doDefaultLayout)
            {
                this.MinWidth = 180;
                this.MinHeight = 300;
                LayoutControl(300, 180);
            }
            //Debug.WriteLine("CalibrationUserControl constructor: this.Visualizer == null?: " + (parent.Visualizer == null));
        }

        public CalibrationUserControl(ClientTagVisualization parent) : this(parent, true)
        { }

        internal CalibrationUserControl(ClientTagVisualization parent, CalibrationData existingCalibration) :
            this(parent, false)
        {
            // Nice-To-Have: Indlæs eksisterende calibration her...
            this.MinWidth = 180;
            this.MinHeight = 300;
            LayoutControl(300, 180);
            
            if (existingCalibration != null)
            {
                AdjustWidth(existingCalibration.Width);
                AdjustHeight(existingCalibration.Height);
            }
            
        }


        private void LayoutControl(double blobWidth, double blobHeight)
        {
            _maxScreenWidth = blobWidth + 50;
            _maxScreenHeight = blobHeight + 50;
            double sliderWidth = Math.Max(SliderThickness, (_maxScreenWidth - SliderSeperationSpace) / 2);
            double sliderHeight = Math.Max(SliderThickness, (_maxScreenHeight - SliderSeperationSpace) / 2);
            _latestLayoutWidth = (sliderWidth * 2) + SliderSeperationSpace + SliderThickness*2;
            _latestLayoutHeight = (sliderHeight * 2) + SliderSeperationSpace + SliderThickness*2;
            this.MinWidth = _latestLayoutWidth;
            this.MinHeight = _latestLayoutHeight;

            /*

        }

        private void LayoutControl(double controlWidth, double controlHeight)
        {

            _latestLayoutWidth = controlWidth;
            _latestLayoutHeight = controlHeight;
            double sliderWidth = Math.Max(SliderThickness,((controlWidth - SliderSeperationSpace - SliderThickness) / 2));
            double sliderHeight = Math.Max(SliderThickness,((controlHeight - SliderSeperationSpace - SliderThickness) / 2));
            _maxScreenWidth = (sliderWidth * 2) + SliderSeperationSpace;
            _maxScreenHeight = (sliderHeight * 2) + SliderSeperationSpace;
            */

            // Axis
            XAxis.X1 = SliderThickness;
            XAxis.X2 = XAxis.X1 + _maxScreenWidth;
            XAxis.Y1 = SliderThickness + sliderHeight + (SliderSeperationSpace / 2);
            XAxis.Y2 = XAxis.Y1;
            YAxis.X1 = SliderThickness + sliderWidth + (SliderSeperationSpace / 2);
            YAxis.X2 = YAxis.X1;
            YAxis.Y1 = SliderThickness;
            YAxis.Y2 = YAxis.Y1 + _maxScreenHeight;
            Canvas.SetLeft(Arrows, YAxis.X1 - (Arrows.Width / 2));
            Arrows.Height = (YAxis.Y2 - YAxis.Y1) / 2;

            // ScreenLeft and ScreenRight sliders
            LeftTrack.Width = sliderWidth;
            RightTrack.Width = sliderWidth;
            LeftLine.Y1 = SliderThickness;
            LeftLine.Y2 = LeftLine.Y1 + _maxScreenHeight;
            RightLine.Y1 = SliderThickness;
            RightLine.Y2 = RightLine.Y1 + _maxScreenHeight;
            Canvas.SetLeft(LeftTrack, SliderThickness);
            Canvas.SetLeft(RightTrack, SliderThickness + SliderSeperationSpace + sliderWidth);
            AdjustWidth(_maxScreenWidth/2);

            // ScreenTop and ScreenBottom sliders
            TopTrack.Height = sliderHeight;
            BottomTrack.Height = sliderHeight;
            TopLine.X1 = SliderThickness;
            TopLine.X2 = TopLine.X1 + _maxScreenWidth;
            BottomLine.X1 = SliderThickness;
            BottomLine.X2 = BottomLine.X1 + _maxScreenWidth;
            Canvas.SetTop(TopTrack, SliderThickness);
            Canvas.SetTop(BottomTrack, SliderThickness + SliderSeperationSpace + sliderHeight);
            AdjustHeight(_maxScreenHeight / 2);
        }

        /// <summary>
        /// Called when the either ScreenLeft or ScreenRight sliders are adjusted/moved.
        /// The method moves the two sliders according to the newWidth parameter
        /// </summary>
        /// <param name="newWidth">The new width between the ScreenLeft and ScreenRight sliders</param>
        private void AdjustWidth(double newWidth)
        {
            // Adjust newWidth to boundaries (if neccersary)
            newWidth = Math.Max(SliderSeperationSpace, Math.Min(_maxScreenWidth, newWidth));

            // Move ScreenLeft slider
            double leftThumbPos = Canvas.GetLeft(LeftTrack) + LeftTrack.Width - ((newWidth - SliderSeperationSpace) / 2) -(SliderThickness/2);
            Canvas.SetLeft(LeftThumb, leftThumbPos);
            double newLeftLinePos = leftThumbPos + (LeftThumb.Width / 2);
            LeftLine.X1 = newLeftLinePos;
            LeftLine.X2 = newLeftLinePos;

            // Move ScreenRight slider
            double rightThumbPos = Canvas.GetLeft(RightTrack) + ((newWidth - SliderSeperationSpace) / 2) - (SliderThickness / 2);
            Canvas.SetLeft(RightThumb, rightThumbPos);
            double newRightLinePos = rightThumbPos + (RightThumb.Width / 2);
            RightLine.X1 = newRightLinePos;
            RightLine.X2 = newRightLinePos;

            // Adjust size and move screen area rectangle
            ScreenArea.Width = newWidth;
            Canvas.SetLeft(ScreenArea, leftThumbPos + (SliderThickness/2));

            // Set the public property
            ScreenWidth = newWidth;
        }

        /// <summary>
        /// Called when the either ScreenTop or ScreenBottom sliders are adjusted/moved.
        /// The method moves the two sliders according to the newHeight parameter
        /// </summary>
        /// <param name="newWidth">The new height between the ScreenTop and ScreenBottom sliders</param>
        private void AdjustHeight(double newHeight)
        {
            // Adjust newHeight to boundaries (if neccersary)
            newHeight = Math.Max(SliderSeperationSpace, Math.Min(_maxScreenHeight, newHeight));
            
            // Move ScreenTop slider
            double topThumbPos = Canvas.GetTop(TopTrack) + TopTrack.Height - ((newHeight - SliderSeperationSpace) / 2) - (SliderThickness / 2);
            Canvas.SetTop(TopThumb, topThumbPos);
            double newTopLinePos = topThumbPos + (TopThumb.Height / 2);
            TopLine.Y1 = newTopLinePos;
            TopLine.Y2 = newTopLinePos;

            // Move ScreenBottom slider
            double bottomThumbPos = Canvas.GetTop(BottomTrack) + ((newHeight - SliderSeperationSpace) / 2) - (SliderThickness / 2);
            Canvas.SetTop(BottomThumb, bottomThumbPos);
            double newBottomLinePos = bottomThumbPos + (BottomThumb.Height / 2);
            BottomLine.Y1 = newBottomLinePos;
            BottomLine.Y2 = newBottomLinePos;

            // Adjust size  and move screen area rectangle
            ScreenArea.Height = newHeight;
            Canvas.SetTop(ScreenArea, topThumbPos + (SliderThickness/2));

            // Set the public property
            ScreenHeight = newHeight;
        }

        #region EventHandlers

        private void OnEllipseContactDown(object sender, ContactEventArgs e)
        {
            Debug.WriteLineIf(DebugSettings.DEBUG_CALIBRATION, "OnEllipseContactDown: this.Visualizer == null?: " + (_parent.Visualizer == null));
            // Capture to the ellipse.  
            e.Contact.Capture(sender as Ellipse);

            // Remember this contact if a contact has not been remembered already.  
            // This contact is then used to move the ellipse around.  
            if (_ellipseControlContact == null)
            {
                _ellipseControlContact = e.Contact;

                // Remember where this contact took place.  
                _lastContactPoint = _ellipseControlContact.GetPosition(this.MainCanvas);
            }

            // Mark this event as handled.  
            e.Handled = true;
        }

        private void OnEllipseContactChanged(object sender, ContactEventArgs e)
        {
            Debug.WriteLineIf(DebugSettings.DEBUG_CALIBRATION, "OnEllipseContactChanged: this.Visualizer == null?: " + (_parent.Visualizer == null));
            if (e.Contact == _ellipseControlContact)
            {
                // Get the current position of the contact.  
                Point currentContactPoint = _ellipseControlContact.GetCenterPosition(this.MainCanvas);

                // Get the change between the controlling contact point and
                // the changed contact point.  
                double deltaX = currentContactPoint.X - _lastContactPoint.X;
                double deltaY = currentContactPoint.Y - _lastContactPoint.Y;

                if (sender.Equals(LeftThumb))
                {
                    AdjustWidth(ScreenWidth - deltaX*2);
                }
                else if (sender.Equals(RightThumb))
                {
                    AdjustWidth(ScreenWidth + deltaX * 2);
                }
                else if (sender.Equals(TopThumb))
                {
                    AdjustHeight(ScreenHeight - deltaY * 2);
                }
                else if (sender.Equals(BottomThumb))
                {
                    AdjustHeight(ScreenHeight + deltaY * 2);
                }

                // Forget the old contact point, and remember the new contact point.  
                _lastContactPoint = currentContactPoint;

                // Mark this event as handled.  
                e.Handled = true;
            }
        }

        private void OnEllipseContactLeave(object sender, ContactEventArgs e)
        {
            Debug.WriteLineIf(DebugSettings.DEBUG_CALIBRATION, "OnEllipseContactLeave: this.Visualizer == null?: " + (_parent.Visualizer == null));
            // If this contact is the one that was remembered  
            if (e.Contact == _ellipseControlContact)
            {
                // Forget about this contact.
                _ellipseControlContact = null;
            }

            // Mark this event as handled.  
            e.Handled = true;
        }
        #endregion


        public Vector TagOffsetOrigo(Vector calibrationOffsetInches)
        {
            Vector center = new Vector(_latestLayoutWidth / 2, _latestLayoutHeight / 2);
            Point origoPoint = GetOrigo();
            Vector origo = new Vector(origoPoint.X, origoPoint.Y);
            Vector origoCenterOffset = Vector.Add(center, origo);
            origoCenterOffset.X /= 48;
            origoCenterOffset.Y /= 48;
            //return Vector.Subtract(calibrationOffsetInches, new Vector(0.25,0.25));
            //return calibrationOffsetInches;
            //return Vector.Add(calibrationOffsetInches, origoCenterOffset);
            return new Vector(0,0);
        }

        public Point GetOrigo()
        {
            return new Point(YAxis.X1, XAxis.Y1);

            //Vector offset = new Vector();
            //Point tagPos = new Point();

            //double offsetX = offset.X;
            //double offsetY = offset.Y;
            //if (offsetX > 0) offsetX -= 0.25;
            //if (offsetY > 0) offsetY -= 0.25;
            //double origoX = (offsetX * 48) + tagPos.X;
            //double origoY = (offsetY * 48) + tagPos.Y;
        }

        internal CalibrationData GetCurrentCalibration()
        {
            Point origo = GetOrigo();
            CalibrationData c = new CalibrationData();
            double offsetX = (origo.X - TagPosition.X) / 48;
            double offsetY = (origo.Y - TagPosition.Y) / 48;
            if (offsetX > 0) offsetX += 0.25;
            if (offsetY > 0) offsetY += 0.25;
            c.OffsetInches = new Vector(offsetX, offsetY);
            c.Width = ScreenWidth;
            c.Height = ScreenHeight;
            c.Orientation = TagOrientation;
            Debug.WriteLineIf(DebugSettings.DEBUG_CALIBRATION, c);
            Debug.WriteLineIf(DebugSettings.DEBUG_CALIBRATION, "GetCurrentCalibration: this.Visualizer == null?: " + (_parent.Visualizer == null));
            return c;
        }
    }
}
