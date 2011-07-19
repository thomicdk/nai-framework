package dk.itu.nai.comm.messages.outgoing;

import dk.itu.nai.comm.Protocol;

public class TouchUpMessage extends TouchMessage {

	public TouchUpMessage( float x, float y) {
		super(Protocol.TOUCH_UP, x, y);
	}
}
