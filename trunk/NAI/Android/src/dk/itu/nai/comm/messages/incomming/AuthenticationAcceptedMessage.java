package dk.itu.nai.comm.messages.incomming;

import dk.itu.nai.comm.Protocol;

public class AuthenticationAcceptedMessage extends IncomingBodyLessMessage {

	public AuthenticationAcceptedMessage() {
		super(Protocol.AUTHENTICATION_ACCEPTED);
	}
}
