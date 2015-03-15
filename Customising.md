# Introduction #
There are several settings available to change the behaviour of the NAI framework on both the Android and Surface platforms.

# MS Surface #
There are a number of settings available for the developer to adjust the behaviour of the NAI framework on the MS Surface. They are listed in the table below:

| **Name** | **.NET type** | **Default value** | **Description** |
|:---------|:--------------|:------------------|:----------------|
| AuthenticationHandler | `NAI.Client.Authentication.IAuthenticationHandler` | `NAI.Client.Authentication.BasicAuthenticationHandler` | The user authentication mechanism used to authenticate smartphone users. It must be compatible with the authentication mechanism running on the smartphone. |
| LoadCalibrations | `bool` | `true`| Indicates whether smartphone calibrations are loaded at application startup. Calibrations are loaded if either `LoadCalibrations` or `LoadAndSaveCalibrations` are set to `true` |
| LoadAndSaveCalibrations | `bool` | `true` | Indicates whether smartphone calibrations are loaded at application startup, and new calibrations are saved. Calibrations are loaded if either `LoadCalibrations` or `LoadAndSaveCalibrations` are set to `true` |
| ServerCertificateSubject | `string` | `string.Empty` | The smartphones and tabletop communicate via a secure SSL based connection. This requires the tabletop to have a X509 certificate installed in the local keystore. This string setting specifies the X509 subject of the certificate which the framework uses to search for the actual certificate in the local keystore. The default value of this setting implies that it must be changed. As an example of how to specify the string, here is how our certificate subject looks: `"CN=Test Certificate, O=ITU, E=mtho@itu.dk, L=Copenhagen, C=DK"`|
| SimulatorMode | `bool` | `false` | This setting is used to indicate whether the application runs in simulator mode. This is important because of the screenshot mechanism in the framework. |
| SimulatorOriginOffset | `System.Drawing.Point` | `(25,101)` | If the `SimulatorMode` is set to `true`, then this setting is used to specify the offset from the top left corner of the screen to the top left point of the simulator screen. The red line in the image below shows how to measure the offset. The default value, `(25,101)`, is equal to placing the simulator window in the top left corner of the screen. ![http://nai-framework.googlecode.com/svn/wiki/images/simulator-offset.jpg](http://nai-framework.googlecode.com/svn/wiki/images/simulator-offset.jpg) |
| StreamingFrameRate | `int` | 15 | The rate at which screenshots are captured and sent to smartphone clients |
| TabletopScreen | `System.Windows.Forms.Screen` | `System.Windows.Forms.Screen.PrimaryScreen` | If the tabletop computer has several monitors attached, this setting is used to specify which monitor is the tabletop. |
| UdpToken | `string` | `NAIServer` | The call sign used in the automatic socket connection establishment. This value must be the same on both the smartphone and tabletop |

The settings are available in the static class `NAI.Properties.Settings`, and it is recommended only to change the settings on application startup. For example:
```
public partial class MainWindow : SurfaceWindow
{
	public MainWindow()
	{
		SetupNaiFramework();
		...
	}

	private void SetupNaiFramework()
	{
		NAI.Properties.Settings.ServerCertificateSubject = "CN=Test Certificate, O=ITU, E=mtho@itu.dk, L=Copenhagen, C=DK";
		NAI.Properties.Settings.SimulatorMode = true;
	}
}
```

## Customising user authentication ##
To implement a customised user authentication mechanism you have to do three things.
  1. Implement a specialization of `NAI.Client.Authentication.ClientCredentials`.
  1. Implement the interface `NAI.Client.Authentication.IAuthenticationHandler`.
  1. Plug-in the authentication mechanism to the framework (see section above).

Below is an example of how to implement the two first steps in an authentication mechanism requiring username and password:
```
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using NAI.Client.Authentication;

namespace NAITest.CustomAuthentication
{
    class UsernamePasswordAuthentication : IAuthenticationHandler
    {
        private Dictionary<string, string> _users;

        public UsernamePasswordAuthentication()
        {
            _users = new Dictionary<string, string>();
            _users.Add("michael", "123456");
            _users.Add("thomas", "qwerty");
        }

        public ClientCredentials ParseCredentialsMessage(byte[] messageBody)
        {
            BinaryReader br = new BinaryReader(new MemoryStream(messageBody), Encoding.UTF8);
            int usernameLength = IPAddress.NetworkToHostOrder(br.ReadInt32());
            string username = Encoding.UTF8.GetString(br.ReadBytes(usernameLength));

            int passwordLength = IPAddress.NetworkToHostOrder(br.ReadInt32());
            string password = Encoding.UTF8.GetString(br.ReadBytes(passwordLength));

            return new UsernamePasswordCredentials(username, password);
        }

        public bool Authenticate(ClientCredentials credentials)
        {
            var cred = credentials as UsernamePasswordCredentials;
            if (_users.ContainsKey(cred.UserId) &&
                _users[cred.UserId].Equals(cred.Password))
            {
                return true;
            }
            return false;
        }
    }

    class UsernamePasswordCredentials : ClientCredentials
    {
        public string Password { get; set; }

        public UsernamePasswordCredentials(string username, string password) : base(username)
        {
            this.Password = password;
        }
    }
}
```

# Android #


## Changing settings ##
Of the settings you can change the most important are 'app\_name' and 'server\_udp\_id\_token'. The latter controls the token that is used to find the tabletop.

The text used in the application can be altered her. Check the value folder for of the NAI project. You can override all the string values by defining the in you own project. This way you can localise the language or target the purpose for different applications.

Most important string values:
  * app\_name' Override the default application name.
  * 'server\_udp\_id\_token' Overrides the default token used to the automatic connection setup. This value must mach the corresponding setting on the


## Customising authentication ##
How you customise the authentication mechanism, and setting custom credentials are explained in section 3.3, Setup, of the thesis mentioned at wiki page [Overview](Overview.md).

You can download a sample application that customises the UI part of the authentication as described in wiki page [Install and run the demo applications](InstallAndRun.md). The 'BasicAuthenticationHandler' in the NAI framework for the smartphone shows you a complete example of how to set up the authentication mechanism using an Activity for having the user enter his credentials.

Below follows a small example that shows how to implement that part of the  authentication handler that creates the credentials object, as well as the specialisation of 'ClientCredentials' that is needed.

Excerpt from an authentication handler that get the username as well as password from an Activity:
```
public class UsernamePasswordAuthenticationHandler extends AuthenticationHandler {

  public UsernamePasswordAuthenticationHandler(BaseActivity context) {
    super(context);
  }

  @Override
  public ClientCredentials onActivityResult(int resultCode, Intent data){
    if (resultCode == Activity.RESULT_OK){
      String username =  data.getExtras().getString(UsernamePasswordAuthenticationActivity.INTENT_EXTRA_USERNAME);
      String password = data.getExtras().getString(UsernamePasswordAuthenticationActivity.INTENT_EXTRA_PASSWORD);
      if (username != null &&  username.length() > 0 &&
          password != null && password.length() > 0)
	  return new UsernamePasswordClientCredentials(username,password);
      // Fall back - close the application.
      context.finish();
      return null;
  }
...
}
```

The 'UsernamePasswordClientCredentials' could then be implemented like this:

```
...
public class UsernamePasswordClientCredentials extends ClientCredentials
{
  private String password;
	
  public UsernamePasswordClientCredentials(String username,String password ) {
    super(username);
    this.password = password;
  }
	
  @Override
  public byte[] toByteArray() {		
    try {
      byte[] usernameBytes = getUserId().trim().getBytes("UTF-8");
      byte[] passwordBytes = this.password.trim().getBytes("UTF-8");
			
      ByteArrayOutputStream baos = new ByteArrayOutputStream();
      DataOutputStream dos = new DataOutputStream(baos);

      dos.writeInt(usernameBytes.length);
      dos.write(usernameBytes);
		
      dos.writeInt(passwordBytes.length);
      dos.write(passwordBytes);
      return baos.toByteArray();
    } catch (UnsupportedEncodingException e) {
      return null;
    } catch (IOException ignore) {}		
      return null;		
  }
}

```



### Accepting server certificates ###
By default the NAI framework for the Android smartphone accept all server certificates. If you for some reason would like to take control over the acceptance you need to implement you own trust manager.

You do that by referencing your own 'TrustManager' in the constructor of your default Activity:
```
...
public class Main extends BaseActivity {    
	 public Main() {
		AuthenticationConfiguration.getInstance()
			.setTrustManager(new TrustManager[]{
				    new X509TrustManager() {
				        public java.security.cert.X509Certificate[] getAcceptedIssuers() {
				            ...
				        }
				        public void checkClientTrusted(
				            ... {
				        }
				        public void checkServerTrusted(
				            ... {
				        }
				    }} 				
		);		
	 }   
}
```

It is out of scope to provide a guide for implementing your own 'TrustManager'. We recommend that you read [Java Secure Socket Extension (JSSE) - Reference Guide](http://download.oracle.com/javase/1.5.0/docs/guide/security/jsse/JSSERefGuide.html#TrustManager).