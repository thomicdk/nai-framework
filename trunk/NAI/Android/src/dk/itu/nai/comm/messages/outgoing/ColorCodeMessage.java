package dk.itu.nai.comm.messages.outgoing;

import java.io.ByteArrayOutputStream;
import java.io.DataOutputStream;
import java.io.IOException;

import dk.itu.nai.comm.Protocol;

public class ColorCodeMessage extends OutgoingMessage {

	private byte[] _colorcode;

	public ColorCodeMessage(byte[] colorcode) {
		super(Protocol.COLOR_CODE);
		this._colorcode = colorcode;
	}

	@Override
	protected byte[] prepareMessageBody() throws IOException 
	{
    	ByteArrayOutputStream baos = new ByteArrayOutputStream();
    	DataOutputStream dos = new DataOutputStream(baos);
    	try {
    		dos.writeInt(this._colorcode.length);
    		dos.write(this._colorcode);
		} catch (IOException e) {}
    	return baos.toByteArray();
	}
}
