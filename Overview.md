
# What is the NAI framework? #
The NAI framework has been developed as part of the master thesis by **Michael Thomassen** and **Thomas Berglund**, IT University of Copenhagen, Denmark, with [Professor Jakob Bardram](http://www.itu.dk/people/bardram/pmwiki/) as supervisor.
The thesis is called: **Non-anonymous user interaction on tabletop displays**.

In general, contemporary tabletops do not have the ability to discriminate the touch of one user from the touch of another. This framework solves that problem, and can therefore add support for non-anonymous user interaction to tabletop displays.
It allows developers to build applications for the Microsoft Surface that have access to the identity of the user carrying out an action.

For a developer, this basically means that the framework provides a set of _UI Controls_ and a set of _Identfied Events_ that you can use for personalised applications that need _access control_ or _dynamically customsation of the user interface_ based on the identity of the user.

The UI controls are pre-built to show how to build you own controls and to help leverage development of applications.

The events are really _normal_ touch and hover events, but in addition to the conventional information, the event arguments also contain the identity of the user causing the event. Because the presence of the user is relevant the framework also support _person events_.

For a better understanding and a more detailed explanation we encourage you to watch a small presentation video, and read the thesis (especially chapter 3).

# Watch the video #
We have created a small video which demonstrates the features the NAI framework.

The video illustrates both the user experience as well as features of the framework.

<a href='http://www.youtube.com/watch?feature=player_embedded&v=H9n6vRNgNR8' target='_blank'><img src='http://img.youtube.com/vi/H9n6vRNgNR8/0.jpg' width='425' height=344 /></a>

# Read the thesis #
We strongly suggest that you download and read through the thesis mentioned above. Especially chapter 3 will serve as a good outset to get an idea of the user experience and how to program your own applications using the framework.

The thesis can be downloaded from the homepage for [The Pervasive Interaction Technology Lab, IT University of Copenhagen, Denmark](http://pit.itu.dk).
Direct link: http://www.itu.dk/pit/?n=Projects.NonAnnTabletop

# Requirements #
The NAI framework has been built for two specific hardware components
  * A smartphone running Android 2.1 or later
  * The Microsoft Surface 1.0

The NAI framework has not been tested on other Android versions than 2.1 and 2.2. However, it should operate on later version of the software stack as well. During development we used HTC Desire with Android 2.2, HTC Legend with Android 2.1.
There is no guarantee that it will run on other types of hardware or Android versions.

The [Surface Simulator](http://msdn.microsoft.com/en-us/library/ee804952(v=Surface.10).aspx) included in the [Surface SDK](http://msdn.microsoft.com/en-us/library/ee804767(v=Surface.10).aspx) can also be used to work with the NAI framework. It is however required to specify in the NAI framework if the simulator is used. Read more in the article on [Customising the framework to your needs](Customising.md).

The NAI framework has been developed for the The Microsoft Surface 1.0. Soon there will be a Microsoft Surface 2.0 available with different specifications and a new development environment. The NAI framework does not yet support this upcoming version.
Microsoft has already released the specifications of the new version of the [Surface SDK 2.0](http://msdn.microsoft.com/en-us/library/ff727815.aspx).
On the [Microsoft Surface homepage](http://www.surface.com) they also promise to provide a migration tool that can transform MSS 1.0 projects to be used at the upcoming MSS 2.0.

## Preparing the smartphone ##
The smartphone must carry a MSS tag on the back. The tag must preferably be positioned upright in the middle of the phone as shown below.
![http://nai-framework.googlecode.com/svn/wiki/images/DesireBack-upright.png](http://nai-framework.googlecode.com/svn/wiki/images/DesireBack-upright.png)

MS `Byte` tags can be [downloaded here](http://www.microsoft.com/download/en/details.aspx?displaylang=en&id=11029). MS `Identity` tags can be generated with [this tool](http://msdn.microsoft.com/en-us/library/ee804803(v=surface.10).aspx).

## Preparing the MS tabletop ##
Install a X509 certificate in the local key store to prepare the tabletop for secure SSL-based communication with the smartphone. The tool [X509 Certificate Generator 1.0](http://www.softpedia.com/get/Security/Security-Related/X509-Certificate-Generator.shtml) is useful for creating in-secure certificates for development and testing purposes. The article on [Customising the framework to your needs](Customising.md) describes how to reference your own cerficate in the framework.

# The next step #
The next step would be to download and set up a project for the Android smartphone and the Microsoft Surface so you can take the test applications for at spin.

More info at the wiki page [Install and run the test applications](InstallAndRun.md).

# Disclaimer #
The code is provided as is. Unfortunately, we cannot guarantee that we can support or maintain the code. Not now, not in the future. However, if you would like to participate, and have some good ideas to how the framework can be improved please do not hesitate to contact us. You might want to read our suggestions on [future work](Improving.md) first.