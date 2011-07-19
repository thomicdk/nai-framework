using System.Windows.Media;
using System.Windows;
using NAI.UI.Client;
using NAI.Client;
using System.Windows.Shapes;
using System.Text;
using NAI.UI.Controls;

namespace NAI.UI.Events
{
    public class RoutedIdentifiedHoverEventArgs : RoutedIdentifiedEventArgs
    {
        public RectangleGeometry HoveringRectangle { get; private set; }

        public RoutedIdentifiedHoverEventArgs(RoutedEvent e, ClientIdentity clientId, RectangleGeometry rectangle)
            : base(e, clientId)
        {
            this.HoveringRectangle = rectangle;
        }

        public RoutedIdentifiedHoverEventArgs(RoutedEvent e, ClientIdentity clientId, object source, RectangleGeometry rectangle)
            : base(e, clientId, source)
        {
            this.HoveringRectangle = rectangle;
        }
    }
}
