using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAI.UI.Client;
using NAI.Communication.MessageLayer.Messages.Incoming;

namespace NAI.Client
{
    /// <summary>
    /// A abstraction definition of a client state.
    /// A state must be able to handle incoming messages
    /// sent from the client.
    /// </summary>
    internal abstract class ClientState
    {
        protected ClientSession _session;

        public ClientState(ClientSession session)
        {
            this._session = session;
        }

        public abstract void HandleIncomingMessage(IncomingMessage message);
        public abstract void OnSessionEnd();
    }
}
