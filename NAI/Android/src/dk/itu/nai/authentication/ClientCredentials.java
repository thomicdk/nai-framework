package dk.itu.nai.authentication;

public abstract class ClientCredentials {
	private String userId;

	abstract public byte[] toByteArray();
	
	public ClientCredentials(String userId) {
		this.userId = userId.trim();
	}
	
	public String getUserId()
	{
		return userId;
	}
}

