package dk.itu.nai.comm;

public interface  ICommunicationServiceCallback {
	
		public void onAuthenticationRejected();
		public void onAuthenticationAccepted();
	
		public void onRequestPairingCode(byte pincodeLength, byte colorCodeLength);
		public void onPairingCodeAccepted();
		public void onPincodeRejected();
		
		public void onPictureStreamStop();
		public void onPictureStreamStart();
		public void onFrame(byte[] image);
		
		/* Connection */
		
		public void onConnected(String IP, int port);
		public void onConnectionLost();
		public void onRequestConnectDirect();
		
}
