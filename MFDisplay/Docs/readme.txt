## Please report all isues via: https://github.com/ScottyMac52/MFDisplay/issues

# MFDisplay

Utility that allows the display of any image cropped from another image displayed anywhere on the display coordinates of a users computer as any size image.

  ### NOTE, if the exe fails you may not have the correct .NET runtime installed. This application using .NET Framework 4.7.1 and is built in C#.NET on Visual Studio 2017. If you need to install the .NET Framework, it's free, here is the link: https://www.microsoft.com/en-us/download/details.aspx?id=56115.   

## Installation
 - Download the release: https://github.com/ScottyMac52/MFDisplay/releases
 - Follow the instructions for the specified release.

 ## MFDSettings Configuration Section Guide
  ### Provides encapsulated configuration for All Modules
  #### Edit MFDisplay.exe.config 
  - FilePath : Location where the CTS profile jpgs are located 
  - DefaultConfig : Name of the default module to load, e.g. A-4E
  - Left MFD Configuration Options (Defaults)
    - xLMFDOffsetStart : Default X coord for the Offset to use for the start of the horizontal cropping of an image
    - xLMFDOffsetFinish : Default X coord for the Offset to use for the finish of the horizontal cropping of an image
  - Right MFD Configuration Options (Defaults)
    - xRMFDOffsetStart : Default X coord for the Offset to use for the start of the horizontal cropping of an image
    - xRMFDOffsetFinish : Default X coord for the Offset to use for the finish of the horizontal cropping of an image
  - Common Configuration Options
    - yOffsetStart : Default Y coord for the offset to use for the start of the vertical cropping of an image
    - yOffsetFinish : Default Y coord for the offset to use for the finish of the vertical cropping of an image
    - width : Default width for the resultant image to be displayed in.
    - height : Default height for the resultant image to be displayed in.
    - top : Default Y coordinate for the top position of the image 
    - lMfdLeft : Default X coordinate for the left position of the image when displayed on the Left MFD
    - rMfdLeft : Default X coordinate for the left position of the image when displayed on the Right MFD
  
    Each module entry below, which starts with the markup <add moduleName... Can create as many named configurations underneath 
    the markup <Configurations>. LMFD and RMFD are "special" names and should always remain. Each configuration can override any 
    of the properties that are labeled as "Default" above. An example of this is the CDU configuration for the A-10C:
    
    <add name="CDU" rMfdLeft="500" top="600" width="694" height="352" xRMFDOffsetStart="1" xRMFDOffsetFinish="694" yOffsetStart="1" yOffsetFinish="352" filename="DCS A10C CDU.jpg" opacity="1.0" />

	Home Fries has already adcded several others:

	A-10C
		<add name="UFC" rMfdLeft="1600" top="1000" width="900" height="210" xRMFDOffsetStart="1" xRMFDOffsetFinish="1730" yOffsetStart="590" yOffsetFinish="990" filename="DCS A-10C UFC.jpg" opacity="1.0" />
	AV-8B
		<add name="UFC" rMfdLeft="1500" top="1" width="885" height="500" xRMFDOffsetStart="30" xRMFDOffsetFinish="975" yOffsetStart="40" yOffsetFinish="580" filename="DCS AV8B UFC.jpg" opacity="1.0" />
	M-2000C
		<add name="INS" rMfdLeft="1500" top="1" width="548" height="500" xRMFDOffsetStart="8" xRMFDOffsetFinish="698" yOffsetStart="114" yOffsetFinish="744" filename="DCS M2000 INS.jpg" opacity="1.0" />
    
    It even overrides the filename. You can use any image and crop any dimension from that image into an image of any size onto any 
    coordinate that your display system is configured to support.
    
    The values in the current configuration are setup as follows:
    
    - Main Display 2K (2560*1440) 
    - LMFD - 1280*720, positioned as 2nd monitor directly to Right of main
    - RMFD - 1280*720, positioned as 3rd monitor directly to Right of 2nd Monitor
    
    - lMfdLeft = starting left position of the display area for the image on the Left MFD, in my case 2975 
    - rMfdLeft = starting left position of the display area for the image on the Right MFD, in my case 3825
    
  4. Once the coordinates match your display preferences you can save the file, exit and execute the executable.
  
## Please report all isues via: https://github.com/ScottyMac52/MFDisplay/issues

