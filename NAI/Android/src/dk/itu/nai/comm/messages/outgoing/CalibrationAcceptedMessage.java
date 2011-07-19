package dk.itu.nai.comm.messages.outgoing;

import dk.itu.nai.comm.Protocol;

public class CalibrationAcceptedMessage extends OutgoingBodyLessMessage {

	public CalibrationAcceptedMessage() {
		super(Protocol.CALIBRATION_ACCEPTED);
	}

}
