package dk.itu.nai.ui.menu;

import android.view.Menu;
import android.view.MenuItem;

public class MenuFactory {
public final static int GROUP_BASIC = 100;
	
	public final static int ID_PREFERENCES = 201;
	public final static int ID_EXIT = 202;

	public final static int GROUP_DYNAMIC = 101;

	public final static int ID_DISCONNECT = 200;
	public final static int ID_REQUEST_CALIBRATION = 203;
	
	/* Fixed set */
	
	public static MenuItem createExitMenuItem(Menu menu)
	{
		return menu.add(GROUP_BASIC, ID_EXIT, Menu.NONE, "Exit");
	}
	
	public static MenuItem createPreferencesMenuItem(Menu menu)
	{
		return menu.add(GROUP_BASIC, ID_PREFERENCES, Menu.NONE, "Preferences");
	}
	
	/* dynamic group */
	 
	public static MenuItem createDisconnectMenuItem(Menu menu)
	{
		return menu.add(GROUP_DYNAMIC, ID_DISCONNECT, Menu.NONE, "Disconnect");
	}
	
	public static MenuItem createRequestCalibrationMenuItem(Menu menu)
	{
		return menu.add(GROUP_DYNAMIC, ID_REQUEST_CALIBRATION, Menu.NONE, "Calibrate");
	}

	public static void createAllMenus(Menu menu) {
		createRequestCalibrationMenuItem(menu);
		//createDisconnectMenuItem(menu);
		createPreferencesMenuItem(menu);
		createExitMenuItem(menu);
	}
	
}
