package dk.itu.nai.authentication;

import dk.itu.nai.ui.BaseActivity;
import android.content.Intent;


public abstract class  AuthenticationHandler {
	
	public static final int REQUEST_AUTHENTICATION_ACTIVITY = 2147474760; 
	 
	protected BaseActivity context;
	
	protected AuthenticationHandler(BaseActivity context)
	{
		this.context = context;
	}
	
	abstract public void requestCredentials();
	abstract public ClientCredentials onActivityResult(int resultCode, Intent data);
	abstract public void onAuthenticationRejected();
	abstract public void onAuthenticationAccepted();

	protected void startActivityForResult(Class<?> cls)
	{
		Intent intent = new Intent(context, cls);
		this.context.startActivityForResult(intent,AuthenticationHandler.REQUEST_AUTHENTICATION_ACTIVITY);	
	}
	
}
