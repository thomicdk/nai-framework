package dk.itu.nai;

import dk.itu.nai.authentication.BasicAuthenticationActivityHandler;
import dk.itu.nai.ui.BaseActivity;


public class Main extends BaseActivity {

	public Main() {
		setAuthenticationHandler(new BasicAuthenticationActivityHandler(this));
	}
}
