
# Create your own applications #
Before you start developing your own application using the NAI framework, we recommend that you read through the design guidelines below. As documented in the thesis (see [Overview](Overview.md)), chapter 7, Evaluation, it does matter _how_ you design the application.
## Design guidelines ##
The list below is taken directly from the thesis, and may be difficult to understand when taken out of context. Please read through the thesis, if you find it difficult to understand.

  1. **List of choices** The small displacement caused by the height difference between the tabletop display and the smartphone screen may make it difficult to align content of lists when the individual list item are displayed on the both screens. Design the application so that it is better alignment is supported, or keep the list within one screen.
  1. **Visual clues** For the user it is difficult to know when a [PersonalizedView](http://code.google.com/p/nai-framework/source/browse/trunk/NAI/Surface/NAI/UI/Controls/IPersonalizedView.cs) emerges. Some visual clue indicating where to put the phone to get the view is important. The same goes for the [IdentifiedViewport](http://code.google.com/p/nai-framework/source/browse/trunk/NAI/Surface/NAI/UI/Controls/IdentifiedViewport.cs) control.
  1. **Screen real estate** Balance the number of objects on the tabletop with the interaction area of the tabletop surface. There needs to be enough room for the phone(s) to operate, as well as for other interaction to happen.
  1. **Lessen the movements** Lessen the number of times the phone should be moved. Users care about their phone, and do not like to slide it along the surface, and get tired of lifting the phone from place to place. For example, a solution could be to allow users to order items from a restaurant menu via normal finger touch on the tabletop, but submit the order items by using the phone. Another scenario could be to place the phone at a certain spot, and then drag items to the phone via normal touch on the tabletop.
  1. **Unintended touch** When people move their phone across the tabletop screen, they may unintendedly touch the surface display underneath with their ngers. A thin smartphone increases the chance, because it is more dicult to grab. The problem may be minimised by UI design, if not all visual elements are made moveable and touchable by normal touch input.

## Setting up an Android project for Eclipse ##
If you do not need to modify the settings, implement your own set of credentials, or customised the authentication mechanism, you do not need to modify the code.

If you do need to change the code to your own need, you can change the code directly in the project, but is recommended that you follow the procedure stated below. If you need to have several versions installed at the same time, must set up your own project(s). The following guides you through that process.

### Setup the library ###
Make sure that you have followed the procedure as explained in the [Install and run the demo applications](InstallAndRun.md). You should then have an Android project with the NAI code.

You need to change the project, so that it can be used as a library. Right click the project folder and choose _Properties_. From the new window select "Android" and tick the _Is Library_. This prevents the project from being an application, but allows other projects to embed the code.
### Creating your own project ###
Next, you need to create you own Android project. We suggest that you name the default Activity "Main", but it can be any name. It is important that the _min SDK level_ is set to _7_, and _build target_ must be min 2.1-update

Now you can link the NAI code as a library to your new project. Right click the project folder, and choose _Properties_. Choose _Android_ in the new window. Below, press the "Add" button to link the NAI code library to the project.

Next you need to modify a couple of things. Android automatically adds stuff to you project. You need to modify that.

You need to change the default Activity. Remove the _onCreate_ method, and have the class extend the _BaseActivity_ from the library, like this:
```
...
import dk.itu.nai.ui.BaseActivity;

public class Main extends BaseActivity {
    
}
```

Next, delete the _main.xml_ file from the layout folder, and delete all  strings from _strings.xml_ in the values folder. If you want to override the default application name, keep the 'app\_name'.


### Sample application ###
You can download a sample appplication that changes the UI of the authentication mechanism.

You get the source here: http://code.google.com/p/nai-framework/svn/trunk/Examples/Android/CustomAuthenticationUI.

It is important that you already have the core NAI framework downloaded and set up as an Android project as described above. It is also important that it is set up as a library.

After checking the sample code out, make sure the project links to that NAI library.

Notice that the string resources in the values folder overrides the default 'app\_name' and the sample application makes use of a string resource not defined in the library.

The sample application can be used with any of the sample applications provided for the Microsoft Surface. The only thing that is changed is the UI of the authentication.

### More information ###
If you need to know more about using Android libraries and how you use the see the documentation: http://developer.android.com/guide/developing/projects/index.html

## Setting up a project for Microsoft Surface for Visual Studio ##
First you have to setup a development environment, either on the actual Surface Unit, or on a separate workstation. Microsoft provides a [guide](http://msdn.microsoft.com/en-us/library/ee804774(v=Surface.10).aspx) on how to do this. The Surface SDK 1.0 only runs on Windows Vista 32-bit [according to Microsoft](http://msdn.microsoft.com/en-us/library/ee804897(v=Surface.10).aspx). We have however successfully been developing some of the NAI framework on a 64-bit Windows 7 machine by following [this guide](http://blog.hompus.nl/2010/03/03/installing-the-microsoft-surface-sdk-on-windows-7-x64/).

With Visual Studio and the Surface SDK installed, it is easy to setup a new Surface project with a reference to the NAI framework. Just complete these steps:

  1. Start Visual Studio and choose 'New Project'
  1. Choose the 'Surface Application (WPF)' template
  1. When the project has been set up, click the 'Project' menu and choose 'Add Reference...'
  1. Click 'Browse' and then find and choose the `NAI.dll`
  1. Done!

Your application is now ready for non-anonymous user interaction. Read chapter 3 of our thesis ([find it here](Overview.md)), or check out the sample applications for inspiration.