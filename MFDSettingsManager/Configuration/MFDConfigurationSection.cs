using System;
using System.Configuration;
using System.IO;
using log4net;
using MFDSettingsManager.Enum;

namespace MFDSettingsManager.Configuration
{
    /// <summary>
    /// Encapsulates a Configuration section for the MFDs
    /// </summary>
    public class MFDConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsDataDirty { get; set; }

        /// <summary>
        /// Logger 
        /// </summary>
        public ILog Logger { get; set; }

        /// <summary>
        /// Gets the configurations section
        /// </summary>
        /// <returns></returns>
        public static MFDConfigurationSection GetConfig(ILog logger, string fullPathtoConfig = null)
        {
            System.Configuration.Configuration currentConfiguration = null;

            if(string.IsNullOrEmpty(fullPathtoConfig))
            {
                currentConfiguration = ConfigurationManager.OpenExeConfiguration(Path.Combine(Environment.CurrentDirectory, "MFDisplay.exe"));
            }
            else
            {
                var exeFileMap = new ExeConfigurationFileMap()
                {
                    ExeConfigFilename = fullPathtoConfig
                };
                currentConfiguration = ConfigurationManager.OpenMappedExeConfiguration(exeFileMap, ConfigurationUserLevel.None);
            }

            var configSection = (MFDConfigurationSection)currentConfiguration.GetSection("MFDSettings");
            configSection.Logger = logger;
            configSection.IsDataDirty = false;
            return configSection ?? new MFDConfigurationSection();
        }

        #region Main configuration properties

        /// <summary>
        /// SaveClips results in the cropped images being saved
        /// </summary>
        [ConfigurationProperty("saveClips", IsRequired = false)]
        public bool? SaveClips
        {
            get
            {
                return (bool?)this["saveClips"];
            }
            set
            {
                IsDataDirty = true;
                this["saveClips"] = value;
            }
        }

        /// <summary>
        /// ImageType to use when SaveClips == true
        /// </summary>
        [ConfigurationProperty("imageType", IsRequired = false)]
        public SavedImageType? ImageType
        {
            get
            {
                try
                {
                    var result = this["imageType"] == null ? SavedImageType.Bmp : (SavedImageType) this["imageType"];
                    return result;
                }
                catch (Exception ex)
                {
                    Logger?.Error("There was an error in determining the ImageType", ex);
                    return SavedImageType.Bmp;
                }
            }

            set
            {
                IsDataDirty = true;
                this["imageType"] = value ?? SavedImageType.Bmp;
            }
        }


        /// <summary>
        /// The file path to the images to be cropped
        /// </summary>
        [ConfigurationProperty("filePath", IsRequired = true)]
        public string FilePath
        {
            get
            {
                return this["filePath"] as string;
            }
            set
            {
                IsDataDirty = true;
                this["filePath"] = value;
            }
        }

        /// <summary>
        /// The default configuration to load
        /// </summary>
        [ConfigurationProperty("defaultConfig", IsRequired = false)]
        public string DefaultConfig
        {
            get
            {
                return this["defaultConfig"] as string;
            }
            set
            {
                IsDataDirty = true;
                this["defaultConfig"] = value;
            }
        }

        #endregion Main configuration properties

        #region MFD Common Left, Top, Wdith and Height

        /// <summary>
        /// Default Width to use for the images
        /// </summary>
        [ConfigurationProperty("width", IsRequired = false)]
        public int? Width
        {
            get
            {
                return (int)Convert.ChangeType(this["width"], typeof(int));
            }
            set
            {
                IsDataDirty = true;
                this["width"] = value;
            }
        }

        /// <summary>
        /// Default Height to use for the images
        /// </summary>
        [ConfigurationProperty("height", IsRequired = false)]
        public int? Height
        {
            get
            {
                return (int)Convert.ChangeType(this["height"], typeof(int));
            }
            set
            {
                IsDataDirty = true;
                this["height"] = value;
            }
        }

        /// <summary>
        /// Default Top position for the images
        /// </summary>
        [ConfigurationProperty("left", IsRequired = false)]
        public int? Left
        {
            get
            {
                return (int)Convert.ChangeType(this["left"], typeof(int));
            }
            set
            {
                IsDataDirty = true;
                this["left"] = value;
            }
        }


        /// <summary>
        /// Default Top position for the images
        /// </summary>
        [ConfigurationProperty("top", IsRequired = false)]
        public int? Top
        {
            get
            {
                return (int)Convert.ChangeType(this["top"], typeof(int));
            }
            set
            {
                IsDataDirty = true;
                this["top"] = value;
            }
        }

        #endregion MFD Common Left, Top, Wdith and Height

        #region MFD X Offsets

        /// <summary>
        /// Default for the starting X position of the crop
        /// </summary>
        [ConfigurationProperty("xOffsetStart", IsRequired = false)]
        public int? XOffsetStart
        {
            get
            {
                return (int)Convert.ChangeType(this["xOffsetStart"], typeof(int));
            }
            set
            {
                IsDataDirty = true;
                this["xOffsetStart"] = value;
            }
        }

        /// <summary>
        /// Default for the ending X position of the crop of the image
        /// </summary>
        [ConfigurationProperty("xOffsetFinish", IsRequired = false)]
        public int? XOffsetFinish
        {
            get
            {
                return (int)Convert.ChangeType(this["xOffsetFinish"], typeof(int));
            }
            set
            {
                IsDataDirty = true;
                this["xOffsetFinish"] = value;
            }
        }

        #endregion MFD X Offsets

        #region MFD Y Offsets

        /// <summary>
        /// Default for the starting Y position of the crop of the image
        /// </summary>
       [ConfigurationProperty("yOffsetStart", IsRequired = false)]
        public int? YOffsetStart
        {
            get
            {
                return (int)Convert.ChangeType(this["yOffsetStart"], typeof(int));
            }
            set
            {
                IsDataDirty = true;
                this["yOffsetStart"] = value;
            }
        }

        /// <summary>
        /// Default for the ending Y position of the crop of the image
        /// </summary>
        [ConfigurationProperty("yOffsetFinish", IsRequired = false)]
        public int? YOffsetFinish
        {
            get
            {
                return (int)Convert.ChangeType(this["yOffsetFinish"], typeof(int));
            }
            set
            {
                IsDataDirty = true;
                this["yOffsetFinish"] = value;
            }
        }

        #endregion MFD Y Offsets

        #region Modules Collection

        /// <summary>
        /// MFD Configurations
        /// </summary>
        [ConfigurationProperty("Modules", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(ModulesConfigurationCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public ModulesConfigurationCollection Modules
        {
            get
            {
                object o = this["Modules"];
                return o as ModulesConfigurationCollection;
            }

            set
            {
                IsDataDirty = true;
                this["Modules"] = value;
            }
        }

        #endregion Modules Collection

    }
}
