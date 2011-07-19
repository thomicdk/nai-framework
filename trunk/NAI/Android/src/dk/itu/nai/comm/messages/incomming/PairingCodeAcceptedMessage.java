package dk.itu.nai.comm.messages.incomming;

import dk.itu.nai.comm.Protocol;

public class PairingCodeAcceptedMessage extends IncomingBodyLessMessage {

	public PairingCodeAcceptedMessage() {
		super(Protocol.PAIRING_CODE_ACCEPTED);
	}
}
