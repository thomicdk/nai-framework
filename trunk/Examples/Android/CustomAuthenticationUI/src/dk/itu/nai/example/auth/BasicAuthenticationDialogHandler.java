package dk.itu.nai.example.auth;

import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.util.Log;
import android.widget.Toast;
import dk.itu.nai.R;
import dk.itu.nai.authentication.AuthenticationHandler;
import dk.itu.nai.authentication.BasicClientCredentials;
import dk.itu.nai.authentication.ClientCredentials;
import dk.itu.nai.ui.BaseActivity;

public class BasicAuthenticationDialogHandler extends AuthenticationHandler {
	private final String ME ="BasicAuthenticationActivityHandler"; 

	private BaseActivity context;

	public BasicAuthenticationDialogHandler(BaseActivity context) {
		super(context);
	}	
	
	@Override
	public ClientCredentials onActivityResult(int resultCode, Intent data) {
		// Not used!
		return null;
	}

	@Override
	public void requestCredentials() {
		Log.d(ME, "requestCredentials");
		showCredentialsPicker();
	}	
	
	@Override
	public void onAuthenticationAccepted() {
		// do nothing
	}

	@Override
	public void onAuthenticationRejected() {
		Toast.makeText(context, "Authentication rejected", Toast.LENGTH_SHORT).show();
		showCredentialsPicker();	
	}
	
	/* Helper methods */
	
	private void showCredentialsPicker()
	{
		Log.d(ME, "showCredetialsPicker");
		final CharSequence[] items = context.getResources().getStringArray(R.array.user_ids);
			
		AlertDialog.Builder builder = new AlertDialog.Builder(this.context);
		builder.setTitle("Pick a user id");
		builder.setItems(items, new DialogInterface.OnClickListener() {
		    public void onClick(DialogInterface dialog, int item) {
		        context.sendCredentials(new BasicClientCredentials(items[item].toString()));			        
		    }
		});
		AlertDialog alert = builder.create();
		alert.show();
	}
		
}
