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
using NAI.Client;
using NAI.UI.Helpers;
using NAI.UI.Controls;

namespace WarmUp
{
    /// <summary>
    /// Interaction logic for EllipseColorPickerScatterViewItem.xaml
    /// </summary>
    public partial class ColorEllipseScatterViewItem : ScatterViewItem
    {

        private Dictionary<ClientIdentity, IPersonalizedView> _HoveringClientIdentities = new Dictionary<ClientIdentity, IPersonalizedView>();

        public ColorEllipseScatterViewItem()
        {

            ControlTemplate ct = new ControlTemplate();
            
            ContentPresenter cp = new ContentPresenter();
            cp.VerticalAlignment = VerticalAlignment.Center;
            cp.HorizontalAlignment = HorizontalAlignment.Center;
            cp.Content = MyElip;
            ct.RegisterName("ContentPresenter", cp);
            this.Template = ct;

            InitializeComponent();

            this.AddHandler(IdentifiedEvents.PreviewIdentifiedHoverOutEvent, new IdentifiedEvents.RoutedIdentifiedHoverEventHandler(MyEllipseScatterViewItemHover));
            this.AddHandler(IdentifiedEvents.PreviewIdentifiedHoverOverEvent, new IdentifiedEvents.RoutedIdentifiedHoverEventHandler(MyEllipseScatterViewItemHover));            
        }

        private void MyEllipseScatterViewItemHover(object sender, RoutedIdentifiedHoverEventArgs e)
        {            
            if (e.RoutedEvent == IdentifiedEvents.PreviewIdentifiedHoverOverEvent)
            {
                AddIdentityHovering(e.ClientId);
            }
            else if (e.RoutedEvent == IdentifiedEvents.PreviewIdentifiedHoverOutEvent)
                RemoveIdentityHovering(e.ClientId);
        }


        private void AddIdentityHovering(ClientIdentity id)
        {
            lock (_HoveringClientIdentities)
            {
                ColorPickerPersonalizedView cp = new ColorPickerPersonalizedView(MyElip);                
                //cp.Width = cv.GetHitTestRectangleGeometry().Bounds.Width;
                //cp.Height = cv.GetHitTestRectangleGeometry().Bounds.Height;
                id.PersonalizedView.Add(cp);
                if (!_HoveringClientIdentities.ContainsKey(id))
                    _HoveringClientIdentities.Add(id, id.PersonalizedView);
            }
        }

        private void RemoveIdentityHovering(ClientIdentity id)
        {
            lock (_HoveringClientIdentities)
            { 
                if (_HoveringClientIdentities.ContainsKey(id))
                {
                    _HoveringClientIdentities[id].Remove();
                    _HoveringClientIdentities.Remove(id);                    
                }
            }     
        }

        private void ScatterViewItem_ScatterManipulationDelta(object sender, ScatterManipulationDeltaEventArgs e)
        {
            List<ClientIdentity> CurrentHoveringClients = UIHelper.StreamingClientsOverUIElement(MyElip);
            List<ClientIdentity> ToBeAdded = new List<ClientIdentity>();
            List<ClientIdentity> ToBeRemoved = new List<ClientIdentity>();
            // for each!
            // If I don't know them, add them
            foreach (ClientIdentity id in CurrentHoveringClients)
            {
                // Those I don't know - handle them as if I got HoverOver event
                if (!_HoveringClientIdentities.ContainsKey(id))
                    ToBeAdded.Add(id);
            }

            // Those I know but aren't there handle them as if I got HoverOut event.
            foreach (ClientIdentity id in _HoveringClientIdentities.Keys)
            {
                // Those I know, but isn't above anymore, remove!
                if (!CurrentHoveringClients.Contains(id))
                    ToBeRemoved.Add(id);
            }
            foreach (ClientIdentity id in ToBeAdded)
            {
                AddIdentityHovering(id);                    
            }

            foreach (ClientIdentity id in ToBeRemoved)
            {
                RemoveIdentityHovering(id);
            }           
        }

    }
}
