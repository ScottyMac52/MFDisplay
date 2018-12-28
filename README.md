# MFDisplay

Utility that allows the display of any image cropped from another image displayed anywhere on the display coordinates of a users computer as any size image.

## Installation
  1. Unzip the release file into a folder on your windows based computer
  2. Then edit the MFDisplay.exe.config file and under the MFDSettings section change the FilePath to be the location where the CTS profile graphics files are. In my case E:\HOTAS\TARGET\CTS\Docs\Profile JPGs.
  3. Next edit the coordinates in the configuration using the Guide below  
 
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
    
    It even overrides the filename. You can use any image and crop any dimension from that image into an image of any size onto any 
    coordinate that your display system is configured to support.
    
    The values in the current configuration are setup as follows:
    
    - Main Display 2K (2560*1440) 
    - LMFD - 1280*720, positioned as 2nd monitor directly to Right of main
    - RMFD - 1280*720, positioned as 3rd monitor directly to Right of 2nd Monitor
    
    - lMfdLeft = starting left position of the display area for the image on the Left MFD, in my case 2975 
    - rMfdLeft = starting left position of the display area for the image on the Right MFD, in my case 3825
    
  4. Once the coordinates match your display preferences you can save the file, exit and execute the executable.
  ### NOTE, if the exe fails you may not have the correct .NET runtime installed. This application using .NET Framework 4.7.1 and is built in C#.NET on Visual Studio 2017. If you need to install the .NET Framework, it's free, here is the link: https://www.microsoft.com/en-us/download/details.aspx?id=56115.   
