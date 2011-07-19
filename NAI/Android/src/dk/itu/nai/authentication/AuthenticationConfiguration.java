package dk.itu.nai.authentication;

import javax.net.ssl.TrustManager;
import javax.net.ssl.X509TrustManager;

public class AuthenticationConfiguration {

	private TrustManager[] trustMgr;
	private static AuthenticationConfiguration instance;
	
	private AuthenticationConfiguration()
	{
    	// Declare a trust manager which accepts everything 
    	// (including our own self-assigned server certificate)
		TrustManager[] trustAllCerts = new TrustManager[]{
			    new X509TrustManager() {
			        public java.security.cert.X509Certificate[] getAcceptedIssuers() {
			            return null;
			        }
			        public void checkClientTrusted(
			            java.security.cert.X509Certificate[] certs, String authType) {
			        }
			        public void checkServerTrusted(
			            java.security.cert.X509Certificate[] certs, String authType) {
			        }
			    }
			};
		
		
		setTrustManager(trustAllCerts);
	}
	
	public void setTrustManager(TrustManager[] trustMgr)
	{
		this.trustMgr = trustMgr;
	}
	
	public TrustManager[] getTrustManager() {
		return trustMgr;
	}
	
	public static AuthenticationConfiguration getInstance()
	{
		if (instance == null)
			instance = new AuthenticationConfiguration();
		return instance;
	}
	
}
