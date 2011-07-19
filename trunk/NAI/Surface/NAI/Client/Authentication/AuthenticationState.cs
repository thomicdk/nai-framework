using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using NAI.Communication.MessageLayer.Messages.Incoming;
using NAI.Client.Pairing;
using NAI.Properties;

namespace NAI.Client.Authentication
{
    internal class AuthenticationState : ClientState
    {

        private static IAuthenticationHandler AuthenticationHandler = RuntimeSettings.AuthenticationHandler;

        public AuthenticationState(ClientSession session)
            : base(session)
        {
            
        }

        public override void HandleIncomingMessage(IncomingMessage message)
        {
            if (message is CredentialsMessage)
            {
                CredentialsMessage cm = message as CredentialsMessage;
                if (AuthenticationHandler.Authenticate(cm.Credentials))
                {
                    _session.ClientId.Credentials = cm.Credentials;
                    _session.Communication.SendAuthenticationAccepted();
                    _session.State = new PairingState(_session);
                }
                else
                {
                    _session.Communication.SendAuthenticationRejected();
                }
            }
            else if (message is ClientPausedMessage ||
                 message is ClientResumedMessage)
            {
                // Ignore, but not reject!
            }
            else
            {
                string errorMsg = string.Format("Message type '{0}' is not supported at this point", message.GetType().Name);
                Debug.WriteLine(errorMsg);
                throw new NotSupportedException(errorMsg);
            }
        }

        public override void OnSessionEnd()
        { }
    }
}
