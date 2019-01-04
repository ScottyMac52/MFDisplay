using System;
using System.Configuration;

namespace MFDSettingsManager
{
    /// <summary>
    /// Encapsulates a Configuration section for the MFDs
    /// </summary>
    public class MFDConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// Gets the configurations section
        /// </summary>
        /// <returns></returns>
        public static MFDConfigurationSection GetConfig()
        {
            return (MFDConfigurationSection)ConfigurationManager.GetSection("MFDSettings") ?? new MFDConfigurationSection();
        }

        #region Main configuration properties

        /// <summary>
        /// SaveClips results in the cropped images being saved
        /// </summary>
        [ConfigurationProperty("saveClips", IsRequired = true)]
        public bool? SaveClips
        {
            get
            {
                return (bool?)this["saveClips"];
            }
            set
            {
                this["saveClips"] = value;
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
                this["defaultConfig"] = value;
            }
        }

        #endregion Main configuration properties

        #region MFD Common Top, Wdith and Height

        /// <summary>
        /// Default Width to use for the images
        /// </summary>
        [ConfigurationProperty("width", IsRequired = true)]
        public int Width
        {
            get
            {
                return (int)Convert.ChangeType(this["width"], typeof(int));
            }
            set
            {
                this["width"] = value;
            }
        }

        /// <summary>
        /// Default Height to use for the images
        /// </summary>
        [ConfigurationProperty("height", IsRequired = true)]
        public int Height
        {
            get
            {
                return (int)Convert.ChangeType(this["height"], typeof(int));
            }
            set
            {
                this["height"] = value;
            }
        }

        /// <summary>
        /// Default Top position for the images
        /// </summary>
        [ConfigurationProperty("top", IsRequired = true)]
        public int Top
        {
            get
            {
                return (int)Convert.ChangeType(this["top"], typeof(int));
            }
            set
            {
                this["top"] = value;
            }
        }

        #endregion MFD Common Top, Wdith and Height

        #region L&R MFD Left

        /// <summary>
        /// Default Left position for the LMFD keyed images
        /// </summary>
        [ConfigurationProperty("lMfdLeft", IsRequired = true)]
        public int LMFDLeft
        {
            get
            {
                return (int)Convert.ChangeType(this["lMfdLeft"], typeof(int));
            }
            set
            {
                this["lMfdLeft"] = value;
            }
        }

        /// <summary>
        /// Default Left position for the RMFD keyed images
        /// </summary>
        [ConfigurationProperty("rMfdLeft", IsRequired = true)]
        public int RMFDLeft
        {
            get
            {
                return (int)Convert.ChangeType(this["rMfdLeft"], typeof(int));
            }
            set
            {
                this["rMfdLeft"] = value;
            }
        }

        #endregion L&R MFD Left

        #region LMFD X Offsets

        /// <summary>
        /// Default for the starting X position of the crop of the image for the LMFD
        /// </summary>
        [ConfigurationProperty("xLMFDOffsetStart", IsRequired = true)]
        public int XLFMDOffsetStart
        {
            get
            {
                return (int)Convert.ChangeType(this["xLMFDOffsetStart"], typeof(int));
            }
            set
            {
                this["xLMFDOffsetStart"] = value;
            }
        }

        /// <summary>
        /// Default for the ending X position of the crop of the image for the LMFD
        /// </summary>
        [ConfigurationProperty("xLMFDOffsetFinish", IsRequired = true)]
        public int XLFMDOffsetFinish
        {
            get
            {
                return (int)Convert.ChangeType(this["xLMFDOffsetFinish"], typeof(int));
            }
            set
            {
                this["xLMFDOffsetFinish"] = value;
            }
        }

        #endregion LMFD X Offsets

        #region RMFD X Offsets

        /// <summary>
        /// Default for the starting X position of the crop of the image for the RMFD
        /// </summary>
        [ConfigurationProperty("xRMFDOffsetStart", IsRequired = true)]
        public int XRFMDOffsetStart
        {
            get
            {
                return (int)Convert.ChangeType(this["xRMFDOffsetStart"], typeof(int));
            }
            set
            {
                this["xRMFDOffsetStart"] = value;
            }
        }

        /// <summary>
        /// Default for the ending X position of the crop of the image for the RMFD
        /// </summary>
        [ConfigurationProperty("xRMFDOffsetFinish", IsRequired = true)]
        public int XRFMDOffsetFinish
        {
            get
            {
                return (int)Convert.ChangeType(this["xRMFDOffsetFinish"], typeof(int));
            }
            set
            {
                this["xRMFDOffsetFinish"] = value;
            }
        }

        #endregion RMFD X Offsets

        #region MFD Y Offsets

        /// <summary>
        /// Default for the starting Y position of the crop of the image
        /// </summary>
       [ConfigurationProperty("yOffsetStart", IsRequired = true)]
        public int YOffsetStart
        {
            get
            {
                return (int)Convert.ChangeType(this["yOffsetStart"], typeof(int));
            }
            set
            {
                this["yOffsetStart"] = value;
            }
        }

        /// <summary>
        /// Default for the ending Y position of the crop of the image
        /// </summary>
        [ConfigurationProperty("yOffsetFinish", IsRequired = true)]
        public int YOffsetFinish
        {
            get
            {
                return (int)Convert.ChangeType(this["yOffsetFinish"], typeof(int));
            }
            set
            {
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
                this["Modules"] = value;
            }
        }

        #endregion Modules Collection

    }
}
