using System;
using NAI.UI.Client;
using NAI.UI.Events;
using System.Diagnostics;
using NAI.UI.Controls;

namespace NAI.Client
{
    /// <summary>
    /// A abstract definition of a paired state.
    /// </summary>
    internal abstract class PairedState : ClientState
    {
        protected IdentifiedInteractionArea _visualizer;
        private ClientTagVisualization _visualization;

        public ClientTagVisualization Visualization
        {
            get { return _visualization; }
            protected set
            {
                _visualization = value;
                if (_visualization != null && _visualization.Visualizer != null && _visualization.Visualizer is IdentifiedInteractionArea)
                {
                    this._visualizer = (IdentifiedInteractionArea)_visualization.Visualizer;
                }
            }
        }

        public PairedState(ClientSession session, ClientTagVisualization visualization)
            : base(session)
        {
            this.Visualization = visualization;
        }

        public void UpdateUI()
        {
            if (Visualization != null)
            {
                Visualization.UpdateUI();
            }
        }

        public override void OnSessionEnd()
        {
            if (_visualizer != null)
            {
                Debug.WriteLineIf(DebugSettings.DEBUG_EVENTS, "Raising IdentifiedPersonLeftEvent");
                _visualizer.Dispatcher.Invoke((Action)delegate()
                {
                    _visualizer.RaiseEvent(new RoutedIdentifiedEventArgs(IdentifiedEvents.IdentifiedPersonLeftEvent, _session.ClientId, _visualization));
                });
            }
            Visualization = null;
            _visualizer = null;
        }

        public virtual void OnLostTag()
        {
            this.Visualization = null;
        }

        public virtual void OnGotTag(ClientTagVisualization visualization)
        {
            this.Visualization = visualization;
            _session.ClientId.PersonalizedView = visualization;
        }
    }
}
