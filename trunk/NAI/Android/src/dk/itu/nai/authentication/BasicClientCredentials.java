package dk.itu.nai.authentication;

import java.io.UnsupportedEncodingException;


public class BasicClientCredentials extends ClientCredentials {

	public BasicClientCredentials(String userId) {
		super(userId);
	}
	
	@Override
	public byte[] toByteArray() {		
		try {
			return	getUserId().trim().getBytes("UTF-8");
		} catch (UnsupportedEncodingException e) {
			return null;
		}		
	}
}
