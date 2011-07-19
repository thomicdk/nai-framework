package dk.itu.nai.comm.messages.outgoing;

import dk.itu.nai.comm.Protocol;

public class ClientResumedMessage extends OutgoingBodyLessMessage {

	public ClientResumedMessage() {
		super(Protocol.CLIENT_RESUMED);
	}

}
