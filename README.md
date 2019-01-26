## Please report all isues via: https://github.com/ScottyMac52/MFDisplay/issues

# MFD4CTS
Utility that allows the display of any image cropped from another image displayed anywhere on the display coordinates of a users computer as any size image.

## Installation
 - Download the release: https://github.com/ScottyMac52/MFDisplay/releases
 - Follow the instructions for the specified release.

 ## MFDSettings Configuration Section Guide
  ### Provides encapsulated configuration for All Modules, edit mfdsettings.config
  - filePath : Location where the CTS profile jpgs are located 
  - defaultConfig : Name of the default module to load, e.g. A-4E
  - saveClips : If true then every cropped image is saved in the following format:
    - File type: PNG
    - Folder: The same location as the origin filename
    - Filename: built as 
 X_{XOffsetStart}To{XOffsetFinish}Y_{YOffsetStart}To{YOffsetFinish}\_{Opacity}\_{ModuleName}\_{ConfigurationName}\_{Width}\_{Height}.png     
### DefaultConfigurations
Here is where as many named configurations can be placed as you want associated with every module. Good examples are LMFD and RMFD. Any attributes that you do not specify in the named configuration will need to be defined in the modules configuration that matches the same name. The recommended configuration is to use the LMFD and RMFD to define the size (Width, Height) of all LMFD and RMFD images. You may also define the defaults for cropping LMFD and RMFD, the defaults for the current images are:

```<add name="LMFD" left="2575" top="700" opacity="1" width="885" height="700" xOffsetStart="101" xOffsetFinish="776" yOffsetStart="250" yOffsetFinish="900"/>```

```<add name="RMFD" left="4250" top="700" opacity="1" width="885" height="700" xOffsetStart="903" xOffsetFinish="1576"  yOffsetStart="250" yOffsetFinish="900"/>```

#### The defaults for Left & Top in the shipped config matches my configuration which is that I have the following:
- Main Monitor = 2560X1440
- LMFD Monitor = 1024X768 positioned bottom right of Main Monitor
- RMFD Monitor = 1024X768 positioned bottom right of the LMFD Monitor
- This means that my total display area is 4608X1440 

#### Modules
 Each module entry, which starts with the markup <add moduleName... Can create as many named configurations underneath the markup <Configurations>. Each configuration can override any property that matches the name of a DefaultConfiguration. The moduleName must be unique in the configuration file. If it is not then you will get an error on startup. Addtionally the image path starts with the path specified in the top level MFDSettings section. So to reference an image in the A-10C directory you would specify fileName="A-10C\filename.jpg". This is true for both Module configs and Configurations configs with the Configuration configs serving as the last override to the filename.  
 
 Each configuration can also be a brand new configuration for a module. An example of this is the CDU configuration for the A-10C:
    
```<add name="CDU" left="500" top="600" width="694" height="352" xOffsetStart="1" xOffsetFinish="694" yOffsetStart="1" yOffsetFinish="352" filename="A-10C\DCS A10C CDU.jpg" opacity="1.0"/>```
   
#### Configuration Options
- fileName : The path of the image to crop, the origin is the filePath in the MFDSettings config.
 - For example to reference a fileName for the F/A-18C the fileName would be **FA-18C\filename.jpg** 
- xOffsetStart : X coord for the offset to use for the start of the horizontal cropping of an image
- xOffsetFinish : X coord for the offset to use for the finish of the horizontal cropping of an image
- yOffsetStart : Y coord for the offset to use for the start of the vertical cropping of an image
- yOffsetFinish : Y coord for the offset to use for the finish of the vertical cropping of an image
- width : Width for the resultant image to be displayed in.
- height : Height for the resultant image to be displayed in.
- top : Y coordinate for the top position of the image 
- left : X coordinate for the left position of the image when displayed
- top : Y coordinate for the left position of the image when displayed

## Please report all isues via: https://github.com/ScottyMac52/MFDisplay/issues

