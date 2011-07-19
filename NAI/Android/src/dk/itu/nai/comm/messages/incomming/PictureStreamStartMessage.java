package dk.itu.nai.comm.messages.incomming;

import dk.itu.nai.comm.Protocol;

public class PictureStreamStartMessage extends IncomingBodyLessMessage {

	public PictureStreamStartMessage() {
		super(Protocol.PICTURE_STREAM_START);
	}

}
