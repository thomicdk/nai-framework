package dk.itu.nai.comm.messages.incomming;

import dk.itu.nai.comm.Protocol;

public class PicureStreamStopMessage extends IncomingBodyLessMessage {

	public PicureStreamStopMessage() {
		super(Protocol.PICTURE_STREAM_STOP);
	}

}
