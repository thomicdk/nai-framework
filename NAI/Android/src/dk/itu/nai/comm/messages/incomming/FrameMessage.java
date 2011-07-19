package dk.itu.nai.comm.messages.incomming;

import java.io.ByteArrayInputStream;
import java.io.DataInputStream;
import java.io.IOException;

import dk.itu.nai.comm.Protocol;

public class FrameMessage extends IncomingMessage {
	
	private byte[] _imageBytes;

	public FrameMessage() {
		super(Protocol.FRAME);		
	}

	@Override
	protected void parseMessage(byte[] messageBody) throws IOException {
		ByteArrayInputStream bais = new ByteArrayInputStream(messageBody);
		DataInputStream dis = new DataInputStream(bais);
		
		int length = dis.readInt();		
		this._imageBytes = new byte[length];
		dis.readFully(this._imageBytes);
	}

	public byte[] getImageBytes()
	{
		return _imageBytes;
	}
}
