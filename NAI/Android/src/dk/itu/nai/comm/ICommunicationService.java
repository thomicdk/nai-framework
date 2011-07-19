package dk.itu.nai.comm;

import dk.itu.nai.authentication.ClientCredentials;

public interface ICommunicationService {

	public void registerCallback(ICommunicationServiceCallback callback);
	
	void sendClientCredentials(ClientCredentials credentials);
	
	public void sendPincode(byte[] pincode);
	public void sendColorCode(byte[] colorcode);
	public void sendCalibrationAccepted();
	public void sendRequestCalibration();
	
	public void sendTouchDownEvent(float x, float y);
	public void sendTouchMoveEvent(float x, float y);
	public void sendTouchUpEvent(float x, float y);
	
	public void sendClientPaused();
	public void sendClientResumed();
	
	/* Utils */
	
	public boolean isConnected();
	public void disconnect();
	public void connect();
	public void connectDirect(String IP, int port);

	
}
