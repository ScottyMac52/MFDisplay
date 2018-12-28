using MFDisplay.Interfaces;
using System;
using System.Configuration;

namespace MFDisplay.Configuration
{
    /// <summary>
    /// Encapsulates a Configuration section for the MFDs
    /// </summary>
    public class MFDConfigurationSection : ConfigurationSection
    {
        public static MFDConfigurationSection GetConfig()
        {
            return (MFDConfigurationSection)ConfigurationManager.GetSection("MFDSettings") ?? new MFDConfigurationSection();
        }

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

        #region MFD Common Top, Wdith and Height

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
    }
}
