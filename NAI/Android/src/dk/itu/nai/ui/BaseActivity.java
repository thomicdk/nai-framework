package dk.itu.nai.ui;

import java.io.ByteArrayInputStream;

import android.app.Activity;
import android.app.Dialog;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.ServiceConnection;
import android.graphics.drawable.Drawable;
import android.os.Bundle;
import android.os.IBinder;
import android.os.PowerManager;
import android.os.PowerManager.WakeLock;
import android.preference.PreferenceManager;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.MotionEvent;
import android.view.View;
import android.view.View.OnTouchListener;
import android.widget.ImageView;
import android.widget.Toast;
import android.widget.ImageView.ScaleType;
import dk.itu.nai.R;
import dk.itu.nai.authentication.AuthenticationHandler;
import dk.itu.nai.authentication.ClientCredentials;
import dk.itu.nai.comm.CommConfiguration;
import dk.itu.nai.comm.CommunicationService;
import dk.itu.nai.comm.ICommunicationService;
import dk.itu.nai.comm.ICommunicationServiceCallback;
import dk.itu.nai.comm.CommunicationService.CommunicationServiceBinder;
import dk.itu.nai.state.State;
import dk.itu.nai.ui.dialogs.DialogFactory;
import dk.itu.nai.ui.dialogs.DialogHandler;
import dk.itu.nai.ui.menu.MenuFactory;
import dk.itu.nai.ui.menu.MenuHandler;
import dk.itu.nai.ui.preferences.ApplicationPreferences;


public class BaseActivity extends Activity implements ICommunicationServiceCallback, OnTouchListener {

	/* Fields */ 

	private static final String ME = "BaseActivity";

	private final int REQUEST_PINCODE_ACTIVITY = 555;
	private final int REQUEST_CODE_PREFERENCES = 444;	
		
	private State currentState = State.AUTHENTICATING;
	private Intent pincodeIntent;

	private final boolean D = true;

	/* Views */
	private ImageView imgV;	

	/* Communication Service */

	private ICommunicationService comm;
	private boolean mCommServiceBound = false;

	private ServiceConnection mServiceConnection = new ServiceConnection()
	{
		private final String ME ="ServiceConnection";
		@Override
		public void onServiceDisconnected(ComponentName name) {
			if (D) Log.d(ME, "Service disconnected");
			synchronized (BaseActivity.this) {
				mCommServiceBound = false;
				comm = null;
			}						
		}

		@Override
		public void onServiceConnected(ComponentName name, IBinder service) {
			if (D) Log.d(ME, "Service connected");
			synchronized (BaseActivity.this) {
				comm = ((CommunicationServiceBinder) service).getService();
				mCommServiceBound = true;
				comm.registerCallback(BaseActivity.this);
				if (!comm.isConnected()){
					showWaitingForConnectionDialog();
					comm.connect();
				}
			}					
			
		}
	};

	private WakeLock wakeLock;

	private AuthenticationHandler authenticationcontroler;

	/* Activity life events */

	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.baseactivity);

		// ApplicationPreferences 
		PreferenceManager.setDefaultValues(this, R.xml.preferences, false);
		
		// CommunicationPreferences
		CommConfiguration.SERVER_UDP_ID_TOKEN = getResources().getString(R.string.server_udp_id_token);

		// Screen and touch listeners
		imgV = (ImageView) findViewById(R.id.imgVTouchScreen);
		imgV.setOnTouchListener(this);

		// Keep alive manager
		PowerManager pm = (PowerManager) getSystemService(Context.POWER_SERVICE);
		wakeLock = pm.newWakeLock(PowerManager.FULL_WAKE_LOCK | PowerManager.ACQUIRE_CAUSES_WAKEUP, "NAIClient");
		wakeLock.setReferenceCounted(false);
		
		// Start Communication Service.
		doStartService();
	}

	@Override
	protected void onResume() {    
		super.onResume();
		Log.d(ME, "onResume");

		if (mCommServiceBound)
			comm.sendClientResumed();
		else
			doBindService();		
		
		//acquireWakeLock();
	}

	@Override
	protected void onPause() {    
		super.onPause();
		Log.d(ME, "onPause. ");
		if (mCommServiceBound)		
			comm.sendClientPaused();
		releaseWakeLock();
	}

	@Override
	protected void onDestroy() {	
		Log.d(ME, "onDestroy()");
		releaseWakeLock();
		doUnbindService();
		doStopService();
		super.onDestroy();
		System.exit(0);
	}

	@Override
	public void onBackPressed() {		
		showDialog(DialogHandler.DIALOG_EXIT);			
	}


	/* UI helper methods */


	@Override
	public boolean onCreateOptionsMenu(Menu menu) {    
		Log.d(ME, "onCreateOptionsMenu");
		MenuFactory.createAllMenus(menu);
		return true;
	}
	@Override
	public boolean onPrepareOptionsMenu(Menu menu) {
		Log.d(ME, "onPrepareOptionsMenu");
		if (mCommServiceBound && comm.isConnected())
			menu.setGroupVisible(MenuFactory.GROUP_DYNAMIC, true);		
		else
			menu.setGroupVisible(MenuFactory.GROUP_DYNAMIC, false);
		return true;
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		Log.d(ME, "onOptionsItemSelction");
		MenuHandler.onOptionsItemSelected(this, item);
		return super.onOptionsItemSelected(item);
	}

	@Override
	protected Dialog onCreateDialog(int id) {
		return DialogHandler.onCreateDialog(id, this); 
	}

	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
		Log.d(ME, "onActivityResult " + String.format("resultcode: %d, requestcode: %d", resultCode, requestCode));
		if (requestCode == REQUEST_PINCODE_ACTIVITY)
		{
			Log.d(ME, "REQUEST_PINCODE_ACTIVITY");
			if(resultCode == RESULT_OK)
			{
				comm.sendPincode(data.getExtras().getByteArray(PincodeActivity.BUNDLE_KEY_PINCODE_AS_BYTES));
			}
			else if (resultCode == PincodeActivity.RESULT_FORCE_CLOSE)
			{
				finish();
			}
			else if (currentState == State.PAIRING && mCommServiceBound && comm.isConnected())
			{
				Log.d(ME, "currentState == State.PAIRING");
				startActivityForResult(pincodeIntent, REQUEST_PINCODE_ACTIVITY);			
			}
		}
		else if (requestCode == AuthenticationHandler.REQUEST_AUTHENTICATION_ACTIVITY)
		{
			ClientCredentials credentials = this.authenticationcontroler.onActivityResult(resultCode, data);
			if (credentials != null)
				sendCredentials(credentials);
		}
	}

	private void showWaitingForConnectionDialog()
	{	
		if(D) Log.d(ME, "showWaitingForConnectionDialog");
		showDialog(DialogHandler.DIALOG_WAITING_FOR_CONNECTION);
	}

	private void hideWaitingForConnectionDialog()
	{
		if(D) Log.d(ME, "hideWaitingForConnectionDialog");
		DialogFactory.getWaitingForConnectionDialog(this).hide();			
	}

	public void launchPreferences(boolean showAlertDialog)
	{
		Intent intent = new Intent(this, ApplicationPreferences.class);
		intent.putExtra("dialog", showAlertDialog);
		startActivityForResult(intent, REQUEST_CODE_PREFERENCES);
	}

	private void acquireWakeLock()
	{
		if (wakeLock != null //&& !wakeLock.isHeld()
				)		
		{
			Log.d(ME, "wake lock acquired");
			wakeLock.acquire();
		}
	}
	
	private void releaseWakeLock()
	{
		if (wakeLock != null 
//				&& wakeLock.isHeld()
				)
		{
			Log.d(ME, "wake lock released");
			wakeLock.release();
		}
			
	}

	/* Connection */

	public void doBindService()
	{
		Log.d(ME, "doBindService");
		bindService(new Intent(this, CommunicationService.class), mServiceConnection, BIND_AUTO_CREATE );
	}

	public void doUnbindService()
	{    	
		Log.d(ME, "doUnbindService");

		if (mCommServiceBound)
		{   
			Log.d(ME, "unbinding");
			comm.registerCallback(null);    	
			unbindService(mServiceConnection);
			mCommServiceBound = false;    	
		}
	}

	private void doStartService()
	{
		Log.d(ME, "doStartService");
		startService(new Intent(this, CommunicationService.class));
	}

	private void doStopService()
	{
		Log.d(ME, "doStopService");
		stopService(new Intent(this, CommunicationService.class));
	}

	public void connect()
	{
		runOnUiThread(new Runnable() {

			@Override
			public void run() {
				if (D) Log.d(BaseActivity.ME, "connect()");
				showWaitingForConnectionDialog();
				comm.connect();				
			}
		});

	}

	public void connectDirect(final String IP,final int port) {
		runOnUiThread(new Runnable() {			
			@Override
			public void run() {
				if(D) Log.d(BaseActivity.ME, "connectDirect(): " + IP + ":" + port);
				showWaitingForConnectionDialog();				
				comm.connectDirect(IP, port);					
			}
		});

	}

	/* remote method invokation */

	public boolean requestCalibration()
	{
		if (mCommServiceBound)
		{
			comm.sendRequestCalibration();			
			return true;
		}
		return false;
	}

	public void disconnect()
	{
		if (mCommServiceBound)
			comm.disconnect();
	}

	
	public void sendCredentials(ClientCredentials credentials) {
		if (mCommServiceBound)
			comm.sendClientCredentials(credentials);		
	}
	

	public void sendAcceptCalibration()
	{
		if (mCommServiceBound)
			comm.sendCalibrationAccepted();
	}		

	/* IServerCommunicationServiceCallback */

	@Override
	public void onConnected(final String IP, final int port) {
		runOnUiThread(new Runnable() {

			@Override
			public void run() {
				Toast.makeText(BaseActivity.this, getResources().getString(R.string.baseActivityToastsConnected), Toast.LENGTH_SHORT).show();				
				hideWaitingForConnectionDialog();
				ApplicationPreferences.updateServerPreferences(IP, port, BaseActivity.this);			
				BaseActivity.this.authenticationcontroler.requestCredentials();
			}
		});
	}


	@Override
	public void onConnectionLost() {
		runOnUiThread(new Runnable() {

			@Override
			public void run() {
				BaseActivity.this.finishActivity(REQUEST_PINCODE_ACTIVITY);
				Toast.makeText(BaseActivity.this, getResources().getString(R.string.baseActivityToastsConnectionLost), Toast.LENGTH_SHORT).show();
				hideWaitingForConnectionDialog();
				connect();
			}
		});
	}

	@Override
	public void onFrame(final byte[] image) {
		if (D) Log.d(ME, "onFrame");
		synchronized (currentState) {
			if (currentState != State.STREAMING)
				return;
		}		
		if (D) Log.d(ME, "onFrame. state STREAMING");
		runOnUiThread(new Runnable() {

			@Override
			public void run() {
				Drawable drawable = Drawable.createFromStream(new ByteArrayInputStream(image), "src");
				synchronized (imgV) {
					imgV.setImageDrawable(drawable);
				}
				imgV.invalidate();
			}
		});			
	}

	
	@Override
	public void onAuthenticationAccepted() {
		runOnUiThread(new Runnable() {
			
			@Override
			public void run() {
				Log.d(ME, "onAuthenticationAccepted");
				BaseActivity.this.authenticationcontroler.onAuthenticationAccepted();
				setState(State.PAIRING);		
			}
		});
	}

	@Override
	public void onAuthenticationRejected() {
		runOnUiThread(new Runnable() {
			
			@Override
			public void run() {
				Log.d(ME, "onAuthenticationRejected");
				BaseActivity.this.authenticationcontroler.onAuthenticationRejected();		
			}
		});
	}
	
	
	@Override
	public void onPairingCodeAccepted() {
		runOnUiThread(new Runnable() {
			@Override
			public void run() {
				if (D) Log.d(ME, "onPairingCodeAccepted");
				setState(State.CALIBRATION);
				finishActivity(REQUEST_PINCODE_ACTIVITY);				
			}
		});
	}

	@Override
	public void onPictureStreamStart() {
		runOnUiThread(new Runnable() {			
			@Override
			public void run() {
				setState(State.STREAMING);
				BaseActivity.this.acquireWakeLock();
//				Toast.makeText(BaseActivity.this, "Picture screen started", Toast.LENGTH_SHORT).show();
			}
		});

	}

	/*  States */

	public void setState(final State state)
	{
		if (D) Log.d(ME, "setState: " + state.toString());
		synchronized (currentState) {
			currentState = state;
			switch (state) {
			case CALIBRATION:
				acquireWakeLock();
				synchronized (imgV) {
					imgV.setScaleType(ScaleType.CENTER_CROP);
					imgV.setImageDrawable(getResources().getDrawable(R.drawable.calibration));		
				}
				imgV.invalidate();
				break;
			case  STREAMING:
				//acquireWakeLock();
			default:
				currentState = state;
				synchronized (imgV) {
					imgV.setScaleType(ScaleType.FIT_CENTER);					;
				}
				break;
			}
		}
	}

	@Override
	public void onPictureStreamStop() {
				runOnUiThread(new Runnable() {
					
					@Override
					public void run() {
						synchronized (BaseActivity.this.currentState) {
							if (BaseActivity.this.currentState== State.STREAMING)
							{								
								synchronized (imgV) {
									//BaseActivity.this.releaseWakeLock();
									Toast.makeText(BaseActivity.this, "Picture stream stopped", Toast.LENGTH_SHORT).show();									
									imgV.setScaleType(ScaleType.CENTER_CROP);
									imgV.setImageDrawable(getResources().getDrawable(R.drawable.placephoneonsurface));
								}
							}
						}												
					}
				});		
	}


	@Override
	public void onPincodeRejected() {
		runOnUiThread(new Runnable() {

			@Override
			public void run() {
				if (D) Log.d(ME, "onPicodeRejected");
				startActivityForResult(pincodeIntent, REQUEST_PINCODE_ACTIVITY);
			}
		});
	}


	@Override
	public void onRequestConnectDirect() {
		runOnUiThread(new Runnable() {			
			@Override
			public void run() {
				if (D) Log.d(BaseActivity.ME, "onRequestConnectDirect");
				hideWaitingForConnectionDialog();
				showDialog(DialogHandler.DIALOG_FAILED_AUTOMATIC_CONNECTION);
			}
		});				
	}


	@Override
	public void onRequestPairingCode(final byte pincodeLength, final byte colorCodeLength) {
		runOnUiThread(new Runnable() {

			@Override
			public void run() {
				Log.d(ME, String.format("requestPairingCode. pincodelength: %d, colorcodeLength: %d", pincodeLength, colorCodeLength));
				pincodeIntent = new Intent(BaseActivity.this, PincodeActivity.class)
				.putExtra("length", pincodeLength);	
				BaseActivity.this.startActivityForResult(pincodeIntent, REQUEST_PINCODE_ACTIVITY);
				currentState = State.PAIRING;
			}
		});
	}


	/* Touch events */
	@Override
	public boolean onTouch(View v, MotionEvent e) {
		if (!mCommServiceBound)
			return false;
		synchronized (currentState) {
			if (currentState == State.PAIRING)
				return false;
			if (currentState == State.CALIBRATION && e.getAction() == MotionEvent.ACTION_DOWN)
			{
				showDialog(DialogHandler.DIALOG_ACCEPT_CALIBRATION);
				return true;
			}
			
			if (currentState == State.STREAMING)
			{
				float relWidth = e.getX()/v.getWidth();
				float relHeight = e.getY() / v.getHeight();

				if (D) Log.d(ME, String.format("onTouch: type: %d -  x: %f, y: %f. relX: %f, relY: %f", e.getAction(), e.getX(), e.getY(), relWidth, relHeight));
				switch (e.getAction()) {
				case MotionEvent.ACTION_DOWN:
					comm.sendTouchDownEvent(relWidth, relHeight);
					break;
				case MotionEvent.ACTION_MOVE:
					comm.sendTouchMoveEvent(relWidth, relHeight);
					break;
				case MotionEvent.ACTION_UP:
					comm.sendTouchUpEvent(relWidth, relHeight);
					break;
				default:
					// ignore!
					break;
				}
				return true; // handled
			}
		}
		return false; // not handled
	}
	

	/* Utils */
	
	public void setAuthenticationHandler(AuthenticationHandler authenticationHandler)
	{
		Log.d(ME, "setAuthenticationHandler");
		this.authenticationcontroler = authenticationHandler;
	}

	


}