using System.Windows;

namespace NAI.UI.Events
{
    public static class IdentifiedEvents
    {
        // These are the delegate types for the event handlers.
        public delegate void RoutedIdentifiedEventHandler(object sender, RoutedIdentifiedEventArgs e);
        public delegate void RoutedIdentifiedTouchEventHandler(object sender, RoutedIdentifiedTouchEventArgs e);
        public delegate void RoutedIdentifiedHoverEventHandler(object sender, RoutedIdentifiedHoverEventArgs e);

        #region Identified Touch Events

        #region Touch Enter

        public static readonly RoutedEvent PreviewIdentifiedTouchEnterEvent = EventManager.RegisterRoutedEvent(
        "PreviewIdentifiedTouchEnter", RoutingStrategy.Tunnel, typeof(RoutedIdentifiedTouchEventHandler), typeof(IdentifiedEvents));

        public static void AddPreviewIdentifiedTouchEnterHandler(DependencyObject d, RoutedIdentifiedTouchEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.AddHandler(IdentifiedEvents.PreviewIdentifiedTouchEnterEvent, handler);
            }
        }

        public static void RemovePreviewIdentifiedTouchEnterHandler(DependencyObject d, RoutedIdentifiedTouchEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.RemoveHandler(IdentifiedEvents.PreviewIdentifiedTouchEnterEvent, handler);
            }
        }


        public static readonly RoutedEvent IdentifiedTouchEnterEvent = EventManager.RegisterRoutedEvent(
        "IdentifiedTouchEnter", RoutingStrategy.Bubble, typeof(RoutedIdentifiedTouchEventHandler), typeof(IdentifiedEvents));

        public static void AddIdentifiedTouchEnterHandler(DependencyObject d, RoutedIdentifiedTouchEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.AddHandler(IdentifiedEvents.IdentifiedTouchEnterEvent, handler);
            }
        }

        public static void RemoveIdentifiedTouchEnterHandler(DependencyObject d, RoutedIdentifiedTouchEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.RemoveHandler(IdentifiedEvents.IdentifiedTouchEnterEvent, handler);
            }
        }

        #endregion

        #region Touch Down

        public static readonly RoutedEvent PreviewIdentifiedTouchDownEvent = EventManager.RegisterRoutedEvent(
           "PreviewIdentifiedTouchDown", RoutingStrategy.Tunnel, typeof(RoutedIdentifiedTouchEventHandler), typeof(IdentifiedEvents));

        public static void AddPreviewIdentifiedTouchDownHandler(DependencyObject d, RoutedIdentifiedTouchEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.AddHandler(IdentifiedEvents.PreviewIdentifiedTouchDownEvent, handler);
            }
        }

        public static void RemovePreviewIdentifiedTouchDownHandler(DependencyObject d, RoutedIdentifiedTouchEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.RemoveHandler(IdentifiedEvents.PreviewIdentifiedTouchDownEvent, handler);
            }
        }

        public static readonly RoutedEvent IdentifiedTouchDownEvent = EventManager.RegisterRoutedEvent(
            "IdentifiedTouchDown", RoutingStrategy.Bubble, typeof(RoutedIdentifiedTouchEventHandler), typeof(IdentifiedEvents));

        public static void AddIdentifiedTouchDownHandler(DependencyObject d, RoutedIdentifiedTouchEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.AddHandler(IdentifiedEvents.IdentifiedTouchDownEvent, handler);
            }
        }

        public static void RemoveIdentifiedTouchDownHandler(DependencyObject d, RoutedIdentifiedTouchEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.RemoveHandler(IdentifiedEvents.IdentifiedTouchDownEvent, handler);
            }
        }

        #endregion

        #region Touch Move

        public static readonly RoutedEvent PreviewIdentifiedTouchMoveEvent = EventManager.RegisterRoutedEvent(
        "PreviewIdentifiedTouchMove", RoutingStrategy.Tunnel, typeof(RoutedIdentifiedTouchEventHandler), typeof(IdentifiedEvents));

        public static void AddPreviewIdentifiedTouchMoveHandler(DependencyObject d, RoutedIdentifiedTouchEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.AddHandler(IdentifiedEvents.PreviewIdentifiedTouchMoveEvent, handler);
            }
        }

        public static void RemovePreviewIdentifiedTouchMoveHandler(DependencyObject d, RoutedIdentifiedTouchEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.RemoveHandler(IdentifiedEvents.PreviewIdentifiedTouchMoveEvent, handler);
            }
        }


        public static readonly RoutedEvent IdentifiedTouchMoveEvent = EventManager.RegisterRoutedEvent(
        "IdentifiedTouchMove", RoutingStrategy.Bubble, typeof(RoutedIdentifiedTouchEventHandler), typeof(IdentifiedEvents));

        public static void AddIdentifiedTouchMoveHandler(DependencyObject d, RoutedIdentifiedTouchEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.AddHandler(IdentifiedEvents.IdentifiedTouchMoveEvent, handler);
            }
        }

        public static void RemoveIdentifiedTouchMoveHandler(DependencyObject d, RoutedIdentifiedTouchEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.RemoveHandler(IdentifiedEvents.IdentifiedTouchMoveEvent, handler);
            }
        }

        #endregion

        #region Touch Up

        public static readonly RoutedEvent PreviewIdentifiedTouchUpEvent = EventManager.RegisterRoutedEvent(
           "PreviewIdentifiedTouchUp", RoutingStrategy.Tunnel, typeof(RoutedIdentifiedTouchEventHandler), typeof(IdentifiedEvents));

        public static void AddPreviewIdentifiedTouchUpHandler(DependencyObject d, RoutedIdentifiedTouchEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.AddHandler(IdentifiedEvents.PreviewIdentifiedTouchUpEvent, handler);
            }
        }

        public static void RemovePreviewIdentifiedTouchUpHandler(DependencyObject d, RoutedIdentifiedTouchEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.RemoveHandler(IdentifiedEvents.PreviewIdentifiedTouchUpEvent, handler);
            }
        }


        public static readonly RoutedEvent IdentifiedTouchUpEvent = EventManager.RegisterRoutedEvent(
        "IdentifiedTouchUp", RoutingStrategy.Bubble, typeof(RoutedIdentifiedTouchEventHandler), typeof(IdentifiedEvents));

        public static void AddIdentifiedTouchUpHandler(DependencyObject d, RoutedIdentifiedTouchEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.AddHandler(IdentifiedEvents.IdentifiedTouchUpEvent, handler);
            }
        }

        public static void RemoveIdentifiedTouchUpHandler(DependencyObject d, RoutedIdentifiedTouchEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.RemoveHandler(IdentifiedEvents.IdentifiedTouchUpEvent, handler);
            }
        }

        #endregion

        #region Touch Leave

        public static readonly RoutedEvent PreviewIdentifiedTouchLeaveEvent = EventManager.RegisterRoutedEvent(
        "PreviewIdentifiedTouchLeave", RoutingStrategy.Tunnel, typeof(RoutedIdentifiedTouchEventHandler), typeof(IdentifiedEvents));

        public static void AddPreviewIdentifiedTouchLeaveHandler(DependencyObject d, RoutedIdentifiedTouchEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.AddHandler(IdentifiedEvents.PreviewIdentifiedTouchLeaveEvent, handler);
            }
        }

        public static void RemovePreviewIdentifiedTouchLeaveHandler(DependencyObject d, RoutedIdentifiedTouchEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.RemoveHandler(IdentifiedEvents.PreviewIdentifiedTouchLeaveEvent, handler);
            }
        }


        public static readonly RoutedEvent IdentifiedTouchLeaveEvent = EventManager.RegisterRoutedEvent(
        "IdentifiedTouchLeave", RoutingStrategy.Bubble, typeof(RoutedIdentifiedTouchEventHandler), typeof(IdentifiedEvents));

        public static void AddIdentifiedTouchLeaveHandler(DependencyObject d, RoutedIdentifiedTouchEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.AddHandler(IdentifiedEvents.IdentifiedTouchLeaveEvent, handler);
            }
        }

        public static void RemoveIdentifiedTouchLeaveHandler(DependencyObject d, RoutedIdentifiedTouchEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.RemoveHandler(IdentifiedEvents.IdentifiedTouchLeaveEvent, handler);
            }
        }

        #endregion 

        #endregion

        #region Identified Hover Events

        #region Hover Over

        public static readonly RoutedEvent PreviewIdentifiedHoverOverEvent = EventManager.RegisterRoutedEvent(
        "PreviewIdentifiedHoverOver", RoutingStrategy.Tunnel, typeof(RoutedIdentifiedHoverEventHandler), typeof(IdentifiedEvents));

        public static void AddPreviewIdentifiedHoverOverHandler(DependencyObject d, RoutedIdentifiedHoverEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.AddHandler(IdentifiedEvents.PreviewIdentifiedHoverOverEvent, handler);
            }
        }

        public static void RemovePreviewIdentifiedHoverOverHandler(DependencyObject d, RoutedIdentifiedHoverEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.RemoveHandler(IdentifiedEvents.PreviewIdentifiedHoverOverEvent, handler);
            }
        }

        public static readonly RoutedEvent IdentifiedHoverOverEvent = EventManager.RegisterRoutedEvent(
        "IdentifiedHoverOver", RoutingStrategy.Bubble, typeof(RoutedIdentifiedHoverEventHandler), typeof(IdentifiedEvents));

        public static void AddIdentifiedHoverOverHandler(DependencyObject d, RoutedIdentifiedHoverEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.AddHandler(IdentifiedEvents.IdentifiedHoverOverEvent, handler);
            }
        }

        public static void RemoveIdentifiedHoverOverHandler(DependencyObject d, RoutedIdentifiedHoverEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.RemoveHandler(IdentifiedEvents.IdentifiedHoverOverEvent, handler);
            }
        }

        #endregion

        #region Hover Move

        public static readonly RoutedEvent PreviewIdentifiedHoverMoveEvent = EventManager.RegisterRoutedEvent(
        "PreviewIdentifiedHoverMove", RoutingStrategy.Tunnel, typeof(RoutedIdentifiedHoverEventHandler), typeof(IdentifiedEvents));

        public static void AddPreviewIdentifiedHoverMoveHandler(DependencyObject d, RoutedIdentifiedHoverEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.AddHandler(IdentifiedEvents.PreviewIdentifiedHoverMoveEvent, handler);
            }
        }

        public static void RemovePreviewIdentifiedHoverMoveHandler(DependencyObject d, RoutedIdentifiedHoverEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.RemoveHandler(IdentifiedEvents.PreviewIdentifiedHoverMoveEvent, handler);
            }
        }


        public static readonly RoutedEvent IdentifiedHoverMoveEvent = EventManager.RegisterRoutedEvent(
        "IdentifiedHoverMove", RoutingStrategy.Bubble, typeof(RoutedIdentifiedHoverEventHandler), typeof(IdentifiedEvents));

        public static void AddIdentifiedHoverMoveHandler(DependencyObject d, RoutedIdentifiedHoverEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.AddHandler(IdentifiedEvents.IdentifiedHoverMoveEvent, handler);
            }
        }

        public static void RemoveIdentifiedHoverMoveHandler(DependencyObject d, RoutedIdentifiedHoverEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.RemoveHandler(IdentifiedEvents.IdentifiedHoverMoveEvent, handler);
            }
        }

        #endregion

        #region Hover Out

        public static readonly RoutedEvent PreviewIdentifiedHoverOutEvent = EventManager.RegisterRoutedEvent(
        "PreviewIdentifiedHoverOut", RoutingStrategy.Tunnel, typeof(RoutedIdentifiedHoverEventHandler), typeof(IdentifiedEvents));

        public static void AddPreviewIdentifiedHoverOutHandler(DependencyObject d, RoutedIdentifiedHoverEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.AddHandler(IdentifiedEvents.PreviewIdentifiedHoverOutEvent, handler);
            }
        }

        public static void RemovePreviewIdentifiedHoverOutHandler(DependencyObject d, RoutedIdentifiedHoverEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.RemoveHandler(IdentifiedEvents.PreviewIdentifiedHoverOutEvent, handler);
            }
        }


        public static readonly RoutedEvent IdentifiedHoverOutEvent = EventManager.RegisterRoutedEvent(
        "IdentifiedHoverOut", RoutingStrategy.Bubble, typeof(RoutedIdentifiedHoverEventHandler), typeof(IdentifiedEvents));

        public static void AddIdentifiedHoverOutHandler(DependencyObject d, RoutedIdentifiedHoverEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.AddHandler(IdentifiedEvents.IdentifiedHoverOutEvent, handler);
            }
        }

        public static void RemoveIdentifiedHoverOutHandler(DependencyObject d, RoutedIdentifiedHoverEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.RemoveHandler(IdentifiedEvents.IdentifiedHoverOutEvent, handler);
            }
        }

        #endregion
            
        #endregion

        #region Identified Person Events

        #region Person Arrived

        public static readonly RoutedEvent IdentifiedPersonArrivedEvent = EventManager.RegisterRoutedEvent(
        "IdentifiedPersonArrived", RoutingStrategy.Direct, typeof(RoutedIdentifiedEventHandler), typeof(IdentifiedEvents));

        public static void AddIdentifiedPersonArrivedHandler(DependencyObject d, RoutedIdentifiedEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.AddHandler(IdentifiedEvents.IdentifiedPersonArrivedEvent, handler);
            }
        }

        public static void RemoveIdentifiedPersonArrivedHandler(DependencyObject d, RoutedIdentifiedEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.RemoveHandler(IdentifiedEvents.IdentifiedPersonArrivedEvent, handler);
            }
        }

        #endregion

        #region Person Left

        public static readonly RoutedEvent IdentifiedPersonLeftEvent = EventManager.RegisterRoutedEvent(
        "IdentifiedPersonLeft", RoutingStrategy.Direct, typeof(RoutedIdentifiedEventHandler), typeof(IdentifiedEvents));

        public static void AddIdentifiedPersonLeftHandler(DependencyObject d, RoutedIdentifiedEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.AddHandler(IdentifiedEvents.IdentifiedPersonLeftEvent, handler);
            }
        }

        public static void RemoveIdentifiedPersonLeftHandler(DependencyObject d, RoutedIdentifiedEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.RemoveHandler(IdentifiedEvents.IdentifiedPersonLeftEvent, handler);
            }
        }

        #endregion

        #endregion

    }
}
