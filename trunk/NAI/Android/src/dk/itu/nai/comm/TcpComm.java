package dk.itu.nai.comm;

import java.io.ByteArrayOutputStream;
import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.net.Socket;
import java.net.UnknownHostException;
import java.security.KeyManagementException;
import java.security.NoSuchAlgorithmException;

import javax.net.ssl.SSLContext;
import javax.net.ssl.SSLSocket;
import javax.net.ssl.SSLSocketFactory;

import android.util.Log;
import dk.itu.nai.authentication.AuthenticationConfiguration;
import dk.itu.nai.comm.messages.incomming.IncomingMessage;
import dk.itu.nai.comm.messages.outgoing.OutgoingMessage;

public class TcpComm extends Thread {

	private ITcpCommCallback callback;
	private int port;
	private String IP;
	private Socket _socket;
	private DataInputStream _in;
	private DataOutputStream _out;
	private final boolean D = false;
	
	private final String ME = "TcpComm";

	/**
	 * The TcpComm auto starts, and will callback to notify on the states.
	 * Do not attempt to re-invoke run. Create another instance to make a second attempt.
	 * @param callback
	 * @param IP
	 * @param port
	 */
	public TcpComm(ITcpCommCallback callback, String IP, int port)
	{
		Log.d(ME, "constructor");
		this.callback = callback;
		this.IP = IP;
		this.port = port;		
		setDaemon(true);
		start();
		Log.d(ME, "constructor - end");
	}

	/**
	 * Don't call this directly. Is used by the threading system.
	 * If you wish to reconnect, then create a new instance of TcpComm.
	 */
	@Override
	public void run() {
		try {
			if (D) Log.d(ME, "before socket creation");
			_socket = getSSLSocket(IP, port);
			if (D) Log.d(ME, "after socket creation");
				StringBuilder str = new StringBuilder();
				str.append("_socket.getLocalPort():");
				str.append(_socket.getLocalPort());
				str.append("\n_socket.getInetAddress():");
				str.append(_socket.getInetAddress());
				str.append("\n_socket.isConnected():");
				str.append(_socket.isConnected());
			if(D)	Log.d(ME, str.toString());		
			_in = new DataInputStream(_socket.getInputStream());
			_out = new DataOutputStream(_socket.getOutputStream());						
		} catch (UnknownHostException e) {		
			Log.d(ME, "UnknownHostException");
			onIOException(e.getMessage(), false);
			callback.onTcpConnectionFailedConnecting();
			return;
		} catch (IOException e) {
			Log.d(ME, "IOExecption");
			onIOException(e.getMessage(), false);
			callback.onTcpConnectionFailedConnecting();
			return;
		}	
		callback.onTcpConnectionEstablished(this.IP, this.port);
		try {
			Thread.sleep(200); // wait before doing the reading!
		} catch (InterruptedException ignore) {}  
		
		while(isConnected())
		{
			synchronized (_in) {
				try {
					if (D) Log.d(ME, "before incoming byte");
					IncomingMessage msg = readMessage();
					if (D) Log.d(ME, "Sending message to callback: " + msg.getCommand().toString());
					callback.onIncommingMessage(msg);
					Thread.yield();
				} catch (Exception e) {
					onIOException(e.getMessage());
					return; // stop running!
				}				
			}			
		}		
	}

	private Socket getSSLSocket(String ip, int port)
	{
    	
		try {
			SSLContext sc = SSLContext.getInstance("TLS");
	    	sc.init(null, AuthenticationConfiguration.getInstance().getTrustManager(), new java.security.SecureRandom());
	    	
	    	SSLSocketFactory factory = sc.getSocketFactory();
			
			return (SSLSocket) factory.createSocket(ip, port);
			
		} catch (UnknownHostException e) {
			Log.d("ME", "UnKnownHostExeption");
			Log.e("ME", e.getMessage());
		} catch (IOException e) {
			Log.d("ME", "IOExeption");
			Log.e("ME", e.getMessage());
		} catch (NoSuchAlgorithmException e) {
			Log.d("ME", "NoSuchAlgorithmException");
			Log.e("ME", e.getMessage());
		} catch (KeyManagementException e) {
			Log.d("ME", "KeyManagementException");
			Log.e("ME", e.getMessage());
		}
		return null;
	}
	
    private IncomingMessage readMessage() throws IOException
    {
        // Read the header of the PDU
        int messageBodyLength = _in.readInt();
        
        // Read the payload (the Message layer SDU)
		byte[] messageBytes = new byte[messageBodyLength];
		_in.readFully(messageBytes);
        return IncomingMessage.parse(messageBytes);
    }
    
    private byte[] messageToPDU(OutgoingMessage message) throws IOException
    {
    	ByteArrayOutputStream baos = new ByteArrayOutputStream();
    	DataOutputStream dos = new DataOutputStream(baos);

        // Prepare the payload of the PDU
        byte[] messageBytes = message.prepareMessage();
        // Write the header of the PDU
		dos.writeInt(messageBytes.length);
        // Write the payload of the PDU
        dos.write(messageBytes);
        return baos.toByteArray();
    }
    
	
	public boolean sendMessage(OutgoingMessage msg)
	{
		if (!isConnected())
			return false;	
		try {
			synchronized (_out) {
				Log.d(ME, "sendMessage: " + msg.getCommand().toString());
				_out.write(messageToPDU(msg));
				_out.flush();
			}
		} catch (IOException e) {
			onIOException(e.getMessage());
			return false;
		}
		return true;
	}

	
	private void onIOException(String msg, boolean doConnectionLostCallBack)
	{
		Log.e(ME, "onIEception: " + msg);
		if (isConnected())
			try {
				_socket.close();
			} catch (IOException ignore) {}
		_socket = null;
		_out = null;
		_in = null;
		if (doConnectionLostCallBack)
			callback.onTcpConnectionLost(msg);
	}
	private void onIOException(String msg)
	{
		onIOException(msg, true);
	}
	
	public boolean isConnected()
	{
		return _socket != null && _socket.isConnected();
	}
	
	public void closeConnection()
	{
		if(D) Log.d(ME, "closeConnection");
		if (!isConnected())
			return;
		try {
			_socket.close();
		} catch (IOException ignore) {}
		_socket = null;
		_in = null;
		_out = null;
		callback.onTcpConnectionClosedExplicit();
	}

	
}
