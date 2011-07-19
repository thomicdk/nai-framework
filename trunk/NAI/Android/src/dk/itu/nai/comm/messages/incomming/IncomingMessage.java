package dk.itu.nai.comm.messages.incomming;

import java.io.IOException;

import dk.itu.nai.comm.Protocol;
import dk.itu.nai.comm.messages.Message;

public abstract class IncomingMessage extends Message {

	public IncomingMessage(Protocol command) {
		super(command);		
		
	}

	protected abstract void parseMessage(byte[] messageBody) throws IOException;
	
    public static IncomingMessage parse(byte[] incomingMessageBytes) throws IOException
    {
        IncomingMessage im = GetMessageType(incomingMessageBytes[0]);
        if (incomingMessageBytes.length > 1)
        {
        	byte[] messageBody = new byte[incomingMessageBytes.length-1];
        	System.arraycopy(incomingMessageBytes, 1, messageBody, 0, messageBody.length);
        	im.parseMessage(messageBody);	
        }
        return im;
    }
    
	private static IncomingMessage GetMessageType(byte command)
	{		
		switch (Protocol.createFromByte(command)) {
		case AUTHENTICATION_ACCEPTED:
			return new AuthenticationAcceptedMessage();
		case AUTHENTICATION_REJECTED:
			return new AuthenticationRejectedMessage();
		case PAIRING_CODE_ACCEPTED:
			return new PairingCodeAcceptedMessage();
		case PINCODE_REJECTED:
			return new PincodeRejectedMessage();
		case FRAME:
			return new FrameMessage();
		case PICTURE_STREAM_START:
			return new PictureStreamStartMessage();
		case PICTURE_STREAM_STOP:
			return new PicureStreamStopMessage();
		case REQUEST_PAIRING_CODE:
			return new RequestPairingCodeMessage();
		default:
			throw new UnsupportedOperationException("Wrong protocol! No such command byte relevant for in Protocol: " + command);			
		}
	}
} 
