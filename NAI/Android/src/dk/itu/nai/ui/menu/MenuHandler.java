package dk.itu.nai.ui.menu;

import android.view.MenuItem;
import dk.itu.nai.state.State;
import dk.itu.nai.ui.BaseActivity;

public class MenuHandler {

	public static void onOptionsItemSelected(BaseActivity screen, MenuItem item) {
		switch (item.getItemId()) {
		case MenuFactory.ID_DISCONNECT:
			screen.disconnect();
			break;
		case MenuFactory.ID_EXIT:
			screen.finish();
			break;
		case MenuFactory.ID_PREFERENCES:
			screen.launchPreferences(false);
			break;
		case MenuFactory.ID_REQUEST_CALIBRATION:
			screen.requestCalibration();
			screen.setState(State.CALIBRATION);
			break;
		}		
	}

	
	
}
