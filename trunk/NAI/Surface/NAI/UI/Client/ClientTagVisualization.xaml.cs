using System;
using System.Windows;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Media;
using System.Diagnostics;
using NAI.UI.Events;
using System.Collections.Generic;
using System.Windows.Shapes;
using NAI.Client;
using NAI.Communication.MessageLayer.Messages.Incoming;
using NAI.Client.Calibration;
using NAI.Client.Pairing;
using NAI.Client.Streaming;
using NAI.Storage;
using NAI.UI.Controls;

namespace NAI.UI.Client
{
    /// <summary>
    /// Interaction logic for ClientTagVisualization.xaml
    /// </summary>
    internal partial class ClientTagVisualization : TagVisualization, IPersonalizedView
    {

        internal ClientSession ClientSession { get; set; }
        
        /// <summary>
        /// The current content of the visualization.
        /// </summary>
        public UIElement MyContent { get; private set; }

        #region Client Properties 

        private ClientIdentity ClientId
        {
            get
            {
                if (this.ClientSession != null)
                {
                    return ClientSession.ClientId;
                }
                return null;
            }
        }

        internal CalibrationData ClientCalibration
        {
            get 
            {
                return ClientDataStorage.Instance.GetClientCalibration(this.VisualizedTag);
            }
            set 
            {
                ClientDataStorage.Instance.AddCalibration(this.VisualizedTag, value);
            }
        }
        #endregion

        public ClientTagVisualization()
        {
            InitializeComponent();
        }

        private void MyTagVisualization_Loaded(object sender, RoutedEventArgs e)
        {
            this.ClientSession = ClientSessionsController.Instance.GetClient(this.VisualizedTag);
            if (this.ClientSession != null)
            {
                //this.ClientSession.Visualization = this;
                if (this.ClientSession.State is PairedState)
                {
                    ((PairedState)ClientSession.State).OnGotTag(this);
                }

            }
            UpdateUI();
        }

        public void UpdateUI()
        {
            this.Dispatcher.Invoke(new UpdateUIDelegate(UpdateUISecure));
        }

        private delegate void UpdateUIDelegate();
        
        /// <summary>
        /// Should only be invoked by the UI thread.
        /// Any other thread should use the public method UpdateUI()
        /// </summary>
        private void UpdateUISecure()
        {
            if (this.Visualizer == null)
            {
                return;
            }
            
            // Some cleanup
            this.PhysicalCenterOffsetFromTag = new Vector();
            this.OrientationOffsetFromTag = 0;

            Type myState = typeof(PairingState); // Assume not paired
            if (ClientSession != null)
            {
                myState = this.ClientSession.State.GetType();
            }

            if (myState.Equals(typeof(PairingState)))
            {
                MyContent = new PairingUserControl(this);
            }
            else if (myState.Equals(typeof(CalibrationState)))
            {
                Debug.WriteLineIf(DebugSettings.DEBUG_CALIBRATION, "Changing UI to Calibration");
                CalibrationData calibration = this.ClientCalibration;
                if (calibration != null)
                {
                    // Load calibration user control with existing calibration
                    CalibrationUserControl control = new CalibrationUserControl(this, calibration);
                    this.PhysicalCenterOffsetFromTag = control.TagOffsetOrigo(calibration.OffsetInches);
                    this.OrientationOffsetFromTag = -calibration.Orientation;
                    MyContent = control;
                }
                else
                {
                    MyContent = new CalibrationUserControl(this);
                }
            }
            else if (myState.Equals(typeof(StreamingState)))
            {
                if (ClientCalibration != null)
                {
                    this.PhysicalCenterOffsetFromTag = ClientCalibration.OffsetInches;
                    this.OrientationOffsetFromTag = -ClientCalibration.Orientation;
                    StreamingRectangleUserControl content = new StreamingRectangleUserControl(this, ClientCalibration.Width, ClientCalibration.Height);
                    MyContent = content;
                    ((StreamingState)this.ClientSession.State).StartStreaming();
                }
                else
                {
                    Debug.WriteLineIf(DebugSettings.DEBUG_CALIBRATION, "StreamingState: Calibration not found, changing state to Calibration");
                    CalibrationState.SetAsState(this.ClientSession, this);
                    return;
                }
            }
            else // Authentication state...
            {
                MyContent = null;
            }

            this.ContentContainer.Children.Clear();
            if (MyContent != null)
            {
                
                this.ContentContainer.Children.Add(MyContent);
                //this.ContentContainer.InvalidateVisual();
                //this.ContentContainer.UpdateLayout();
                this.InvalidateVisual();
                this.UpdateLayout();
                if (ClientSession != null && ClientSession.State is StreamingState)
                {
                    // Perform hit testing to raise LensOver events
                    HitTestArea();
                }
                else if (MyContent is CalibrationUserControl)
                {
                    CalibrationUserControl calibrationUserControl = MyContent as CalibrationUserControl;
                    calibrationUserControl.TagPosition = Visualizer.TranslatePoint(this.Center, MyContent);
                }
            }
        }

 
        /// <summary>
        /// Method called by PairingState when a match is found. The ClientSession that the correct Pincode was
        /// recieved from is passed along.
        /// </summary>
        /// <param name="socket"></param>
        internal void AssociateClient(ClientSession clientSession)
        {
            Debug.WriteLineIf(DebugSettings.DEBUG_COMMUNICATION, "ClientStateTagVisualization.AssociateClientIdentity(ClientSession)");
            this.ClientSession = clientSession;
            this.ClientSession.Tag = this.VisualizedTag;
            this.ClientSession.ClientId.PersonalizedView = this;
            Debug.WriteLineIf(DebugSettings.DEBUG_EVENTS, "Rasing IdentifiedPersonArrivedEvent");
            this.Dispatcher.Invoke((Action)delegate()
            {
                this.Visualizer.RaiseEvent(new RoutedIdentifiedEventArgs(IdentifiedEvents.IdentifiedPersonArrivedEvent, clientSession.ClientId, this));
            });
            UpdateUI();
        }


        public void CalibrationAccepted()
        {
            this.Dispatcher.Invoke((Action)delegate() {
                Debug.WriteLineIf(DebugSettings.DEBUG_CALIBRATION, "Received CalibrationAcceptedMessage");
                ClientCalibration = ((CalibrationUserControl)MyContent).GetCurrentCalibration();
                if (ClientSession.State is CalibrationState)
                {
                    ((CalibrationState)ClientSession.State).OnCalibrationSaved();
                }
            });
        }

        internal void TouchEvent(TouchEventMessage message)
        {
            if (MyContent is StreamingRectangleUserControl)
            {
                this.Dispatcher.Invoke(new HandleTouchEventMessageDelegate(HandleTouchEventMessage), message as TouchEventMessage);
            }
        }


        private delegate void HandleTouchEventMessageDelegate(TouchEventMessage message);
        private void HandleTouchEventMessage(TouchEventMessage message)
        {
            Debug.WriteLineIf(DebugSettings.DEBUG_EVENTS, "Message: " + message.GetType().Name);
            Debug.WriteLineIf(DebugSettings.DEBUG_EVENTS, "X: " + message.X.ToString() + " Y: " + message.Y.ToString());
            Point? hitPoint = ((StreamingRectangleUserControl)MyContent).ConvertToPointTagVisualizer(this.Visualizer, message.X, message.Y);
            if (hitPoint != null)
            {
                Debug.WriteLineIf(DebugSettings.DEBUG_EVENTS, "Hitpoint != null " + hitPoint.ToString());
                List<HitTestResult> hitTestResults = HitTest((Point)hitPoint);

                foreach (HitTestResult hitTestResult in hitTestResults)
                {
                    if (hitTestResult != null && hitTestResult.VisualHit != null && hitTestResult.VisualHit is UIElement)
                    {
                        UIElement visualHit = hitTestResult.VisualHit as UIElement;
                        Debug.WriteLineIf(DebugSettings.DEBUG_EVENTS, "EVENT RISING!!!! on " + visualHit.GetType().Name + ". Name: " + (visualHit is FrameworkElement? ((FrameworkElement)visualHit).Name : "?"));
                        RaiseIdentifiedTouchEvent(message, visualHit, hitPoint);
                    }
                }
            }
        }


        /// <summary>
        /// Raise the Tunnel/Bubbling event pair 
        /// </summary>
        /// <param name="target">The UIElement to raise the event pair on</param>
        /// <param name="e">The Event args to follow the event</param>
        /// <param name="bubblingEvent">The Bubble event to raise after the Tunnel event</param>
        private void RaiseEventPair(UIElement target, RoutedEvent bubblingEvent, RoutedIdentifiedEventArgs e)
        {
            Debug.WriteLineIf(DebugSettings.DEBUG_EVENTS, "Raising "+e.RoutedEvent+"  on " + target + ". Name: " + (target is FrameworkElement ? ((FrameworkElement)target).Name : "?"));
            target.RaiseEvent(e);
            e.RoutedEvent = bubblingEvent;
            target.RaiseEvent(e);
        }

        private UIElement _previousMoveTargetElement = null;
        private void RaiseIdentifiedTouchEvent(TouchEventMessage message, UIElement visualHit, Point? hitPoint)
        {
            if (message is TouchDownMessage)
            {
                RoutedIdentifiedTouchEventArgs args = new RoutedIdentifiedTouchEventArgs(IdentifiedEvents.PreviewIdentifiedTouchDownEvent, ClientId, this, (Point)hitPoint);
                RaiseEventPair(visualHit, IdentifiedEvents.IdentifiedTouchDownEvent, args);
                _previousMoveTargetElement = visualHit;
            }
            else if (message is TouchMoveMessage)
            {

                if (_previousMoveTargetElement != visualHit)
                {
                    if (!UIElementsRelated(_previousMoveTargetElement, visualHit))
                    {
                        if (_previousMoveTargetElement != null)
                        {
                            RaiseEventPair(_previousMoveTargetElement, IdentifiedEvents.IdentifiedTouchLeaveEvent, new RoutedIdentifiedTouchEventArgs(IdentifiedEvents.PreviewIdentifiedTouchLeaveEvent, ClientId, this, (Point)hitPoint));
                        }
                        if (visualHit != null)
                        {
                            RaiseEventPair(visualHit, IdentifiedEvents.IdentifiedTouchEnterEvent, new RoutedIdentifiedTouchEventArgs(IdentifiedEvents.PreviewIdentifiedTouchEnterEvent, ClientId, this, (Point)hitPoint));
                        }
                    }
                    else
                    {
                        RaiseEventPair(visualHit, IdentifiedEvents.IdentifiedTouchMoveEvent, new RoutedIdentifiedTouchEventArgs(IdentifiedEvents.PreviewIdentifiedTouchMoveEvent, ClientId, this, (Point)hitPoint));
                    }
                    _previousMoveTargetElement = visualHit;
                }
                else
                {
                    RaiseEventPair(visualHit, IdentifiedEvents.IdentifiedTouchMoveEvent, new RoutedIdentifiedTouchEventArgs(IdentifiedEvents.PreviewIdentifiedTouchMoveEvent, ClientId, this, (Point)hitPoint));
                }
            }
            else if (message is TouchUpMessage)
            {
                RaiseEventPair(visualHit, IdentifiedEvents.IdentifiedTouchUpEvent, new RoutedIdentifiedTouchEventArgs(IdentifiedEvents.PreviewIdentifiedTouchUpEvent, ClientId, this, (Point)hitPoint));
                _previousMoveTargetElement = null;
            }
        }

        private bool UIElementsRelated(UIElement e1, UIElement e2)
        {
            DependencyObject parentE1 = ((IdentifiedInteractionArea)this.Visualizer).GetRegisteredIdentifiedAncestor(e1);
            DependencyObject parentE2 = ((IdentifiedInteractionArea)this.Visualizer).GetRegisteredIdentifiedAncestor(e2);
            
            if (parentE1 == null || parentE2 == null) return false;

            Debug.WriteLineIf(DebugSettings.DEBUG_EVENTS, (e1 != null? e1.ToString() : "null") + " parent: " + (parentE1 != null? parentE1.ToString() : "null"));            
            Debug.WriteLineIf(DebugSettings.DEBUG_EVENTS, (e2 != null? e2.ToString() : "null") + " parent: " + (parentE2 != null? parentE2.ToString() : "null"));
            
            if (parentE1.Equals(parentE2))
                return true;
            return false;
        }

        #region Tag Event handlers 

        /// <summary>
        /// Keep track of the registered identified UIElements this tag visualization is currently hovering
        /// </summary>
        private HashSet<UIElement> _lensOverElements = new HashSet<UIElement>();

        private void TagVisualization_Moved(object sender, TagVisualizerEventArgs e)
        {
            if (MyContent is StreamingRectangleUserControl)
            {
                HitTestArea();
            }
        }

        private void TagVisualization_LostTag(object sender, RoutedEventArgs e)
        {
            // Raise HoverOut events
            foreach (UIElement element in _lensOverElements)
            {
                RaiseEventPair(element, IdentifiedEvents.IdentifiedHoverOutEvent, new RoutedIdentifiedHoverEventArgs(IdentifiedEvents.PreviewIdentifiedHoverOutEvent, ClientId, this, null));
                Debug.WriteLineIf(DebugSettings.DEBUG_HITTEST || DebugSettings.DEBUG_EVENTS, "Raising HoverOutEvent on " + element);
            }

            // Cleanup
            if (ClientSession != null && ClientSession.State is PairedState)
            {
                ((PairedState)ClientSession.State).OnLostTag();
            }
            else // Pairing State
            {
                ClientSessionsController.Instance.UnregisterPairingCodes(this);
            }
            this.ClientSession = null;

        }
        
        #endregion

        #region Hit Testing

        #region Hit Testing - Point

        private List<HitTestResult> _pointHitTestResults = new List<HitTestResult>();
        private List<HitTestResult> HitTest(Point point)
        {
            //Debug.WriteLine("=== Hit Test start ===");
            _pointHitTestResults.Clear();
            VisualTreeHelper.HitTest(this.Visualizer,
                new HitTestFilterCallback(PointHitTestFilterCallback),
                new HitTestResultCallback(PointHitTestResultCallback),
                new PointHitTestParameters(point));

            //Debug.WriteLine("=== Hit Test end ===" + Environment.NewLine);
            return _pointHitTestResults;
        }

        private HitTestFilterBehavior PointHitTestFilterCallback(DependencyObject o)
        {
            //Debug.Write(" Filter > " + o.ToString());
            //FrameworkElement contr = ((FrameworkElement)o);
            //if (contr != null)
            //    Debug.WriteLine("    Name: " + contr.Name);
            //else
            //    Debug.WriteLine("");

            //return HitTestFilterBehavior.Continue;   

            if (MyContent is StreamingRectangleUserControl)
            {
                if (((StreamingRectangleUserControl)MyContent).ScreenRectangle.Equals(o))
                {
                    //Debug.WriteIf(DebugSettings.DEBUG_EVENTS, " > Continue(SkipSelfAndChildren):  " + o.ToString());
                    //if (o is UIElement) Debug.WriteLineIf(DebugSettings.DEBUG_EVENTS, "  : Z: " + Canvas.GetZIndex((UIElement)o));
                    //else Debug.WriteLineIf(DebugSettings.DEBUG_EVENTS, "");
                    //return HitTestFilterBehavior.ContinueSkipSelfAndChildren;
                    return HitTestFilterBehavior.ContinueSkipSelf;
                }
                // EXTREME HACK - To use ScatterViewPanel
                else if (o is Rectangle && ((Rectangle)o).Name.Equals("Sheen"))
                {
                    return HitTestFilterBehavior.ContinueSkipSelf;
                }

                //else if (o is SurfaceButton)
                //{
                //((SurfaceButton)o).RaiseEvent(new SecureRoutedEventArgs(RestrictedTagvisualizer.PreviewSecureContactDownEvent, _clientId, new Point(11, 11)));
                //Debug.WriteLine("Event raised on SurfaceButton!!");
                //}
                else
                {
                    //Debug.WriteIf(DebugSettings.DEBUG_EVENTS, " > Continue:  " + o.ToString());
                    //if (o is UIElement) Debug.WriteLineIf(DebugSettings.DEBUG_EVENTS, "  : Z: " + Canvas.GetZIndex((UIElement)o));
                    //else Debug.WriteLineIf(DebugSettings.DEBUG_EVENTS, "");
                    return HitTestFilterBehavior.Continue;
                }
            }

            return HitTestFilterBehavior.Stop;
            //return HitTestFilterBehavior.Continue;
        }

        private HitTestResultBehavior PointHitTestResultCallback(HitTestResult result)
        {
            _pointHitTestResults.Add(result);

            //Debug.WriteLineIf(DebugSettings.DEBUG_EVENTS, "## HitTestResult: " + result.VisualHit.ToString());
            //FrameworkElement contr = ((FrameworkElement)result.VisualHit);
            //if (contr != null)
            //    Debug.WriteLine("    Name: " + contr.Name);
            //else
            //    Debug.WriteLine("");

            //return HitTestResultBehavior.Continue;

            return HitTestResultBehavior.Stop; // Stop at first hit result!
        }
        #endregion

        #region Hit Testing - Area

        public RectangleGeometry GetHitTestRectangleGeometry()
        {
            FrameworkElement streamRectangle = ((StreamingRectangleUserControl)MyContent).ScreenRectangle;

            if (streamRectangle == null) return null;

            return new RectangleGeometry()
            {
                Rect = new Rect(0, 0, streamRectangle.Width, streamRectangle.Height),
                Transform = (Transform)streamRectangle.TransformToAncestor(this.Visualizer)
            };
        }


        private void HitTestArea()
        {
            
            // For hit testing
            RectangleGeometry hitTestRectangle = GetHitTestRectangleGeometry();

            IdentifiedInteractionArea visualizer = this.Visualizer as IdentifiedInteractionArea;
            HashSet<DependencyObject> hitTestResults = HitTest(hitTestRectangle);

            // 2. Raise HoverOut events on disappeared elements
            // 3. Raise HoverMove events on existing elements
            foreach (UIElement element in _lensOverElements)
            {
                if (!hitTestResults.Contains(element))
                {
                    RaiseEventPair(element, IdentifiedEvents.IdentifiedHoverOutEvent, new RoutedIdentifiedHoverEventArgs(IdentifiedEvents.PreviewIdentifiedHoverOutEvent, ClientId, this, hitTestRectangle));
                    Debug.WriteLineIf(DebugSettings.DEBUG_HITTEST || DebugSettings.DEBUG_EVENTS, "Raising HoverOutEvent on " + element + ". Name: " + (element is FrameworkElement ? ((FrameworkElement)element).Name : "?"));
                }
                else
                {
                    RaiseEventPair(element, IdentifiedEvents.IdentifiedHoverMoveEvent, new RoutedIdentifiedHoverEventArgs(IdentifiedEvents.PreviewIdentifiedHoverMoveEvent, ClientId, this, hitTestRectangle));
                    Debug.WriteLineIf(DebugSettings.DEBUG_HITTEST || DebugSettings.DEBUG_EVENTS, "Raising HoverMoveEvent on " + element + ". Name: " + (element is FrameworkElement ? ((FrameworkElement)element).Name : "?"));
                }
            }
            _lensOverElements.RemoveWhere(elem => !hitTestResults.Contains(elem));

            // 4. Raise HoverOver events on new elements
            foreach (DependencyObject result in hitTestResults)
            {
                if (result is UIElement)
                {
                    UIElement uiResult = result as UIElement;
                    if (!_lensOverElements.Contains(uiResult))
                    {
                        _lensOverElements.Add(uiResult);
                        RaiseEventPair(uiResult, IdentifiedEvents.IdentifiedHoverOverEvent, new RoutedIdentifiedHoverEventArgs(IdentifiedEvents.PreviewIdentifiedHoverOverEvent, ClientId, this, hitTestRectangle));
                        Debug.WriteLineIf(DebugSettings.DEBUG_HITTEST || DebugSettings.DEBUG_EVENTS, "Raising HoverOverEvent on " + uiResult + ". Name: " + (uiResult is FrameworkElement ? ((FrameworkElement)uiResult).Name : "?"));
                    }
                }
            }
        }


        //private void HitTestAreaOld()
        //{
        //    // Do Hit Testing for PhoneOverEvent and PhoneOutEvent
        //    FrameworkElement streamRectangle = ((StreamingRectangleUserControl)MyContent).ScreenRectangle;
            
        //    // For hit testing
        //    RectangleGeometry hitTestRectangle = new RectangleGeometry()
        //    {
        //        Rect = new Rect(0, 0, streamRectangle.Width, streamRectangle.Height),
        //        Transform = (Transform)streamRectangle.TransformToAncestor(this.Visualizer)
        //    };

        //    IdentifiedInteractionArea visualizer = this.Visualizer as IdentifiedInteractionArea;
        //    HashSet<DependencyObject> hitTestResults = HitTest(hitTestRectangle);

        //    HashSet<UIElement> registeredAncestors = new HashSet<UIElement>();

        //    // 1. Get registered ancestors for each hit test result (if any!)
        //    foreach (DependencyObject result in hitTestResults)
        //    {
        //        DependencyObject depObj = visualizer.GetRegisteredIdentifiedAncestor(result);
        //        if (depObj != null && depObj is UIElement)
        //            registeredAncestors.Add(depObj as UIElement);
        //    }

        //    // 2. Raise LensOut events on disappeared elements
        //    // 3. Raise LensHoverMove events on existing elements
        //    foreach (UIElement element in _lensOverElements)
        //    {
        //        if (!registeredAncestors.Contains(element))
        //        {
        //            RaiseEventPair(element, IdentifiedEvents.IdentifiedHoverOutEvent, new RoutedIdentifiedHoverEventArgs(IdentifiedEvents.PreviewIdentifiedHoverOutEvent, ClientId, this, hitTestRectangle));
        //            Debug.WriteLineIf(DebugSettings.DEBUG_HITTEST || DebugSettings.DEBUG_EVENTS, "Raising LensOutEvent on " + element);
        //        }
        //        else
        //        {
        //            RaiseEventPair(element, IdentifiedEvents.IdentifiedHoverMoveEvent, new RoutedIdentifiedHoverEventArgs(IdentifiedEvents.PreviewIdentifiedHoverMoveEvent, ClientId, this, hitTestRectangle));
        //            Debug.WriteLineIf(DebugSettings.DEBUG_HITTEST || DebugSettings.DEBUG_EVENTS, "Raising LensHoverMoveEvent on " + element);
        //        }
        //    }
        //    _lensOverElements.RemoveWhere(elem => !registeredAncestors.Contains(elem));

        //    // 4. Raise LensOver events on new elements
        //    foreach (DependencyObject result in hitTestResults)
        //    {
        //        DependencyObject registeredAncestor = visualizer.GetRegisteredIdentifiedAncestor(result);
        //        if (registeredAncestor != null && registeredAncestor is UIElement)
        //        {
        //            UIElement registeredAncestorUIElement = registeredAncestor as UIElement;

        //            if (!_lensOverElements.Contains(registeredAncestorUIElement))
        //            {
        //                _lensOverElements.Add(registeredAncestorUIElement);
        //                RaiseEventPair(registeredAncestorUIElement, IdentifiedEvents.IdentifiedHoverOverEvent, new RoutedIdentifiedHoverEventArgs(IdentifiedEvents.PreviewIdentifiedHoverOverEvent, ClientId, this, hitTestRectangle));
        //                Debug.WriteLineIf(DebugSettings.DEBUG_HITTEST || DebugSettings.DEBUG_EVENTS, "Raising LensOverEvent on " + registeredAncestorUIElement);
        //            }
        //        }
        //    }
        //}

        private HashSet<DependencyObject> _areaHitTestResults = new HashSet<DependencyObject>();
        private HashSet<DependencyObject> HitTest(RectangleGeometry area)
        {
            _areaHitTestResults.Clear();
            VisualTreeHelper.HitTest(this.Visualizer,
                new HitTestFilterCallback(AreaHitTestFilterCallback),
                new HitTestResultCallback(AreaHitTestResultCallback),
                new GeometryHitTestParameters(area));
            return _areaHitTestResults;
        }

        private HitTestFilterBehavior AreaHitTestFilterCallback(DependencyObject o)
        {
            if (MyContent is StreamingRectangleUserControl)
            {
                if (((StreamingRectangleUserControl)MyContent).ScreenRectangle.Equals(o))
                {
                    return HitTestFilterBehavior.ContinueSkipSelf;
                }
                else
                {
                    return HitTestFilterBehavior.Continue;
                }
            }
            return HitTestFilterBehavior.Stop;
        }

        private HitTestResultBehavior AreaHitTestResultCallback(HitTestResult result)
        {
            _areaHitTestResults.Add(result.VisualHit);
            
            return HitTestResultBehavior.Continue;
        }

        #endregion


        #region public hit testing


        public HashSet<DependencyObject> GetDependencyObjectsBelowScreenRectangle()
        {
            // Do Hit Testing on reactagle equivalent to the phone screen
            FrameworkElement streamRectangle = ((StreamingRectangleUserControl)MyContent).ScreenRectangle;

            // For hit testing
            RectangleGeometry hitTestRectangle = new RectangleGeometry()
            {
                Rect = new Rect(0, 0, streamRectangle.Width, streamRectangle.Height),
                Transform = (Transform)streamRectangle.TransformToAncestor(this.Visualizer)
            };

            return HitTest(hitTestRectangle);
        }

        public bool IsOverUIElement(UIElement TargetElement)
        {
            HashSet<DependencyObject> results = GetDependencyObjectsBelowScreenRectangle();            
            return results.Contains(TargetElement);
        }

        #endregion

        #endregion


        #region PersonalizedView Members

        public void Add(UIElement view)
        {
            if (ClientSession.State is StreamingState && MyContent is StreamingRectangleUserControl)
            {
                ((StreamingRectangleUserControl)MyContent).AddPersonalizedView(view);
            }
        }

        public void Remove()
        {
            if (ClientSession.State is StreamingState && MyContent is StreamingRectangleUserControl)
            {
                ((StreamingRectangleUserControl)MyContent).RemovePersonalizedView();
            }
        }

        #endregion
    }
}
