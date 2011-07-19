using System.Drawing;
using System.Windows;
using NAI.UI.Client;
using NAI.Client;
using NAI.UI.Controls;

namespace NAI.UI.Events
{
    public class RoutedIdentifiedTouchEventArgs : RoutedIdentifiedEventArgs
    {
        public System.Windows.Point Point { get; private set; }

        public RoutedIdentifiedTouchEventArgs(RoutedEvent e, ClientIdentity clientId, System.Windows.Point point)
            : base(e, clientId)
        {
            this.Point = point;
        }

        public RoutedIdentifiedTouchEventArgs(RoutedEvent e, ClientIdentity clientId, object source, System.Windows.Point point)
            : base(e, clientId, source)
        {
            this.Point = point;
        }
    }
}
