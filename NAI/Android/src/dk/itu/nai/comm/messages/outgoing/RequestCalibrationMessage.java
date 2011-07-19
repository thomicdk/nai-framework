package dk.itu.nai.comm.messages.outgoing;

import dk.itu.nai.comm.Protocol;

public class RequestCalibrationMessage extends OutgoingBodyLessMessage {

	public RequestCalibrationMessage() {
		super(Protocol.REQUEST_CALIBRATION);
	}

}
