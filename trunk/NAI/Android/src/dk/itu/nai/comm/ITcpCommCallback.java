package dk.itu.nai.comm;

import dk.itu.nai.comm.messages.incomming.IncomingMessage;

public interface ITcpCommCallback {
	
	public void onTcpConnectionEstablished(String IP, int port);
	public void onTcpConnectionFailedConnecting();
	public void onTcpConnectionClosedExplicit();
	public void onTcpConnectionLost(String msg);
	
	public void onIncommingMessage(IncomingMessage msg);
	
}
