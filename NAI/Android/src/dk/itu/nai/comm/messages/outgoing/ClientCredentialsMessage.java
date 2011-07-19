package dk.itu.nai.comm.messages.outgoing;

import java.io.IOException;

import dk.itu.nai.authentication.ClientCredentials;
import dk.itu.nai.comm.Protocol;

public class ClientCredentialsMessage extends OutgoingMessage {
	
	private ClientCredentials credentials;

	public ClientCredentialsMessage(ClientCredentials credentials) {
		super(Protocol.AUTHENTICATION);
		this.credentials = credentials;	
	}

	@Override
	protected byte[] prepareMessageBody() throws IOException {
		return credentials.toByteArray();
	}

}
