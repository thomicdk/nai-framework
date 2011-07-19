using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using NAI.UI.Events;
using NAI.Client;
using System.Windows.Media;
using System.Windows.Shapes;
using NAI.UI.Helpers;
using System.Windows;
using NAI.Client.Streaming;

namespace NAI.UI.Controls
{
    public class IdentifiedViewport : Canvas
    {
        #region Public Properties
        public Geometry Base 
        {
            get { return _viewportGeometry.Geometry1; }
            set { 
                _viewportGeometry.Geometry1 = value;
                _viewportShapeCopyTransparent.Data = value;
            }
        }

        public Brush Fill
        {
            get { return _viewportShape.Fill; }
            set { _viewportShape.Fill = value; }
        }

        #endregion

        #region Private members

        private Path _viewportShape;
        private Path _viewportShapeCopyTransparent;
        private CombinedGeometry _viewportGeometry;
        private GeometryGroup _clientScreenRectangles;

        #endregion


        public delegate bool IdentifiedViewportFilterDelegate(RoutedIdentifiedHoverEventArgs e);
        public IdentifiedViewportFilterDelegate FilterDelegate { get; set; }

        /// <summary>
        /// Key: Clients are identified by their tags
        /// Value: A rectangle geometry describing the location of the client's phone
        /// </summary>
        private Dictionary<ClientIdentity, RectangleGeometry> _clientToRectangle = new Dictionary<ClientIdentity, RectangleGeometry>();
        private Dictionary<ClientIdentity, RectangleGeometry> _clientHoveringRectangles = new Dictionary<ClientIdentity,RectangleGeometry>();

        public IdentifiedViewport()
        {
            IdentifiedInteractionArea.AddIdentifiedDependencyObjectRoot(this);
            InitializeLayout();
            InitializeEventHandlers();
            this.FilterDelegate = new IdentifiedViewportFilterDelegate(OnIdentifiedHoverEvent);
        }

        private void InitializeLayout()
        {
            _viewportShapeCopyTransparent = new Path();
            _viewportShapeCopyTransparent.Fill = Brushes.Transparent;

            _viewportShape = new Path();
            _viewportShape.Fill = Brushes.White;

            _viewportGeometry = new CombinedGeometry();
            _viewportGeometry.GeometryCombineMode = GeometryCombineMode.Exclude;
            _clientScreenRectangles = new GeometryGroup();
            _viewportGeometry.Geometry2 = _clientScreenRectangles;
            _viewportShape.Data = _viewportGeometry;

            this.Children.Add(_viewportShape);
            this.Children.Add(_viewportShapeCopyTransparent);
        }

        private void InitializeEventHandlers()
        {
            this.AddHandler(IdentifiedEvents.PreviewIdentifiedHoverOverEvent, new IdentifiedEvents.RoutedIdentifiedHoverEventHandler(OnHoverOver));
            this.AddHandler(IdentifiedEvents.PreviewIdentifiedHoverOutEvent, new IdentifiedEvents.RoutedIdentifiedHoverEventHandler(OnHoverOut));
            this.AddHandler(IdentifiedEvents.PreviewIdentifiedHoverMoveEvent, new IdentifiedEvents.RoutedIdentifiedHoverEventHandler(OnHoverMove));
        }


        private void AddScreenRectangle(ClientIdentity clientId, RectangleGeometry screenRectangle)
        {
            
            RectangleGeometry rg = new RectangleGeometry(screenRectangle.Rect);

            // Apply transformations            
            rg.Transform = BuildTransformGroup(screenRectangle);
            lock (_clientToRectangle)
            {
                if (!_clientHoveringRectangles.ContainsKey(clientId))
                {
                    _clientScreenRectangles.Children.Add(rg);
                    _clientHoveringRectangles.Add(clientId, screenRectangle);
                    _clientToRectangle[clientId] = rg;
                }
            }
        }

        private TransformGroup BuildTransformGroup(RectangleGeometry rect)
        {
            TransformGroup group = new TransformGroup();
            group.Children.Add(rect.Transform);
            group.Children.Add((Transform)IdentifiedInteractionArea.GetTransformTo(this));

            return group;
        }

        private void MoveScreenRectangle(ClientIdentity clientId, RectangleGeometry screenRectangle)
        {
            lock (_clientToRectangle)
            {
                _clientHoveringRectangles[clientId] = screenRectangle;
                if (_clientToRectangle.ContainsKey(clientId))
                {
                    _clientToRectangle[clientId].Transform = BuildTransformGroup(_clientHoveringRectangles[clientId]);
                }   
            }
        }

        private void RemoveScreenRectangle(ClientIdentity clientId)
        {
            if (_clientToRectangle.ContainsKey(clientId))
            {
                RectangleGeometry clientRectangle = _clientToRectangle[clientId];
                lock (_clientToRectangle)
                {
                    if (_clientScreenRectangles.Children.Contains(clientRectangle))
                    {
                        _clientScreenRectangles.Children.Remove(clientRectangle);
                    }
                    _clientToRectangle.Remove(clientId);
                    _clientHoveringRectangles.Remove(clientId);                    
                }
            }
        }

        private void OnHoverOver(object sender, RoutedIdentifiedHoverEventArgs e)
        {

            if (e.OriginalSource.Equals(_viewportShape)) 
            {
                if (FilterDelegate(e))
                {
                    AddScreenRectangle(e.ClientId, e.HoveringRectangle);
                }
                else 
                {
                    RemoveScreenRectangle(e.ClientId);
                }
            }
        }

        private void OnHoverOut(object sender, RoutedIdentifiedHoverEventArgs e)
        {
            if (!UIHelper.StreamingClientsOverUIElement(this._viewportShapeCopyTransparent).Contains(e.ClientId) ||
                e.OriginalSource.Equals(_viewportShapeCopyTransparent))
            {
                RemoveScreenRectangle(e.ClientId);
            }
        }

        private void OnHoverMove(object sender, RoutedIdentifiedHoverEventArgs e)
        {
            
            if (FilterDelegate(e))
            {
                MoveScreenRectangle(e.ClientId, e.HoveringRectangle);
            }
            else
            {
                RemoveScreenRectangle(e.ClientId);
            }
        }

        /// <summary>
        /// Default filter behaviour, accept everyone!
        /// </summary>
        private bool OnIdentifiedHoverEvent(RoutedIdentifiedHoverEventArgs e)
        {
            return true;
        }


        public void Moved()
        {

            List<ClientIdentity> CurrentHoveringClients = UIHelper.StreamingClientsOverUIElement((UIElement)_viewportShapeCopyTransparent);

            List<ClientIdentity> ToBeAdded = new List<ClientIdentity>();
            List<ClientIdentity> ToBeRemoved = new List<ClientIdentity>();
            lock (_clientToRectangle)
            {

                // for each!
                // If I don't know them, add them
                foreach (ClientIdentity id in CurrentHoveringClients)
                {
                    // Those I don't know - handle them as if I got HoverOver event
                    if (!_clientToRectangle.ContainsKey(id))
                        ToBeAdded.Add(id);
                }

                // Those I know but aren't there handle them as if I got HoverOut event.
                foreach (ClientIdentity id in _clientToRectangle.Keys)
                {
                    // Those I know, but isn't above anymore, remove!
                    if (!CurrentHoveringClients.Contains(id))
                        ToBeRemoved.Add(id);
                }
            }


             ClientState cs;
            foreach (ClientIdentity id in ToBeAdded)
            {
                cs = ClientSessionsController.Instance.GetClientState(id.TagData);
                if (cs != null && cs is StreamingState)
                {
                    AddScreenRectangle(id, ((StreamingState)cs).Visualization.GetHitTestRectangleGeometry());                    
                }
            }

            foreach (ClientIdentity id in ToBeRemoved)
            {
               RemoveScreenRectangle(id);
            }  
            

            lock (_clientToRectangle)
            {
                foreach (ClientIdentity k in _clientToRectangle.Keys)
                {
                    _clientToRectangle[k].Transform = BuildTransformGroup(_clientHoveringRectangles[k]);
                }
            }
        }
    }
}
