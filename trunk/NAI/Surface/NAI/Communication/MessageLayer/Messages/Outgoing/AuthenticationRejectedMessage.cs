using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NAI.Communication.MessageLayer.Messages.Outgoing
{
    internal sealed class AuthenticationRejectedMessage : OutgoingBodyLessMessage
    {
        public AuthenticationRejectedMessage()
            : base(MessageProtocol.AUTHENTICATION_REJECTED)
        { }

    }
}
