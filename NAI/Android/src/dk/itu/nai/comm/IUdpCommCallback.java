package dk.itu.nai.comm;

import java.net.InetAddress;

public interface IUdpCommCallback {
	/**
	 * Method called if something unexpected happend. Is not called if onUdpClientResult has been called, or after kill()
	 */
	public void onUdpClientStopped();
	
	/**
	 * Called if the UdpServer has found a NAI server.
	 * @param IP
	 */
	public void onUdpClientResult(InetAddress IP);
	
	
}
