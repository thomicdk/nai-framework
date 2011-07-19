package dk.itu.nai.comm;

import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.net.DatagramPacket;
import java.net.InetAddress;
import java.net.MulticastSocket;
import java.net.SocketTimeoutException;

import android.util.Log;

public class UdpComm extends Thread {

	// http://code.google.com/p/android/issues/detail?id=8407

	private final String serverIdMatch;
	private byte[] serverIdMatchByteArray = null; 
	private final int UDPGroupPort = 10035;
	private final String UDPGroupIP = "224.0.2.0";

	private final String ME = "UdpComm";

	private IUdpCommCallback callback;
	private boolean _running = false;
	private boolean _closeOnFirstHit = false;

	/**
	 * UdpComm is not started by default. You need to call start() first!
	 * If everything goes as plannen, you will not get a notofication when the service stops, unless it is unexpected.
	 * calling kill() will not result in a call to onUdpClientStopped.
	 * @param callback
	 * @param closeOnFirstHit
	 */
	public UdpComm(IUdpCommCallback callback, boolean closeOnFirstHit)
	{
		Log.d(ME, "UdpComm constuctor");
		serverIdMatch = CommConfiguration.SERVER_UDP_ID_TOKEN;
		this.callback = callback;
		this._closeOnFirstHit = closeOnFirstHit;
		try {
			serverIdMatchByteArray = serverIdMatch.getBytes("UTF-8");
		} catch (UnsupportedEncodingException ignore){}
		
		setDaemon(true);
	}


	@Override
	public void run() {
		Log.d(ME, "run()");
		MulticastSocket mSocket = null;
		
		try {
			mSocket = new MulticastSocket();
			mSocket.setSoTimeout(500);
			mSocket.setTimeToLive(2);
						
			byte[] response_msg = new byte[serverIdMatchByteArray.length];
			byte[] request_msg = serverIdMatch.getBytes("UTF-8");
			DatagramPacket requestPacket = new DatagramPacket(request_msg, request_msg.length, InetAddress.getByName(UDPGroupIP), UDPGroupPort);
			DatagramPacket responsePacket;
			Log.d(ME, "run() before while loop");
			while(_running)
			{
				Thread.yield();
//				Log.d(ME, "Before sending packet");
				mSocket.send(requestPacket);
				
				responsePacket = new DatagramPacket(response_msg, response_msg.length );
				
//				Log.d(ME, "run() before recieve packet");
				try
				{
					mSocket.receive(responsePacket);
				}catch (SocketTimeoutException e) {
					continue;
				}
				
				Log.d(ME, "Got packet.");
				Log.d(ME, String.format("%s:%d %d", responsePacket.getAddress().toString(), responsePacket.getPort(), responsePacket.getLength()));
				String result = new String(responsePacket.getData(), "UTF-8");
				Log.d(ME, "->" + result);
				if (_running && result.startsWith(serverIdMatch))
				{
					Log.d(ME, "packet data match!");
					if (callback != null && responsePacket.getAddress() != null)
						callback.onUdpClientResult(responsePacket.getAddress());
				
					if (_closeOnFirstHit)
						_running = false;					
				}
			}
			mSocket.close();
			return;

		} catch (UnsupportedEncodingException e) {
			Log.d(ME, "Could not convert!: " + e.getMessage());
			e.printStackTrace();
		} catch (IOException e) {
			_running = false;
			Log.e(ME, "Got IO EXCEPTION. Closing connection");				
			e.printStackTrace();
		}

		if (mSocket!= null && !mSocket.isClosed())
			mSocket.close();
		mSocket = null;
		try {
			callback.onUdpClientStopped();
		}catch (Exception ignore) {}
	}

	public void kill() {
		Log.d(ME, "kill()");
		_running = false;
	}

	@Override
	public synchronized void start() {
		Log.d(ME, "start()");
		_running = true;
		super.start();
	}

	public boolean isRunning()
	{
		return _running;
	}

}
