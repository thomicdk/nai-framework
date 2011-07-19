package dk.itu.nai.ui.dialogs;

import android.app.Dialog;
import dk.itu.nai.ui.BaseActivity;

public class DialogHandler {
	
	public static final int DIALOG_WAITING_FOR_CONNECTION = 1;
	public static final int DIALOG_CONNECT_DIRECT = 2;
	public static final int DIALOG_FAILED_AUTOMATIC_CONNECTION = 3;
	public static final int DIALOG_FORCE_PREFERENCES = 4;
	public static final int DIALOG_EXIT = 5;
	public static final int DIALOG_ACCEPT_CALIBRATION = 6;
	
	public static Dialog onCreateDialog(int id, BaseActivity activity)
	{
		if (id == DIALOG_WAITING_FOR_CONNECTION)				
			return DialogFactory.getWaitingForConnectionDialog(activity);
		 else if (id == DIALOG_CONNECT_DIRECT)
			return DialogFactory.getConnectDirectDialog(activity);
		else if (id == DIALOG_FAILED_AUTOMATIC_CONNECTION)
			return DialogFactory.getUdpExceptionDialog(activity); 	
		else if (id == DIALOG_FORCE_PREFERENCES)
			return DialogFactory.getForcePreferenceUpdateDialog(activity);
		else if (id == DIALOG_EXIT)
			return DialogFactory.getExitDialog(activity);
		else if (id == DIALOG_ACCEPT_CALIBRATION)
			return DialogFactory.getAcceptCalibrationDialog(activity);
		return null;				
	}
	
}
