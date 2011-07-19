using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NAI.Communication.MessageLayer.Messages.Outgoing
{
    internal sealed class AuthenticationAcceptedMessage : OutgoingBodyLessMessage
    {
        public AuthenticationAcceptedMessage()
            : base(MessageProtocol.AUTHENTICATION_ACCEPTED)
        { }
    }
}
