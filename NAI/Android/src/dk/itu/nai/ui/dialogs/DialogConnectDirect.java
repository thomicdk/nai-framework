package dk.itu.nai.ui.dialogs;

import java.util.regex.Pattern;

import android.app.Activity;
import android.app.Dialog;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.EditText;
import dk.itu.nai.R;
import dk.itu.nai.ui.BaseActivity;
import dk.itu.nai.ui.preferences.ApplicationPreferences;

public abstract class DialogConnectDirect extends Dialog implements OnClickListener, TextWatcher {

	private EditText editIP;
	private EditText editPort;
	private Button btnExit;
	private Button btnConnect;
	private String ME ="DialogConnectDirect";

	private Activity activity;


	public DialogConnectDirect(BaseActivity context) {
		super(context);
		this.activity = context;
	}

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.dialog_connect_direct);		
		setTitle(getContext().getResources().getString(R.string.dialogConnectDirectTitle));
		setCancelable(false);

		SharedPreferences prefs = PreferenceManager.getDefaultSharedPreferences(activity);
		
		editIP = (EditText) findViewById(R.id.editIPAddress);
		editIP.setText(prefs.getString(ApplicationPreferences.KEY_SERVER_IP, ""));
		editIP.addTextChangedListener(this);		
		editPort = (EditText) findViewById(R.id.editPort);
		editPort.setText(prefs.getString(ApplicationPreferences.KEY_SERVER_PORT, ""));

		btnExit = (Button) findViewById(R.id.btnExit);
		btnExit.setOnClickListener(this);		
		btnConnect = (Button) findViewById(R.id.btnConnect);
		btnConnect.setOnClickListener(this);
		
		btnConnect.setEnabled(validateIpAddress(editIP.getText().toString().trim()));
	}	
	
	@Override
	public void afterTextChanged(Editable s) {
		// ignore
	}
	
	@Override
	public void beforeTextChanged(CharSequence s, int start,
			int count, int after) {
		// Ignore		
	}

	@Override
	public void onTextChanged(CharSequence s, int start,
			int before, int count) {
		Log.d(ME , "onTextChanged");
		btnConnect.setEnabled(validateIpAddress(editIP.getText().toString().trim()));		
	}
	
	
	@Override
	public void onClick(View v) {
		if (btnExit.equals(v))
			System.exit(0);
		else
		{
			
			int port = 0;
			if (editPort.getText().toString().trim().length()>0)
				port = Integer.parseInt(editPort.getText().toString());

			onConnect(editIP.getText().toString().trim(), port);		
			this.dismiss();
		}		
	}

	abstract public void onConnect(String IP, int port);
	
	/* Helpers */
	
	private boolean validateIpAddress(String iPaddress){
		final Pattern IP_PATTERN = 
            Pattern.compile("^(25[0-5]|2[0-4]\\d|[0-1]?\\d?\\d)(\\.(25[0-5]|2[0-4]\\d|[0-1]?\\d?\\d)){3}$");
		return IP_PATTERN.matcher(iPaddress).matches();        
    
}
	
	
}
