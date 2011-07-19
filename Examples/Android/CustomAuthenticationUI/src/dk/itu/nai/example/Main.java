package dk.itu.nai.example;

import dk.itu.nai.example.auth.BasicAuthenticationDialogHandler;
import dk.itu.nai.ui.BaseActivity;

public class Main extends BaseActivity {
	
	public Main() {
		setAuthenticationHandler(new BasicAuthenticationDialogHandler(this));
	}    
}