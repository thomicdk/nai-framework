package dk.itu.nai.comm.messages.incomming;

import dk.itu.nai.comm.Protocol;

public class IncomingBodyLessMessage extends IncomingMessage {

	public IncomingBodyLessMessage(Protocol command) {
		super(command);
	}

	@Override
	protected void parseMessage(byte[] messageBody) {}

}
