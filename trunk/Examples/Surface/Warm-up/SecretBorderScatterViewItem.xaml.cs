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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using System.Diagnostics;
using NAI.UI.Events;
using NAI.UI.Helpers;
using NAI.Client;

namespace WarmUp
{
    /// <summary>
    /// Interaction logic for SurfaceUserControl1.xaml
    /// </summary>
    public partial class SecretBorderScatterViewItem: ScatterViewItem
    {
        
        private HashSet<ClientIdentity> _HoveringClients = new HashSet<ClientIdentity>();
    

        public SecretBorderScatterViewItem()
        {
            InitializeComponent();

            MySecretBorder.AddHandler(IdentifiedEvents.PreviewIdentifiedHoverOverEvent, new IdentifiedEvents.RoutedIdentifiedHoverEventHandler(MySecretBorderHover));
            MySecretBorder.AddHandler(IdentifiedEvents.PreviewIdentifiedHoverOutEvent, new IdentifiedEvents.RoutedIdentifiedHoverEventHandler(MySecretBorderHover));
        }

        private void MySecretBorderHover(object sender, RoutedIdentifiedHoverEventArgs e)
        {
            if (e.RoutedEvent == IdentifiedEvents.PreviewIdentifiedHoverOverEvent)
            {
                NewPersonOverBorder(e.ClientId);
            }
            else if (e.RoutedEvent == IdentifiedEvents.PreviewIdentifiedHoverOutEvent)
            {
                RemovePersonOverBorder(e.ClientId);
            }
        }


        private void BorderScatterViewItem_ScatterManipulationDelta(object sender, ScatterManipulationDeltaEventArgs e)
        {                     
            List<ClientIdentity> CurrentHoveringClients = UIHelper.StreamingClientsOverUIElement(MySecretBorder);
            List<ClientIdentity> ToBeAdded = new List<ClientIdentity>();
            List<ClientIdentity> ToBeRemoved = new List<ClientIdentity>();
            // for each!
            // If I don't know them, add them
            foreach (ClientIdentity id in CurrentHoveringClients)
            {
                // Those I don't know - handle them as if I got HoverOver event
                if (!_HoveringClients.Contains(id))
                    ToBeAdded.Add(id);
            }
            
            // Those I know but aren't there handle them as if I got HoverOut event.
            foreach (ClientIdentity id in _HoveringClients)
            {
                // Those I know, but isn't above anymore, remove!
                if (!CurrentHoveringClients.Contains(id))
                    ToBeRemoved.Add(id);
            }

            foreach (ClientIdentity id in ToBeAdded)
                NewPersonOverBorder(id);
            foreach (ClientIdentity id in ToBeRemoved)
                RemovePersonOverBorder(id);
        }

        #region SecretBorder helper methods
        private void ToggleBorderVisibility()
        {
            if (_HoveringClients.Count <= 0)
            {
                MySecretBorderTextBlock.Visibility = Visibility.Hidden;
                MySecretBorder.Background = Brushes.Red;
            }
            else
            {
                MySecretBorderTextBlock.Visibility = Visibility.Visible;
                MySecretBorder.Background = Brushes.LightGreen;
            }
        }

        private void NewPersonOverBorder(ClientIdentity id)
        {
            lock (_HoveringClients)
            {
                _HoveringClients.Add(id);
                MySecretBorderTextBlock.Text = "Hello, " + id.Credentials.UserId;
                ToggleBorderVisibility();
            }
        }

        private void RemovePersonOverBorder(ClientIdentity id)
        {
            lock (_HoveringClients)
            {
                _HoveringClients.Remove(id);
                ToggleBorderVisibility();
            }
        }

        #endregion


    }
}
