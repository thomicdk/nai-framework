package dk.itu.nai.comm;

import dk.itu.nai.comm.messages.outgoing.OutgoingMessage;

public interface ICommunicationHandler {
	
	public void start();
	public void killConnection();	
	
	public boolean sendMessage(OutgoingMessage msg);	
}
