package dk.itu.nai.comm.messages;

import dk.itu.nai.comm.Protocol;

public abstract class Message {

	protected Protocol command;
	
	public Message(Protocol command)
	{
		this.command = command;
	}
	
	public Protocol getCommand()
	{
		return command;
	}
}
