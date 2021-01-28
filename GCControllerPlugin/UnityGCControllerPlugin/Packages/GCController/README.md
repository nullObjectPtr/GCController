# Important!

This plugin is provided as a Unity Package Manager (UPM) package. It must be placed in the /packages directory for it to run correctly. It will NOT install correctly if you copy it into your assets folder. Please read the full setup instructions below to ensure proper installation.

# Hello
C# wrapper for apple's GCController framework

# Support
## Forums
Support is handled mainly via the forums: http://www.hovelhouse.com/forums but you are welcome to send an e-mail directly to us at support@hovelhouse.com

## Documentation
TODO
 
# Setup
 
 IMPORTANT: This plugin is provided as a Unity Package Manager (UPM) package. It must be placed in the /packages directory for it to run correctly. It will NOT install correctly if you copy it into your assets folder.
 
## Installing the Unity Package

* Unzip the archive and place the entire directory in the packages directory (this must be done from the finder)

OR

* Move the archive to the /packages folder of your unity project and unzip it.
* Open "Window->Package Manager"
* Click the "+" button in the upper left and select "Add Package From Disk"
* Select the "package.json" file in the "CloudKit" folder of inside the unzipped directory
* Unity will now import the package into your project
 
 There are three libraries provided. One dynamic library for MacOS, one static library for iOS, and one static library for TVOS.
 
## Usage
To use, import the namespace "HovelHouse.GCController". No plugin initialization is needed and you do not need to add anything to your scenes. Just start using the classes as you would if this were an objective-c project. Class names and methods very closely match their Objective-C counterparts. See the provided examples for details.
 
## Building

Apple has dropped support for ARMv7 in the latest x-code. A "Universal Library" build from Unity is no longer supported.

### Required Build Settings - iOS
Set *Target Minimum IOS Version" to  14
 
### MacOS
* Target minimum macOS version is 11
 
# Known Issues
 
# FAQ
* No questions yet. Be the first! Send an e-mail to support@hovelhouse.com
 
# Road Map
 
### P1
* More of the API covered
