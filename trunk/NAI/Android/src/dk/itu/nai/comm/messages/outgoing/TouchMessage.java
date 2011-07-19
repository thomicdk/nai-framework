package dk.itu.nai.comm.messages.outgoing;

import java.io.ByteArrayOutputStream;
import java.io.DataOutputStream;
import java.io.IOException;

import dk.itu.nai.comm.Protocol;

public class TouchMessage extends OutgoingMessage {

	private float y;
	private float x;

	public TouchMessage(Protocol command, float x, float y) {
		super(command);
		this.x = x;
		this.y = y;
	}

	@Override
	protected byte[] prepareMessageBody() throws IOException {
    	ByteArrayOutputStream baos = new ByteArrayOutputStream();
    	DataOutputStream dos = new DataOutputStream(baos);
    	try {
    		dos.writeDouble(x);
    		dos.writeDouble(y);
		} catch (IOException e) {}
    	return baos.toByteArray();
	}
}
