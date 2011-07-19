package dk.itu.nai.ui.preferences;

import android.app.Dialog;
import android.content.Context;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.content.SharedPreferences.OnSharedPreferenceChangeListener;
import android.os.Bundle;
import android.preference.EditTextPreference;
import android.preference.Preference;
import android.preference.PreferenceActivity;
import android.preference.PreferenceManager;
import dk.itu.nai.R;
import dk.itu.nai.ui.dialogs.DialogFactory;
import dk.itu.nai.ui.dialogs.DialogHandler;

public class ApplicationPreferences extends PreferenceActivity implements OnSharedPreferenceChangeListener {

	public static final String KEY_SERVER_IP = "pref_server_ip";
	public static final String KEY_SERVER_PORT = "pref_server_port";
	public static final String KEY_SERVER_ADDR_UPDATE = "pref_server_addr_update";

	SharedPreferences sharedPref;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		addPreferencesFromResource(R.xml.preferences);
	}


	@Override
	protected void onResume() {
		super.onResume();
		if (getIntent().getExtras().getBoolean("dialog"))
			showDialog(DialogHandler.DIALOG_FORCE_PREFERENCES);
		
		// Shared prefernces:
		sharedPref = PreferenceManager.getDefaultSharedPreferences(this);


		// Override summaries

		/* Server IP */
		EditTextPreference mServerIpPreference = (EditTextPreference) getPreferenceScreen().findPreference(KEY_SERVER_IP);
		updateSummary(mServerIpPreference, KEY_SERVER_IP, false);

		/* Server port */		
		EditTextPreference mServerPortPreference = (EditTextPreference) getPreferenceScreen().findPreference(KEY_SERVER_PORT);
		updateSummary(mServerPortPreference, KEY_SERVER_PORT, false);


		// register for change events
		getPreferenceScreen().getSharedPreferences().registerOnSharedPreferenceChangeListener(this);		
	}

	@Override
	protected Dialog onCreateDialog(int id) {
		if (id == DialogHandler.DIALOG_FORCE_PREFERENCES)
			return DialogFactory.getForcePreferenceUpdateDialog(this);
		return super.onCreateDialog(id);
	}
	
	@Override
	protected void onPause() {
		super.onPause();
		// unregister changes
		getPreferenceScreen().getSharedPreferences().unregisterOnSharedPreferenceChangeListener(this);
	}

	/* OnSharedPreferenceChangeListener */

	@Override
	public void onSharedPreferenceChanged(SharedPreferences arg0, String key) {
		if (key.equals(KEY_SERVER_IP))
			updateSummary(getPreferenceScreen().findPreference(KEY_SERVER_IP), KEY_SERVER_IP, false);
		else if (key.equals(KEY_SERVER_PORT))
			updateSummary(getPreferenceScreen().findPreference(KEY_SERVER_PORT), KEY_SERVER_PORT, false);
	}

	private void updateSummary(Preference p, String key, boolean showWarning)
	{
		String pref = sharedPref.getString(key, "");
		if (!pref.equals(""))
			p.setSummary(pref);
		else
			if (showWarning) p.setSummary("MISSING!! " + p.getSummary());	
	}

	public static boolean hasValidPreferences(SharedPreferences sharedPref)
	{
		return true;
	}
	
	public static void updateServerPreferences(String ip, int port, Context context)
	{
		SharedPreferences prefs = PreferenceManager.getDefaultSharedPreferences(context);
		if (prefs.getBoolean(ApplicationPreferences.KEY_SERVER_ADDR_UPDATE, true))
		{			
			Editor edit = prefs.edit();
			edit.putString(ApplicationPreferences.KEY_SERVER_IP, ip);
			edit.putString(ApplicationPreferences.KEY_SERVER_PORT, String.format("%d", port));
			edit.commit();
		}
		
	}
	
	
}
