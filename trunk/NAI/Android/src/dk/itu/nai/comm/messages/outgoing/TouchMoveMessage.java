package dk.itu.nai.comm.messages.outgoing;

import dk.itu.nai.comm.Protocol;

public class TouchMoveMessage extends TouchMessage {

	public TouchMoveMessage( float x, float y) {
		super(Protocol.TOUCH_MOVE, x, y);
	}

}
