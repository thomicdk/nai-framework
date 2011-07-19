package dk.itu.nai.comm.messages.incomming;

import java.io.ByteArrayInputStream;
import java.io.DataInputStream;
import java.io.IOException;

import dk.itu.nai.comm.Protocol;

public class RequestPairingCodeMessage extends IncomingMessage {

	private byte colorCodeLength;
	private byte pincodeLength;

	public RequestPairingCodeMessage() {
		super(Protocol.REQUEST_PAIRING_CODE);
	}

	@Override
	protected void parseMessage(byte[] messageBody) throws IOException {
		ByteArrayInputStream bais = new ByteArrayInputStream(messageBody);
		DataInputStream dis = new DataInputStream(bais);
		pincodeLength = dis.readByte();
		colorCodeLength = dis.readByte();
	}

	public byte getPincodeLength() {
		return pincodeLength;
	}
	 
	public byte getColorCodeLength()
	{
		return colorCodeLength;
	}
}
