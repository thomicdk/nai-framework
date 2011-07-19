package dk.itu.nai.comm.messages.outgoing;

import java.io.ByteArrayOutputStream;
import java.io.DataOutputStream;
import java.io.IOException;

import android.util.Log;
import dk.itu.nai.comm.Protocol;

public class PincodeMessage extends OutgoingMessage {

	private byte[] _pincode;

	public PincodeMessage(byte[] pincode) {
		super(Protocol.PIN_CODE);
		Log.d("PincodeMessage", " Constructor: Got pincode array length: " + pincode.length);
		this._pincode = pincode;
	}

	@Override
	protected byte[] prepareMessageBody() throws IOException {
    	ByteArrayOutputStream baos = new ByteArrayOutputStream();
    	DataOutputStream dos = new DataOutputStream(baos);
    	try {
    		dos.writeByte(this._pincode.length);
    		dos.write(this._pincode);
		} catch (IOException e) {}
    	return baos.toByteArray();
	}
	

}
