using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAI.Client.Authentication;
using NAI.Properties;

namespace NAI.Communication.MessageLayer.Messages.Incoming
{
    internal sealed class CredentialsMessage : IncomingMessage
    {
        public ClientCredentials Credentials { get; set; }

        public CredentialsMessage() : base(MessageProtocol.CREDENTIALS)
        {  }

        protected override void ParseMessage(byte[] incomingMessageBytes)
        {
            this.Credentials = Settings.AuthenticationHandler.ParseCredentialsMessage(incomingMessageBytes);
        }
    }
}
