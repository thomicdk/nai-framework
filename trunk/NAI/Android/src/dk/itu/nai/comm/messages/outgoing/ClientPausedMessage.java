package dk.itu.nai.comm.messages.outgoing;

import dk.itu.nai.comm.Protocol;

public class ClientPausedMessage extends OutgoingBodyLessMessage {

	public ClientPausedMessage() {
		super(Protocol.CLIENT_PAUSED);
	}
}
