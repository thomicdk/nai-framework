package dk.itu.nai.comm.messages.outgoing;

import java.io.IOException;

import dk.itu.nai.comm.Protocol;

public class OutgoingBodyLessMessage extends OutgoingMessage {

	public OutgoingBodyLessMessage(Protocol command) {
		super(command);
	}

	@Override
	protected byte[] prepareMessageBody() throws IOException {
		return new byte[0];
	}

	
}
