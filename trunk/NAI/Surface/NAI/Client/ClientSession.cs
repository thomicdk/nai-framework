using NAI.UI.Client;
using NAI.Communication;
using NAI.Communication.MessageLayer;
using Microsoft.Surface.Presentation;
using NAI.Communication.MessageLayer.Messages.Incoming;
using NAI.Communication.SocketLayer;
using NAI.Client.Authentication;

namespace NAI.Client
{
    /// <summary>
    /// This is the state machine controlling a client's (smartphone)
    /// session with the tabletop.
    /// A session always starts upon socket connection establishment,
    /// and first state is always authentication.
    /// A session ends when the connection to the client is terminated or lost
    /// </summary>
    internal sealed class ClientSession : IMessageLayerIncoming
    {
        /// <summary>
        /// The current state of the client
        /// </summary>
        public ClientState State { get; set; }
        
        /// <summary>
        /// Communication interface with the client
        /// </summary>
        public IMessageLayerOutgoing Communication { get; private set; }

        /// <summary>
        /// The identity of the client
        /// </summary>
        public ClientIdentity ClientId { get; private set; }

        /// <summary>
        /// The tag used by the client
        /// </summary>
        public TagData Tag { get; set; }

        public ClientSession(MessageCommunication messageComm)
        {
            this.Communication = messageComm;
            messageComm.IncomingMessageHandler = this;
            this.ClientId = new ClientIdentity();
            State = new AuthenticationState(this);
        }

        public void SetClientTag(TagData tag)
        {
            this.Tag = tag;
            this.ClientId.TagData = tag;
        }

        #region IMessageLayerIncomingHandler Members

        public void OnIncomingMessage(IncomingMessage message)
        {
            this.State.HandleIncomingMessage(message);
        }

        public void OnConnectionLost()
        {
            // End session
            this.State.OnSessionEnd();
            Communication = null;
            ClientSessionsController.Instance.RemoveClient(this);
        }

        #endregion
    }

}
