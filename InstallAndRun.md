
# Introduction #
The code base includes a number of example applications. For MS Surface, there are two applications which utilize non-anonymous user interaction. For Android there are two ways of installing the matching default application.
How to install and run the three applications are described in the following.

# MS Surface #
There are two example applications available for the MS Surface, "Warm-up" and "Restaurant". They have been used during conduction of a user evaluation as part of the thesis writing. They both use the default user authentication mechanism in the NAI framework.

Both applications can be checked out from this part of the SVN repository: http://code.google.com/p/nai-framework/svn/trunk/Examples/Surface
You can also download a binary version here:
  * [Warm-up](http://nai-framework.googlecode.com/files/WarmUp.zip)
  * [Restaurant](http://nai-framework.googlecode.com/files/Restaurant.zip)

Both applications include a config file where you must specify the X509 certificate subject required for the secure communication between the tabletop and smartphone.

## Warm-up ##
This application demonstrates some of the features in the NAI Framework. The screenshot below shows the application running with three smartphone clients.

![http://nai-framework.googlecode.com/svn/wiki/images/warm-up.jpg](http://nai-framework.googlecode.com/svn/wiki/images/warm-up.jpg)

## Restaurant ##
The application is an order and payment system for the restaurant of the future. It is an example of deployment of the NAI framework in a familiar and realistic scenario. The screenshot below shows the first state of the application, where two restaurant guests order food and beverages with their smartphones.

![http://nai-framework.googlecode.com/svn/wiki/images/restaurant.jpg](http://nai-framework.googlecode.com/svn/wiki/images/restaurant.jpg)

# Android smartphone #
There are two ways of getting the NAI framework example on your Android smartphone. One is downloading a binary at the [download page](http://code.google.com/p/nai-framework/downloads/list) and install it directly, the other is checking out the Android code into a Android project in Eclipse. You need to check out the following folder to get the lates version:
http://code.google.com/p/nai-framework/svn/trunk/NAI/Android

Make sure the Android project is targeted min. SDK 7, or Android 2.1-update. Earlier versions will not operate correctly.

Enable the Wi-Fi connection, and connect the smartphone to the same router as the tabeltop if you will take advantage of the automatic lookup service built into the NAI framework.

If you get an error that the project is set up as a library, you need to right click the project folder and choose _Properties_. In the new window you click _Android_ and tick off the flag indicating that the project is a library. Then you connect the phone, and start up the application.