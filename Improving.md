# Improving the NAI framework #

## Ideas for improvements ##
Besides removing bugs, there are a number of features we would like to suggest for future work. A more elaborate list can be found in the thesis mentioned in the [Overview](Overview.md).

  1. **Multi-touch** The framework must support identified multi-touch. Both the tabletop and the smartphone supports it.
  1. **Block contact events** The framework is dependent on the smartphones having a non-reflective back. Even a small metallic ring around the camera (like on the HTC Desire) is enough to cause unwanted Contact events. This limitation can be eliminated by suppressing the Contact events raised in the area underneath the phone.
  1. **extend authentication mechanism** The current implementation of the customisable authentication mechanism is only executed during the startup of the smartphone application. Applications may very well require stronger authentication functionality where some individual events, like a click on a button, requires the user to be authenticated again.
  1. **Automatic pairing** The current pairing mechanism using PIN codes is somewhat cumbersome and requires the user's attention. The pairing could however be performed automatically, for example by using a unique colour sequence displayed by the tabletop underneath the phone. The idea is that the colour sequence is detected by the smartphone camera, and  converted to that is sent to the tabletop for verication.

In addition to the list provided in the thesis some wild ideas could be interesting to pursue.

  * **Haptic feedback** Add support for feedback by letting the tabletop be able to control actuators on the smartphone. For example, use the vibrator when somebody presses a button or using the speaker to play a sound if a user isn't authenticated.
  * **Custom messages** Since there  exists a network connection between the smartphone and the tabletop, it is possible to implement it so that there can be send custom messages (really just array of bytes) between the two devices. A customisable handler can then decide what should happen to the message. This follows the OSI scheme of separating concerns by separating socket layer from message layer as documented in the thesis, chapter 5.

## Get started ##
Here is what is necessary to do in order to get started on improving the two platforms in the NAI Framework.
## The MS Surface code ##
When the Surface code is checked out from trunk, you get a solution containing two projects:
| **Title** | **Description** |
|:----------|:----------------|
| NAI | The actual framework as a class library. The project compiles into a DLL and is hence in itself not executeable |
| NAIDevelopment | A sparse test project which directly reference the NAI class library and is executeable. Useful for testing changes to the framework code |

The class  `DebugSettings` in the root of the NAI project can be used to turn console debugging on and off for different areas of the code.

To get an idea of how the code is structured, chapter 3 and 5 of our thesis is relevant to read.
## The Android smartphone code ##
The procedure is straight forward. You check out into an Eclipse project just as described at [Install and run the demo applications](InstallAndRun.md) and improve the code from there.

# Tell us #
Do not forget to tell us if you think that the improvements could benefit others. Then we will merge your contribution into this project.