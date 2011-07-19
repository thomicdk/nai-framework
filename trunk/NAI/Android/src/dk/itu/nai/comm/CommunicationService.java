package dk.itu.nai.comm;


import android.app.Service;
import android.content.Intent;
import android.os.Binder;
import android.os.IBinder;
import android.util.Log;
import dk.itu.nai.authentication.ClientCredentials;
import dk.itu.nai.comm.messages.incomming.FrameMessage;
import dk.itu.nai.comm.messages.incomming.IncomingMessage;
import dk.itu.nai.comm.messages.incomming.RequestPairingCodeMessage;
import dk.itu.nai.comm.messages.outgoing.CalibrationAcceptedMessage;
import dk.itu.nai.comm.messages.outgoing.ClientCredentialsMessage;
import dk.itu.nai.comm.messages.outgoing.ClientPausedMessage;
import dk.itu.nai.comm.messages.outgoing.ClientResumedMessage;
import dk.itu.nai.comm.messages.outgoing.ColorCodeMessage;
import dk.itu.nai.comm.messages.outgoing.OutgoingMessage;
import dk.itu.nai.comm.messages.outgoing.PincodeMessage;
import dk.itu.nai.comm.messages.outgoing.RequestCalibrationMessage;
import dk.itu.nai.comm.messages.outgoing.TouchDownMessage;
import dk.itu.nai.comm.messages.outgoing.TouchMoveMessage;
import dk.itu.nai.comm.messages.outgoing.TouchUpMessage;
import dk.itu.nai.util.TaskQueue;

public class CommunicationService extends Service implements ICommunicationService, ICommunicationHandlerCallback {
	
	/* Fields */
	private final String ME = "CommunicationService"; 
	private final Binder mBinder = new CommunicationServiceBinder();
	private final int UDP_CONNECTION_TIMEOUT = 2000;
	private final TaskQueue tasks = new TaskQueue();
	private ICommunicationServiceCallback callback;
	private boolean hasConnection = false;
	private ICommunicationHandler client;	
	
	private final boolean D = false;
	
	/* nested classes */
	
	public class CommunicationServiceBinder extends Binder {
		public ICommunicationService getService()
		{
			return CommunicationService.this;
		}
	}
	
	
	/* Constructor */
	public CommunicationService() {
		if (D) Log.d(ME, "Constructor");
	}

	/* Basic class events */
	@Override
	public void onCreate() {
		if (D) Log.d(ME, "onCreate()");
		super.onCreate();		
		tasks.start();
	}
	
	@Override
	public IBinder onBind(Intent arg0) {
		Log.d(ME, "onBind");
		return mBinder;
	}

	@Override
	public boolean onUnbind(Intent intent) {
		Log.d(ME, "onUnbind");
		callback = null;
		return false;			
	}
	
	@Override
	public int onStartCommand(Intent intent, int flags, int startId) {
	Log.d(ME, "onStartCommand");
	        return Service.START_STICKY;
	}
	
	@Override
	public void onDestroy() {	
		Log.d(ME, "onDestroy");
		
		synchronized (this) {
			tasks.stop();
			if (client != null)
				client.killConnection();
		}
		super.onDestroy();		
	}

	/* - - ICommunicationService - - */
	
	@Override
	public synchronized void registerCallback(ICommunicationServiceCallback callback) {
		Log.d(ME, "registerCallback");
		this.callback = callback;		
	}

	/* Send data */
	
	@Override
	public void sendClientCredentials(ClientCredentials credentials)
	{
		sendMessage("sendClientCredentials", new ClientCredentialsMessage(credentials));
	}
	
	@Override
	public void sendColorCode(byte[] colorcode) {
		sendMessage("sendColorCode", new ColorCodeMessage(colorcode));		
	}

	@Override
	public void sendPincode(final byte[] pincode) {
		sendMessage("sendPincode", new PincodeMessage(pincode));
	}

	@Override
	public void sendCalibrationAccepted() {
		sendMessage("sendCalibrationAccepted", new CalibrationAcceptedMessage());
	}
	
	@Override
	public void sendClientPaused() {
		sendMessage("sendClientPaused", new ClientPausedMessage());	
	}

	@Override
	public void sendClientResumed() {
		sendMessage("sendClientResumed", new ClientResumedMessage());	
	}

	@Override
	public void sendRequestCalibration() {
		sendMessage("sendRequestCalibration", new RequestCalibrationMessage());
	}

	@Override
	public void sendTouchDownEvent(float x, float y) {
		sendMessage("sendTouchDownEvent", new TouchDownMessage(x, y));
	}

	@Override
	public void sendTouchMoveEvent(float x, float y) {
		sendMessage("sendTouchMoveEvent", new TouchMoveMessage(x,y));
	}

	@Override
	public void sendTouchUpEvent(float x, float y) {
		sendMessage("sendTouchUpEvent", new TouchUpMessage(x, y));	
	}

	private void sendMessage(final String log, final OutgoingMessage msg)
	{
		tasks.addTask(new Runnable() {			
			@Override
			public void run() {
				Log.d(ME, log);
				synchronized (client) {
					if (hasConnection)
						client.sendMessage(msg);
				}
			}
		});
	}
	
/*  incoming data */
	
	@Override
	public void onIncommingMessage(final IncomingMessage msg) {
		tasks.addTask(new Runnable() {
			public void run() {
				handleIncommingMessage(msg);				
			}
		});		
	}
	
	private void handleIncommingMessage(IncomingMessage msg)	
	{	
		if (D) Log.d(ME, "handleIncommingMessage: " + msg.getCommand().toString());
		if (callback == null)
		{
			Log.d(ME, "callback was null. skipping incomming message:" + msg.getCommand().toString());
			return;
		}
		try {
		switch (msg.getCommand()) {
		case AUTHENTICATION_ACCEPTED:
			callback.onAuthenticationAccepted();
			break;
		case AUTHENTICATION_REJECTED:
			callback.onAuthenticationRejected();
			break;
		case FRAME:			
			callback.onFrame(((FrameMessage) msg).getImageBytes());
			break;		
		case PAIRING_CODE_ACCEPTED:
			callback.onPairingCodeAccepted();
			break;
		case PICTURE_STREAM_START:
			callback.onPictureStreamStart();
			break;
		case PICTURE_STREAM_STOP:
			callback.onPictureStreamStop();
			break;
		case PINCODE_REJECTED:
			callback.onPincodeRejected();
			break;
		case REQUEST_PAIRING_CODE:
			RequestPairingCodeMessage rMsg = (RequestPairingCodeMessage) msg;
			callback.onRequestPairingCode(rMsg.getPincodeLength(), rMsg.getColorCodeLength());
			break;
		default:
			Log.d(ME, "handleIncommingMessage. Unknown protocol: " + msg.getCommand().toString());
			break;
		}
		} catch (Exception e)
		{
			Log.e(ME, "handeIncommingMessage. Get exception trying to invoke on callback:" + msg.getCommand().toString());
			Log.e(ME, e.getMessage());		
		}
	}
	
	/* Connection status */
	
	@Override
	public void connectDirect(final String IP,final int port) {
		 tasks.addTask(new Runnable() {
			
			@Override
			public void run() {
				if (D) Log.d(CommunicationService.this.ME, "connectDirect");
				CommunicationService.this.client = new CommunicationHandler(CommunicationService.this, IP, port);
				CommunicationService.this.client.start();				
			}
		});		
	}

	@Override
	public synchronized void disconnect() {
		tasks.addTask(new Runnable() {
			
			@Override
			public void run() {
				if (client!= null)
					client.killConnection();		
			}
		});
	}
		
	@Override
	public void connect() {		
		tasks.addTask(new Runnable() {
			
			@Override
			public void run() {
				synchronized (CommunicationService.this) {
					Log.d(ME, "connect()");
					if (!hasConnection)
						CommunicationService.this.client = new CommunicationHandler(CommunicationService.this, UDP_CONNECTION_TIMEOUT);
					CommunicationService.this.client.start();
				}								
			}
		});		
	}

	@Override
	public synchronized boolean isConnected() {
		return hasConnection;		
	}

	@Override
	public synchronized void onTcpConnectionEstablished(final String IP, final int port) {
		hasConnection = true;
		tasks.addTask(new Runnable() {			
			@Override
			public void run() {
				if (callback != null)
				callback.onConnected(IP, port);				
			}
		});
	}

	@Override
	public synchronized void onTcpConnectionFailedToConnect() {
		Log.d(CommunicationService.this.ME, "onTcpConnectionFailedToConnect");
		tasks.addTask(new Runnable() {			
			@Override
			public void run() {
				CommunicationService.this.hasConnection = false;
				Log.d(CommunicationService.this.ME, "onTcpConnectionFailedToConnect (runnable)");
				if (callback!= null)
					CommunicationService.this.callback.onConnectionLost();						
			}
		});
	}

	@Override
	public synchronized void onTcpConnectionLost() {
		tasks.addTask(new Runnable() {			
			@Override
			public void run() {
				CommunicationService.this.hasConnection = false;
				Log.d(CommunicationService.this.ME, "onTcpConnectionLost");
				if (callback!= null)
					CommunicationService.this.callback.onConnectionLost();				
			}
		});	
	}
	

	@Override
	public synchronized void onUdpConnectionError() {
		hasConnection = false;
		tasks.addTask(new Runnable() {
			
			@Override
			public void run() {
				if (callback != null)
					callback.onRequestConnectDirect();				
			}
		});			
	}

	@Override
	public synchronized void onUdpConnectionTimedOut() {
		hasConnection = false;
		if (callback != null)
			callback.onRequestConnectDirect();		
	}
	
	
	
	
}
