package dk.itu.nai.comm.messages.incomming;

import dk.itu.nai.comm.Protocol;

public class AuthenticationRejectedMessage extends IncomingBodyLessMessage {

	public AuthenticationRejectedMessage() {
		super(Protocol.AUTHENTICATION_REJECTED);
	}
}
