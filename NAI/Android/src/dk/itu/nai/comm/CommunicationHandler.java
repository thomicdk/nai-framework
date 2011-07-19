package dk.itu.nai.comm;

import java.net.InetAddress;

import android.util.Log;
import dk.itu.nai.comm.messages.incomming.IncomingMessage;
import dk.itu.nai.comm.messages.outgoing.OutgoingMessage;

public class CommunicationHandler extends Thread implements ICommunicationHandler, ITcpCommCallback, IUdpCommCallback {

	private ICommunicationHandlerCallback callback;

	private String _remoteIP ="";
	private int _remotePort = 8888;
	private boolean _useUDP = false;
	private boolean _udpSessionRunning = false;
	private int _udpTimeout = 10000;
	private boolean _initError = false;

	private TcpComm tcp = null;
	private boolean _tcpIsConnected = false;

	private final boolean D = false; 

	private final String ME = "CommunicationHandler";

	/**
	 * Initiate a connection use Multicast to obtain the IP address.
	 * Must be started explicit
	 * @param callback
	 * @param timeout
	 */
	public CommunicationHandler(ICommunicationHandlerCallback callback,  int timeout)
	{
		Log.d(ME, "constructor - use UDP");
		this.callback = callback;
		_udpTimeout = timeout;
		_useUDP = true;		

		setDaemon(true);
	}

	/**
	 * Setup up a connection to the IP address directly. Without using UDP to listen for relevant servers
	 * @param callback
	 * @param IP
	 * @param port
	 */
	public CommunicationHandler(ICommunicationHandlerCallback callback, String IP, int port)
	{
		Log.d(ME, "constructor - use TCP");
		this.callback = callback;
		_remoteIP = IP; _remotePort = port;
		_useUDP = false;
	}

	@Override
	public void run() {
		Log.d(ME, "run()");
		if (_useUDP && !_initError)
		{	
			_udpSessionRunning = true;
			Log.d(ME, "Looking for server via UDP multicasts");
			UdpComm udp = new UdpComm(this, true);
			Long timerStart = System.currentTimeMillis();
			udp.start();
			while (_udpSessionRunning && !_initError)
			{
				if (System.currentTimeMillis() - timerStart > _udpTimeout)
				{
					Log.d(ME, "Before UDP timeout");
					callback.onUdpConnectionTimedOut();
					udp.kill();
					cleanUp();
					return;
				}					
				Thread.yield();
			}
			udp.kill();

		}
		// Did something go wrong? Should we continue with establishing TcpConnection?
		if (_initError)
		{
			Log.d(ME, "befor onUdpConnectionErrror");
			callback.onUdpConnectionError();
			cleanUp();
			return;
		}

		Log.d(ME, "Got the server address: " + _remoteIP);
		// Continues with establishing the TcpConnection

		tcp = new TcpComm(this, _remoteIP, _remotePort);
	}

	private void cleanUp()
	{
		_remoteIP = null;
		_udpSessionRunning = false;
		_initError = false;

	}

	public void killConnection()
	{
		synchronized (tcp) {
			if (tcp!= null)
			{
				tcp.closeConnection();				
			}
		}
	}
	
	/* Send Data */
	
	public boolean sendMessage(OutgoingMessage msg) {
		synchronized (tcp) {
			if (_tcpIsConnected)
				return tcp.sendMessage(msg);
		}
		return false;
	}
		
	/* - - - recieve data - - */ 	
	
	public void onIncommingMessage(IncomingMessage msg) {
		if (D) Log.d(ME, "newIncommingMessage");
		callback.onIncommingMessage(msg);
	}
	
	/* ITcpCommCallback */

	@Override
	public void onTcpConnectionClosedExplicit() {
		Log.d(ME, "onTcpConnectionClosedExplicit()");
		synchronized (tcp) {
			_tcpIsConnected = false;
		}
		callback.onTcpConnectionLost();
	}

	@Override
	public void onTcpConnectionEstablished(String IP, int port) {
		Log.d(ME, "onTcpConnectionEstablished()");
		synchronized (tcp) {
			_tcpIsConnected = true;
		}
		callback.onTcpConnectionEstablished(IP, port);
	}

	@Override
	public void onTcpConnectionFailedConnecting() {
		Log.d(ME, "onTcpConnectionFailedConnecting()");
		synchronized (tcp) {
			_tcpIsConnected = false;
		}
		callback.onTcpConnectionFailedToConnect();		
	}

	@Override
	public void onTcpConnectionLost(String msg) {
		Log.d(ME, "onTcpConnectionLost()");
		synchronized (tcp) {
			_tcpIsConnected = false;
		}
		callback.onTcpConnectionLost();
	}

	/* IUdpCommCallback */

	@Override
	public void onUdpClientResult(InetAddress IP) {
		Log.d(ME, "onUdpClientResult()");
		synchronized (_remoteIP) {
			this._remoteIP = IP.getHostAddress();
			_udpSessionRunning = false;
		}
	}

	@Override
	public void onUdpClientStopped() {
		synchronized (_remoteIP) {
			if (_useUDP)
			{
				_initError = true;
				_udpSessionRunning = false;			
			}
		}

	}








}
