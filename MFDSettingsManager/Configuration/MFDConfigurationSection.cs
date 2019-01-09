using System;
using System.Configuration;
using System.IO;
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
        /// Gets the configurations section
        /// </summary>
        /// <returns></returns>
        public static MFDConfigurationSection GetConfig()
        {
            var mappedExeConfig = ConfigurationManager.OpenExeConfiguration(Path.Combine(Environment.CurrentDirectory, "MFDisplay.exe"));
            var configSection = (MFDConfigurationSection)mappedExeConfig.GetSection("MFDSettings");
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
                    var result = this["imageType"] == null ? SavedImageType.Bmp : (SavedImageType)System.Enum.Parse(typeof(SavedImageType), (string)this["imageType"]);
                    return result;
                }
                catch (Exception)
                {

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
                IsDataDirty = true;
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
                IsDataDirty = true;
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
                IsDataDirty = true;
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
                IsDataDirty = true;
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
                IsDataDirty = true;
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
                IsDataDirty = true;
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
                IsDataDirty = true;
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
                IsDataDirty = true;
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
                IsDataDirty = true;
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
                IsDataDirty = true;
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
