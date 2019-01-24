## Please report all isues via: https://github.com/ScottyMac52/MFDisplay/issues

# MFDisplay

Utility that allows the display of any image cropped from another image displayed anywhere on the display coordinates of a users computer as any size image.

  ### NOTE, if the exe fails you may not have the correct .NET runtime installed. This application using .NET Framework 4.7.1 and is built in C#.NET on Visual Studio 2017. If you need to install the .NET Framework, it's free, here is the link: https://www.microsoft.com/en-us/download/details.aspx?id=56115.   

## Installation
 - Download the release: https://github.com/ScottyMac52/MFDisplay/releases
 - Follow the instructions for the specified release.
 ### NOTE, the application may crash as of release 0.0.3 due to the fact that the click once installer automatically executes the program during the install process and the intial profile path in the config file is E:\HOTAS\TARGET\CTS\Docs\Profile JPGs which is the path on my computer. If it does crash then you need to edit the MFDisplay.exe.config file and make sure that under MFDSettings the FilePath parameter is set a valid directory where the CTS Profile images are stored.

 ## MFDSettings Configuration Section Guide
  ### Provides encapsulated configuration for All Modules
  #### Edit MFDisplay.exe.config 
  - filePath : Location where the CTS profile jpgs are located 
  - defaultConfig : Name of the default module to load, e.g. A-4E
  - saveClips : If true then every cropped image is saved in the following format:
    - File type: PNG
    - Folder: The same location as the origin filename
    - Filename: built as 
 X_{XOffsetStart}To{XOffsetFinish}Y_{YOffsetStart}To{YOffsetFinish}\_{Opacity}\_{ModuleName}\_{ConfigurationName}\_{Width}\_{Height}.png     
  - DefaultConfigurations
    - Here is where as many named configurations can be placed as you want associated with every module. Good examples are LMFD and RMFD. Any attributes that you do not specify in the named configuration will need to be defined in the modules configuration that matches the same name. The recommended configuration is to use the LMFD and RMFD to define the size (Width, Height) of all LMFD and RMFD images. You may also define the defaults for cropping LMFD and RMFD, the defaults for the current images are:
    
<add name="LMFD" opacity="1" width="885" height="700" xOffsetStart="101" xOffsetFinish="776" yOffsetStart="250" yOffsetFinish="900"/>
<add name="RMFD" opacity="1" width="885" height="700" xOffsetStart="903" xOffsetFinish="1576" yOffsetStart="250" yOffsetFinish="900"/>
  
  - The defaults for Left & Top in the shipped config matches my configuration which is that I have the following:
    - Main Monitor = 2560*1440
    - LMFD Monitor = 1024*768 positioned bottom right of Main Monitor
    - RMFD Monitor = 1024*768 positioned bottom right of the LMFD Monitor
  - This means that my total display area is 4608*1440 so my configurations for Let and Top are...
    - <add name="LMFD" left="2575" top="700" />
    - <add name="RMFD" left="4250" top="700" />
      
  - Modules
    - Each module entry, which starts with the markup <add moduleName... Can create as many named configurations underneath the markup <Configurations>. Each configuration can override any property that matches the name of a DefaultConfiguration. Each configuration can also be a brand new configuration for a module. An example of this is the CDU configuration for the A-10C:
    - <add name="CDU" left="500" top="600" width="694" height="352" xOffsetStart="1" xOffsetFinish="694" yOffsetStart="1" yOffsetFinish="352" filename="DCS A10C CDU.jpg" opacity="1.0" />
    
  - Configuration Options
    - xOffsetStart : X coord for the offset to use for the start of the horizontal cropping of an image
    - xOffsetFinish : X coord for the offset to use for the finish of the horizontal cropping of an image
    - yOffsetStart : Y coord for the offset to use for the start of the vertical cropping of an image
    - yOffsetFinish : Y coord for the offset to use for the finish of the vertical cropping of an image
    - width : Width for the resultant image to be displayed in.
    - height : Height for the resultant image to be displayed in.
    - top : Y coordinate for the top position of the image 
    - left : X coordinate for the left position of the image when displayed
    - top : Y coordinate for the left position of the image when displayed
  
    Each module entry below, which starts with the markup <add moduleName... Can create as many named configurations underneath 
    the markup <Configurations>. LMFD and RMFD are "special" names and should always remain. Each configuration can override any 
    of the properties that are labeled as "Default" above. An example of this is the CDU configuration for the A-10C:
    
    <add name="CDU" rMfdLeft="500" top="600" width="694" height="352" xRMFDOffsetStart="1" xRMFDOffsetFinish="694" yOffsetStart="1" yOffsetFinish="352" filename="DCS A10C CDU.jpg" opacity="1.0" />
    
  4. Once the coordinates match your display preferences you can save the file, exit and execute the executable.
  ### NOTE, if the exe fails you may not have the correct .NET runtime installed. This application using .NET Framework 4.7.1 and is built in C#.NET on Visual Studio 2017. If you need to install the .NET Framework, it's free, here is the link: https://www.microsoft.com/en-us/download/details.aspx?id=56115.   
  
## Please report all isues via: https://github.com/ScottyMac52/MFDisplay/issues

