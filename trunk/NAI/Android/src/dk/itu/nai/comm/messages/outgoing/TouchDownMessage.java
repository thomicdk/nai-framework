package dk.itu.nai.comm.messages.outgoing;

import dk.itu.nai.comm.Protocol;

public class TouchDownMessage extends TouchMessage {

	public TouchDownMessage(float x, float y) {
		super(Protocol.TOUCH_DOWN, x, y);
	}

}
