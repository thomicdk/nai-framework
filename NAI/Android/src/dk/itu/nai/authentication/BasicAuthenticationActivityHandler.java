package dk.itu.nai.authentication;

import android.app.Activity;
import android.content.Intent;
import android.util.Log;
import android.widget.Toast;
import dk.itu.nai.ui.BaseActivity;
import dk.itu.nai.ui.authentication.BasicAuthenticationActivity;


public class BasicAuthenticationActivityHandler extends AuthenticationHandler {
	private final String ME ="BasicAuthenticationActivityHandler"; 

	//private BaseActivity context;

	public BasicAuthenticationActivityHandler(BaseActivity context) {
		super(context);
		//this.context = context;
	}
		
	@Override
	public ClientCredentials onActivityResult(int resultCode, Intent data) {
		if (resultCode == Activity.RESULT_OK)			{
			String userId = data.getExtras().getString(BasicAuthenticationActivity.INTENT_EXTRA_USER_ID);
			if (userId != null &&  userId.length() > 0)
			{
				return new BasicClientCredentials(userId);
			}			
		}		
		// Fall back - close the application.
		context.finish();
		return null;
	}

	@Override
	public void requestCredentials() {
		Log.d(ME, "requestCredentials");
		startActivityForResult(BasicAuthenticationActivity.class);
	}
	
	@Override
	public void onAuthenticationAccepted() {
		// do nothing
	}

	@Override
	public void onAuthenticationRejected() {
		Toast.makeText(context, "Authentication rejected", Toast.LENGTH_SHORT).show();
		requestCredentials();
	}
	
	/* Helper methods */
		
//	private void showCredentialsActivity()
//	{
//		Log.d(ME, "showCredentialsActivity");
//		startActivityForResult(BasicAuthenticationActivity.class);
		//Intent intent = new Intent(context, BasicAuthenticationActivity.class);
		//this.context.startActivityForResult(intent,AuthenticationHandler.REQUEST_AUTHENTICATION_ACTIVITY);		
//	}
	
	
}
