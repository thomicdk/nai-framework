using System;
using System.Windows;
using NAI.UI.Client;
using NAI.Client;
using NAI.UI.Controls;

namespace NAI.UI.Events
{
    public class RoutedIdentifiedEventArgs : RoutedEventArgs
    {
        public ClientIdentity ClientId { get; private set; }

        public RoutedIdentifiedEventArgs(RoutedEvent e, ClientIdentity clientId)
            : base(e)
        {
            if (clientId == null)
                throw new NotSupportedException("clientId cannot be null.");
            this.ClientId = clientId;
        }

        public RoutedIdentifiedEventArgs(RoutedEvent e, ClientIdentity clientId, object source)
            : base(e, source)
        {
            if (clientId == null)
                throw new NotSupportedException("clientId cannot be null.");
            this.ClientId = clientId;
        }

    }
}
