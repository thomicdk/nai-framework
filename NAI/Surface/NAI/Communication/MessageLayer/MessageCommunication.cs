using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAI.Communication.SocketLayer;
using NAI.Communication.MessageLayer.Messages.Outgoing;
using NAI.Communication.MessageLayer.Messages.Incoming;
using System.Diagnostics;

namespace NAI.Communication.MessageLayer
{
    internal class MessageCommunication : ISocketLayerIncoming, IMessageLayerOutgoing
    {
        public IMessageLayerIncoming IncomingMessageHandler { get; set; }
        private ISocketLayerOutgoing _socketComm;
        
        public MessageCommunication(SocketCommunication socketComm)
        {
            this._socketComm = socketComm;
            socketComm.IncomingMessageHandler = this;
        }

        private void SendMessage(OutgoingMessage message)
        {
            _socketComm.SendMessage(message.PrepareMessage());
        }

        #region IMessageLayerOutgoing Members

        public void SendAuthenticationAccepted()
        {
            SendMessage(new AuthenticationAcceptedMessage());
        }

        public void SendAuthenticationRejected()
        {
            SendMessage(new AuthenticationRejectedMessage());
        }

        public void SendPictureStreamStart()
        {
            SendMessage(new PictureStreamStartMessage());
        }

        public void SendPictureStreamStop()
        {
            SendMessage(new PictureStreamStopMessage());
        }

        public void SendFrame(byte[] frame)
        {
            SendMessage(new FrameMessage(frame));
        }

        public void SendRequestPairingCode(byte pincodeLength, byte colorCodeLength)
        {
            SendMessage(new RequestPairingCodeMessage(pincodeLength, colorCodeLength));
        }

        public void SendPairingCodeAccepted()
        {
            SendMessage(new PairingCodeAcceptedMessage());
        }

        public void SendPincodeRejected()
        {
            SendMessage(new PincodeRejectedMessage());
        }

        #endregion

        #region ISocketLayerIncomingHandler Members

        public void OnIncomingMessage(byte[] message)
        {
            if (IncomingMessageHandler != null)
            {
                try
                {
                    IncomingMessage im = IncomingMessage.Parse(message);
                    if (IncomingMessageHandler != null)
                    {
                        IncomingMessageHandler.OnIncomingMessage(im);
                    }
                }
                catch (NotSupportedException e)
                {
                    Debug.WriteLine("MessageLayer: Protocol not valid. " + e.Message);
                }
            }
        }

        public void OnConnectionLost()
        {
            if (IncomingMessageHandler != null)
            {
                IncomingMessageHandler.OnConnectionLost();
                IncomingMessageHandler = null;
            }
        }

        #endregion
    }
}
