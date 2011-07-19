package dk.itu.nai.comm.messages.incomming;

import dk.itu.nai.comm.Protocol;

public class PincodeRejectedMessage extends IncomingBodyLessMessage {

	public PincodeRejectedMessage() {
		super(Protocol.PINCODE_REJECTED);
	}

}
