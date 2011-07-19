using Microsoft.Surface.Presentation.Controls;
using NAI.UI.Events;
using System.Windows;
using NAI.UI.Helpers;

namespace NAI.UI.Controls
{
    public class IdentifiedSurfaceButton : SurfaceButton
    {
        public IdentifiedSurfaceButton()
        {
            if (BlockClickEvent)
                IdentifiedInteractionArea.KillEventsForIdentifiedUIElement(this);
            IdentifiedInteractionArea.AddIdentifiedDependencyObjectRoot(this);
            this.AddHandler(IdentifiedEvents.PreviewIdentifiedTouchDownEvent, new IdentifiedEvents.RoutedIdentifiedTouchEventHandler(onIdentifiedTouchDown));
            this.AddHandler(IdentifiedEvents.PreviewIdentifiedTouchUpEvent, new IdentifiedEvents.RoutedIdentifiedTouchEventHandler(onIdentifiedTouchUp));
            this.AddHandler(IdentifiedEvents.PreviewIdentifiedTouchLeaveEvent, new IdentifiedEvents.RoutedIdentifiedTouchEventHandler(onIdentifiedTouchLeave));
            
            this.AddHandler(IdentifiedEvents.PreviewIdentifiedHoverOverEvent, new IdentifiedEvents.RoutedIdentifiedHoverEventHandler(onIdentifiedHoverOver));
            this.AddHandler(IdentifiedEvents.PreviewIdentifiedHoverMoveEvent, new IdentifiedEvents.RoutedIdentifiedHoverEventHandler(onIdentifiedHoverMove));
            this.AddHandler(IdentifiedEvents.PreviewIdentifiedHoverOutEvent, new IdentifiedEvents.RoutedIdentifiedHoverEventHandler(onIdentifiedHoverOut));
        }
        
        /// <summary>
        /// Should you decide to subclass a IdentifiedSurfaceButton and override onIdentifiedTouchDown or onIdentifiedTouchDown,
        /// then call base.onIdentifiedTouchDown or ...Up before doing anything else. 
        /// 
        /// You could also add your own PreviewIdentifiedTouchDownEvent or PreviewIdentifiedTouchUpEvent handlers. 
        /// 
        /// PreviewIdentifiedTouchMoveEvent is not supported by this class. You need to implement you own events.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void onIdentifiedTouchDown(object sender, RoutedIdentifiedTouchEventArgs e)
        {            
            if (this.IsEnabled)
            {
                AnimationHelper.SetSurfaceButtonIsPressed(this, true);
                e.Handled = true; // Supress the event to avoid the bubbling event from being raised 
                this.RaiseEvent(new RoutedIdentifiedEventArgs(IdentifiedSurfaceButton.IdentifiedClickEvent, e.ClientId));
            }
        }

        protected void onIdentifiedTouchUp(object sender, RoutedIdentifiedTouchEventArgs e)
        {
            if (this.IsEnabled)
                AnimationHelper.SetSurfaceButtonIsPressed(this, false);
        }


        protected void onIdentifiedTouchLeave(object sender, RoutedIdentifiedTouchEventArgs e)
        {
            if (this.IsEnabled)
                AnimationHelper.SetSurfaceButtonIsPressed(this, false);
        }

        private bool _blockClickEvent = true;
        public bool BlockClickEvent
        {
            get { return _blockClickEvent; }
            set
            {
                if (_blockClickEvent != value)
                {
                    if (value)
                        IdentifiedInteractionArea.KillEventsForIdentifiedUIElement(this);
                    else
                        IdentifiedInteractionArea.StopKillEventsForIdentifiedUIElement(this);
                  _blockClickEvent = value;
                }                
            }
        }

        # region Identified Click events    

        // Register IdentifiedClickEvent
        public static readonly RoutedEvent IdentifiedClickEvent = EventManager.RegisterRoutedEvent(
           "IdentifiedClick", RoutingStrategy.Bubble, typeof(IdentifiedEvents.RoutedIdentifiedEventHandler), typeof(IdentifiedSurfaceButton));

        // Provide CLR accessors for the event
        public event IdentifiedEvents.RoutedIdentifiedEventHandler IdentifiedClick
        {
            add { AddHandler(IdentifiedClickEvent, value); }
            remove { RemoveHandler(IdentifiedClickEvent, value); }
        }

        # endregion

        # region Identified Hover Events

        int _hoveringOverCounter = 0;

        private void onIdentifiedHoverOver(object sender, RoutedIdentifiedHoverEventArgs e)
        {
            e.Source = this;
            _hoveringOverCounter++;
            if (_hoveringOverCounter > 1)
            {
                e.Handled = true;
            }
        }

        private void onIdentifiedHoverMove(object sender, RoutedIdentifiedHoverEventArgs e)
        {
            //Debug.WriteLine(string.Format("HoverMove: Source: {0}. Org. Source: {1}", e.Source, e.OriginalSource ));
            //e.Source = this;
            e.Source = this;
        }

        private void onIdentifiedHoverOut(object sender, RoutedIdentifiedHoverEventArgs e)
        {
            e.Source = this;
            _hoveringOverCounter--;
            if (_hoveringOverCounter > 0)
            {
                e.Handled = true;
            }
        }    
    
        #endregion
    }



}
