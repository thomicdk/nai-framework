using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using NAI.UI.Events;
using Restaurant.States.Checkout;
using System.Windows.Media.Animation;
using Restaurant.Model;
using System.Diagnostics;
using Restaurant.States.Ordering;
using Restaurant.States.Eating;
using System.ComponentModel;
using Restaurant.States.Final;
using Petzold.TextOnPath;

namespace Restaurant
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class MainWindow : SurfaceWindow
    {
        private UIElement _currentView;

        public MainWindow()
        {
            InitializeComponent();
            // Add handlers for Application activation events
            AddActivationHandlers();
            NAI.UI.Controls.IdentifiedInteractionArea.AddIdentifiedDependencyObjectRoot(InnerEllipse);
            RestaurantIIA.AddHandler(IdentifiedEvents.IdentifiedPersonArrivedEvent, new IdentifiedEvents.RoutedIdentifiedEventHandler(PersonCheckin));
            RestaurantIIA.AddHandler(IdentifiedEvents.IdentifiedPersonLeftEvent, new IdentifiedEvents.RoutedIdentifiedEventHandler(PersonCheckout));
            Session.Instance.SetRestaurantView(this);
        }

        #region Window Members
        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for Application activation events
            RemoveActivationHandlers();
        }

        /// <summary>
        /// Adds handlers for Application activation events.
        /// </summary>
        private void AddActivationHandlers()
        {
            // Subscribe to surface application activation events
            ApplicationLauncher.ApplicationActivated += OnApplicationActivated;
            ApplicationLauncher.ApplicationPreviewed += OnApplicationPreviewed;
            ApplicationLauncher.ApplicationDeactivated += OnApplicationDeactivated;
        }

        /// <summary>
        /// Removes handlers for Application activation events.
        /// </summary>
        private void RemoveActivationHandlers()
        {
            // Unsubscribe from surface application activation events
            ApplicationLauncher.ApplicationActivated -= OnApplicationActivated;
            ApplicationLauncher.ApplicationPreviewed -= OnApplicationPreviewed;
            ApplicationLauncher.ApplicationDeactivated -= OnApplicationDeactivated;
        }

        /// <summary>
        /// This is called when application has been activated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplicationActivated(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when application is in preview mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplicationPreviewed(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        ///  This is called when application has been deactivated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplicationDeactivated(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }
        #endregion


        public void GoToState(Restaurant.Model.States newState)
        {
            if (_currentView != null)
            {
                MyContainer.Children.Remove(_currentView);
                _currentView = null;
            }

            if (newState == Restaurant.Model.States.Ordering)
            {
                _currentView = new OrderingView();
                SetButtonColor(Colors.Red, Colors.DarkRed);
                CreateCircularText(" Confirm Order --- Confirm Order --- Confirm Order ---");
            }
            else if (newState == Restaurant.Model.States.Eating)
            {
                _currentView = new EatingView();
                SetButtonColor(Colors.Blue, Colors.DarkBlue);
                CreateCircularText(" Go to Checkout -- Go to Checkout -- Go to Checkout --");
            }
            else if (newState == Restaurant.Model.States.Checkout)
            {
                _currentView = new CheckoutView();
                SetButtonColor(Colors.Gold, Colors.DarkGoldenrod);
                CreateCircularText(" Pay Bill ---- Pay Bill --- Pay Bill ---- Pay Bill ---");
            }
            else if (newState == Restaurant.Model.States.Finished)
            {
                
                _currentView = new FinishedView();
                //MyContainer.Children.Add(new FinishedView());
                //_currentView = null;
            }

            if (_currentView != null)
            {
                MyContainer.Children.Add(_currentView);
            }
        }

        private void SetButtonColor(Color newColor, Color darkerColor)
        { 
            foreach (GradientStop gradientStop in ((RadialGradientBrush)OuterEllipse.Fill).GradientStops)
            {
                if (!gradientStop.Color.Equals(Colors.White))
                {
                    gradientStop.Color = newColor;
                }
            }
            CenterButtonBackground.Fill = new SolidColorBrush(darkerColor);
        }

        private void CreateCircularText(string text)
        {
            EllipseGeometry rectangleGeometry = new EllipseGeometry(new Rect(15, 15, 160, 160));
            PathGeometry pathGeometry = PathGeometry.CreateFromGeometry(rectangleGeometry);
            PathFigure pathFigure = pathGeometry.Figures[0];

            TextOnPathWarped textOnPathWarped = new TextOnPathWarped();
            textOnPathWarped.Text = text;
            textOnPathWarped.Foreground = Brushes.Black;
            textOnPathWarped.PathFigure = pathFigure;
            textOnPathWarped.HorizontalAlignment = HorizontalAlignment.Center;
            textOnPathWarped.VerticalAlignment = VerticalAlignment.Center;

            RoundText.Child = textOnPathWarped;
        }


        private DoubleAnimation rAnimation = null;
        private DoubleAnimation gAnimation = null;

        private void CenterButtonAnimations()
        {
            if (rAnimation == null)
            {
                // Rotate Text
                RotateTransform oTransform = (RotateTransform)RoundText.RenderTransform;
                rAnimation = new DoubleAnimation();
                rAnimation.From = oTransform.Angle;
                rAnimation.To = oTransform.Angle + 360;
                rAnimation.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 5000));
                rAnimation.RepeatBehavior = RepeatBehavior.Forever;
                
                oTransform.BeginAnimation(RotateTransform.AngleProperty, rAnimation);

                // Glowing Button
                gAnimation = new DoubleAnimation();
                gAnimation.From = 0.8;
                gAnimation.To = 0.5;
                gAnimation.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 1000));
                gAnimation.RepeatBehavior = RepeatBehavior.Forever;
                gAnimation.AutoReverse = true;
                OuterEllipse.BeginAnimation(UIElement.OpacityProperty, gAnimation);
            }
            else
            {
                // Stop animations
                // Rotate animation
                double angleAtAnimationStop = ((RotateTransform)RoundText.RenderTransform).Angle; // Save current angle
                ((RotateTransform)RoundText.RenderTransform).BeginAnimation(RotateTransform.AngleProperty, null); // Reset
                ((RotateTransform)RoundText.RenderTransform).Angle = angleAtAnimationStop; // Set angle again

                // Glowing button
                
                OuterEllipse.BeginAnimation(UIElement.OpacityProperty, null);
                
                gAnimation = null;
                rAnimation = null;
            }
        }


        private void Ellipse_HoverOver(object sender, RoutedIdentifiedHoverEventArgs e)
        {
            CenterButtonAnimations();
            if (Session.Instance.GlobalState == Restaurant.Model.States.Ordering)
            {
                if (Session.Instance.GetPerson(e.ClientId).State == Restaurant.Model.States.Ordering)
                {
                    e.ClientId.PersonalizedView.Add(new ConfirmOrderPersonalView());
                }
            }
            else if (Session.Instance.GlobalState == Restaurant.Model.States.Eating)
            {
                if (Session.Instance.GetPerson(e.ClientId).State == Restaurant.Model.States.Eating)
                {
                    e.ClientId.PersonalizedView.Add(new GoToPaymentPersonalView());
                }
            }
            else if (Session.Instance.GlobalState == Restaurant.Model.States.Checkout)
            {
                Person p = Session.Instance.GetPerson(e.ClientId);
                if (p.State == Restaurant.Model.States.Checkout && p.PaymentAmount > 0)
                {
                    e.ClientId.PersonalizedView.Add(new ConfirmPaymentPersonalView(p));
                }
            }
        }
        
        private void Ellipse_HoverOut(object sender, RoutedIdentifiedHoverEventArgs e)
        {            
            CenterButtonAnimations();
            e.ClientId.PersonalizedView.Remove();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            GoToState(Restaurant.Model.States.Ordering);
        }

        private void PersonCheckin(object sender, RoutedIdentifiedEventArgs args)
        {
            Debug.WriteLine("Person Check-in");
            if (!Session.Instance.HasPerson(args.ClientId))
            {
                Session.Instance.AddPerson(new Person(args.ClientId));
            }
        }

        private void PersonCheckout(object sender, RoutedIdentifiedEventArgs args)
        {
            Debug.WriteLine("Person Check-out");
            Session.Instance.RemovePerson(args.ClientId);
        }

        // For Testing
        private void OuterEllipse_PreviewContactDown(object sender, ContactEventArgs e)
        {
            //if (e.Contact.IsFingerRecognized)
            //{
            //    CenterButtonAnimations();
            //}
            //else if (!e.Contact.IsTagRecognized)
            //{
            Restaurant.Model.States newState = StateMachineHelper.GetNextState(Session.Instance.GlobalState);
            if (Session.Instance.Persons.Count > 0)
            {
                foreach (Person p in Session.Instance.Persons)
                {
                    Session.Instance.NextStateForPerson(p, newState);
                }
            }
            else
            {
                Session.Instance.NextGlobalState();
            }   
            //}
        }
    }
}