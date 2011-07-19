package dk.itu.nai.ui.dialogs;

import android.app.Activity;
import android.app.AlertDialog;
import android.app.Dialog;
import android.app.ProgressDialog;
import android.content.DialogInterface;
import dk.itu.nai.R;
import dk.itu.nai.ui.BaseActivity;

public final class DialogFactory {

		private static AlertDialog udpExceptionDialog;
		private static ProgressDialog _waitingForConnectionDialog;
	
		public static AlertDialog getUdpExceptionDialog(final BaseActivity activity)
		{
			
			if (udpExceptionDialog != null)
				return udpExceptionDialog;
			
			AlertDialog.Builder builder = new AlertDialog.Builder(activity);
			builder.setMessage(activity.getResources().getString(R.string.dialogFactoryUdpExceptionDialogError) +"\n\n" + activity.getResources().getString(R.string.dialogFactoryUdpExceptionDialogGuidance))
			       .setCancelable(false).setTitle(activity.getResources().getString(R.string.dialogFactoryUdpExceptionTitle))
			       .setNeutralButton("Set IP", new DialogInterface.OnClickListener() {										
					public void onClick(DialogInterface dialog, int which) {
						dialog.dismiss();
						activity.showDialog(DialogHandler.DIALOG_CONNECT_DIRECT);
						}})
			       .setNegativeButton("Exit", new DialogInterface.OnClickListener() {
			           public void onClick(DialogInterface dialog, int id) {
			        	   dialog.dismiss();
			        	   activity.finish();
			           }
			       })
			       .setPositiveButton("Try again", new DialogInterface.OnClickListener() {
			           public void onClick(DialogInterface dialog, int id) {			        	   
			                dialog.dismiss();
			                activity.connect();			                
			           }
			       });
			udpExceptionDialog = builder.create();				
			return udpExceptionDialog; 
		}
	
		
		public static AlertDialog getForcePreferenceUpdateDialog(final Activity activity)
		{			
			AlertDialog.Builder builder = new AlertDialog.Builder(activity);
			builder.setMessage("Please set a unique user id in the preferences.")
			       .setCancelable(false).setTitle("Preference missing!")
			       .setNeutralButton("OK", new DialogInterface.OnClickListener() {										
					public void onClick(DialogInterface dialog, int which) {
						dialog.dismiss();
						}});
				return builder.create();
		}
		
		public static ProgressDialog getWaitingForConnectionDialog(final BaseActivity activity)
		{			
			if (_waitingForConnectionDialog != null)
				return _waitingForConnectionDialog;
			
			_waitingForConnectionDialog = new ProgressDialog(activity)
			{
				@Override
				public void onBackPressed() {		
					hide();
					activity.finish();
					super.onBackPressed();
				}
			};
			_waitingForConnectionDialog.setMessage(activity.getResources().getString(R.string.dialogFactoryWaitingForConnection));						
			return _waitingForConnectionDialog;
		}

		public static DialogConnectDirect getConnectDirectDialog(final BaseActivity activity) {						
			DialogConnectDirect dialog = new DialogConnectDirect(activity) {
				
				@Override
				public void onConnect(String IP, int port) {
					activity.connectDirect(IP, port);					
				}

			
			};
			
			return dialog;
		}

		public static AlertDialog getExitDialog(final BaseActivity activity)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder(activity);
			builder.setMessage(activity.getResources().getString(R.string.dialogFactoryExitDialogQuestion))
			       .setCancelable(false).setTitle(activity.getResources().getString(R.string.dialogFactoryExitDialogTitle))
			       .setPositiveButton("Yes", new DialogInterface.OnClickListener() {										
					public void onClick(DialogInterface dialog, int which) {
							dialog.dismiss();
							activity.finish();
						}})
					.setNegativeButton("No", new DialogInterface.OnClickListener() {
						public void onClick(DialogInterface dialog, int arg1) {
							dialog.dismiss();
						}
					});
				return builder.create();
		}


		public static Dialog getAcceptCalibrationDialog(final BaseActivity activity) {
			AlertDialog.Builder builder = new AlertDialog.Builder(activity);
			builder.setMessage(activity.getResources().getString(R.string.dialogFactoryCalibrationDialogQuestion))
			       .setCancelable(false).setTitle(activity.getResources().getString(R.string.dialogFactoryCalibrationDialogTitle))
			       .setPositiveButton("Yes", new DialogInterface.OnClickListener() {										
					public void onClick(DialogInterface dialog, int which) {
							dialog.dismiss();
							activity.sendAcceptCalibration();
						}})
					.setNegativeButton("No", new DialogInterface.OnClickListener() {
						public void onClick(DialogInterface dialog, int arg1) {
							dialog.dismiss();
						}
					});
				return builder.create();
		}
		
		
}
