package dk.itu.nai.ui.authentication;
import android.app.Activity;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;
import dk.itu.nai.R;

public class BasicAuthenticationActivity extends Activity {

	public static final int RESULT_FORCE_CLOSE = 9999;
	public static final String INTENT_EXTRA_USER_ID = "user_id";
	
	private static final String PREF_KEY_USER_ID = "pref_user_id";
		
	private EditText ed;

	private SharedPreferences sharedPref;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.basic_authentication_activity);
		ed = (EditText) findViewById(R.id.etUserId);
		
		sharedPref = PreferenceManager.getDefaultSharedPreferences(this);
		ed.setText(sharedPref.getString(PREF_KEY_USER_ID, ""));		
	}
	
	/* button events **/ 
	public void onSubmitAuthentication(View v)
	{
		String userId = ed.getText().toString().trim();
		if (userId.length()<1){
			Toast.makeText(this, "Please enter a user id.", Toast.LENGTH_SHORT).show();
			return;
		}
		// Update preferences
		Editor edit = sharedPref.edit();
		edit.putString(PREF_KEY_USER_ID, userId);
		edit.commit();
				
		// Return from this activity
		setResult(userId);
		finish();
		
	}
	
	public void onExitApplication(View v)
	{
		   setResult(RESULT_FORCE_CLOSE);
		   finish();
	}
	
	
	/* Helpers */
	private void setResult(String userId)
	{		
		Intent intent = getIntent();
		intent.putExtra(INTENT_EXTRA_USER_ID, userId);
		setResult(RESULT_OK, intent);					
	}
	
}
