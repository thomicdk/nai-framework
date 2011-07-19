package dk.itu.nai.ui;

import android.app.Activity;
import android.app.AlertDialog;
import android.app.Dialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import dk.itu.nai.R;

/**
 * Requires length (int) as extra parameter to the intent.
 * @author berglund
 *
 */
public class PincodeActivity extends Activity {
	
	private static final String ME = "PincodeActivity";
	
	private static final int DIALOG_EXIT = 1234;
	public static final int RESULT_FORCE_CLOSE = 9999;
	
	public static final String BUNDLE_KEY_PINCODE_LENGTH = "length";
	public static final String BUNDLE_KEY_PINCODE_AS_BYTES = "pincodeAsBytes";
	public static final String BUNDLE_KEY_PINCODE = "pincode";
	
	
	private String currentPin = "";
	private int maxNumbers  = 0;
	private int currentPosition = 0;

	private EditText et;
	private Button btnOK;

	
	/* Basic Activity event handlers */
	
	/** Called when the activity is first created. */
	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		Log.d(ME, "onCreate");
		setContentView(R.layout.pincode_activity);
		
		// get views
		et = (EditText) findViewById(R.id.etPinCode);
		et.setFocusable(false);
		et.setClickable(false);
		
		btnOK = (Button) findViewById(R.id.btnOk);
		btnOK.setEnabled(false); 

		//  pincode setup
		Bundle bundle = getIntent().getExtras();
		byte length = bundle.getByte(BUNDLE_KEY_PINCODE_LENGTH);
		if (length == 0)
			finish();
		maxNumbers = length;
		// Initialize et
		
		et.setText(getResources().getString(R.string.txtEnterPin));
		
		Log.d(ME, " pincode Length is " + length);

	}
	@Override
	protected void onResume() {	
		super.onResume();
		Log.d(ME, "onResume");
	}
	
	/*  Options */
	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		menu.add(getResources().getString(R.string.pincodeActivityOptionsMenuItemExit));
		return super.onCreateOptionsMenu(menu);
	}
	
	@Override
	public boolean onMenuItemSelected(int featureId, MenuItem item) {
		showDialog(DIALOG_EXIT);
		return super.onMenuItemSelected(featureId, item);
	}
	
	/*  Dialog     */ 
	
	@Override
	protected Dialog onCreateDialog(int id) {
		if (id != DIALOG_EXIT)
			return super.onCreateDialog(id);
		
		AlertDialog.Builder builder = new AlertDialog.Builder(this);
		builder.setMessage(getResources().getString(R.string.pincodeActivityExitDialogMessage))
		       .setCancelable(false).setTitle(getResources().getString(R.string.pincodeActivityExitDialogTitle))		       
		       .setNegativeButton(getResources().getString(R.string.pincodeActivityDialogNegativeButtonText), new DialogInterface.OnClickListener() {
		           public void onClick(DialogInterface dialog, int id) {
		                dialog.dismiss();
		           }		           
		       })
		       .setPositiveButton(getResources().getString(R.string.pincodeActivityDialogPositiveButtonText), new DialogInterface.OnClickListener() {
		           public void onClick(DialogInterface dialog, int id) {
		        	   setResult(RESULT_FORCE_CLOSE);
		        	   PincodeActivity.this.finish();		                			               
		           }
		       });
		return builder.create();			
	}
	
	
	
	/* Button click event handlers */
	
	@Override
	public void onBackPressed() {
		showDialog(DIALOG_EXIT);		
	}
	
	
	public void onNumberClick(View v)
	{
		if (currentPosition >= maxNumbers)
			return;
		currentPin += (String) v.getTag();
		currentPosition++;
		et.setText(pinView(currentPosition));
		if (currentPosition==maxNumbers)
			btnOK.setEnabled(true);		
		Log.d(ME, String.format("currentPin: %s, currentPosition: %d, Tag:%s", currentPin, currentPosition, (String) v.getTag()));
	}

	public void onOKClick(View v)	
	{		
		Log.d(ME, "onOKClick");
		setResult(currentPin);
		finish();		
	}
	
	public void onDelClick(View v)
	{
		if (currentPosition < 1)
			return;
		currentPosition--;
		currentPin = currentPin.substring(0, currentPin.length()-1);
		et.setText(pinView(currentPosition));		
		if (currentPosition!=maxNumbers)
			btnOK.setEnabled(false);
		Log.d(ME, String.format("currentPin: %s, currentPosition: %d", currentPin, currentPosition));
	}

	/* Helper methods */
	
	private void setResult(String pinCode)
	{
		Log.d(ME, "SendResult");
		Intent intent = getIntent();
		intent.putExtra(BUNDLE_KEY_PINCODE, pinCode);
		intent.putExtra(BUNDLE_KEY_PINCODE_AS_BYTES, pincodeAsBytes(pinCode));
		setResult(RESULT_OK, intent);					
	}

	private byte[] pincodeAsBytes(String pinCode) {
		byte[] bytes = new byte[pinCode.length()];				
		for(int i = 0; i< pinCode.length(); i++)
		{
			bytes[i] = (byte) ((byte) pinCode.charAt(i) - 48);
			Log.d(ME, "pincodeAsBytes:" + bytes[i]);
		}		
		return bytes;
	}

	private String pinView(int pos)
	{
		String r="";
			for(int i=0; i< maxNumbers; i++)
			{
				if (i < pos)
					r += "#";
				else
					r += "_";
				if (i != maxNumbers - 1)
					r += " ";
			}		
		return r;
	}
}