package dk.itu.nai.comm;

import dk.itu.nai.comm.messages.incomming.IncomingMessage;

public interface ICommunicationHandlerCallback {

	public void onUdpConnectionTimedOut();
	public void onUdpConnectionError();
	
	public void onTcpConnectionLost();
	public void onTcpConnectionEstablished(String IP, int port);
	public void onTcpConnectionFailedToConnect();
	
	public void onIncommingMessage(IncomingMessage msg);
	
}
