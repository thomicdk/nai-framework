package dk.itu.nai.comm.messages.outgoing;


import java.io.ByteArrayOutputStream;
import java.io.DataOutputStream;
import java.io.IOException;


import dk.itu.nai.comm.Protocol;
import dk.itu.nai.comm.messages.Message;

public abstract class OutgoingMessage extends Message {

	public OutgoingMessage(Protocol command) {
		super(command);
	}

	/**
	 * IO stream is flushed by the sender. Method is not thread safe.
	 *  That is handled byte the method that actually sends.
	 * @param dout
	 * @throws IOException
	 */
	protected abstract byte[] prepareMessageBody() throws IOException;

	public byte[] prepareMessage() throws IOException
	{
		ByteArrayOutputStream baos = new ByteArrayOutputStream();
		DataOutputStream dos = new DataOutputStream(baos);
		dos.writeByte(command.getCommandByte() & 255);
		dos.write(prepareMessageBody());
		return baos.toByteArray();
	}
	
}
