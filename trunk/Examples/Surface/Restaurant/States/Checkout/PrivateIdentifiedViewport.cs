using System.Windows;
using NAI.Client;
using NAI.UI.Controls;
using NAI.UI.Events;

namespace Restaurant.States.Checkout
{
    public class PrivateIdentifiedViewport : IdentifiedViewport
    {
        public static readonly DependencyProperty ClientIdProperty =
            DependencyProperty.Register("ClientId", 
            typeof(ClientIdentity), typeof(PrivateIdentifiedViewport));

        public ClientIdentity ClientId
        {
            get { return (ClientIdentity)GetValue(ClientIdProperty); }
            set { SetValue(ClientIdProperty, value); }
        }

        public PrivateIdentifiedViewport()
        {
            base.FilterDelegate = new IdentifiedViewport.IdentifiedViewportFilterDelegate(Filter);
        }

        private bool Filter(RoutedIdentifiedEventArgs e)
        {
            if (ClientId == null) return true;
            return e.ClientId.Equals(ClientId);
        }
    }
}
