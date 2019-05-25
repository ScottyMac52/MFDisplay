using MFDSettingsManager.Configuration.Collections;
using System;
using System.Configuration;

namespace MFDSettingsManager.Configuration
{
    /// <summary>
    /// Encapsulates a MFD configuration
    /// </summary>
    public class Configuration : ConfigurationBase
    {

        /// <summary>
        /// Default constructor
        /// </summary>
        public Configuration()
        {
        }

        /// <summary>
        /// Sub-Configurations
        /// </summary>
        [ConfigurationProperty("SubConfigurations", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(SubConfigurationCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public SubConfigurationCollection SubConfigurations
        {
            get
            {
                object o = this["SubConfigurations"];
                return o as SubConfigurationCollection;
            }

            set
            {
                this["SubConfigurations"] = value;
            }
        }


        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="dc"></param>
        public Configuration(Configuration dc) : base(dc)
        {
            Height = dc.Height;
            Width = dc.Width;
            Left = dc.Left;
            Top = dc.Top;
        }

        #region Left, Top, Wdith and Height

        /// <summary>
        /// Nullable Width of the image in the display, used to override the default
        /// </summary>
        [ConfigurationProperty("width", IsRequired = false)]
        public int? Width
        {
            get
            {
                if (this["width"] == null)
                {
                    return null;
                }
                return (int)Convert.ChangeType(this["width"], typeof(int));
            }
            set
            {
                this["width"] = value;
            }
        }

        /// <summary>
        /// Nullable Height of the image in the display, used to override the default
        /// </summary>
        [ConfigurationProperty("height", IsRequired = false)]
        public int? Height
        {
            get
            {
                if (this["height"] == null)
                {
                    return null;
                }
                return (int)Convert.ChangeType(this["height"], typeof(int));
            }
            set
            {
                this["height"] = value;
            }
        }

        /// <summary>
        /// Nullable Left position of the image in the display
        /// </summary>
        [ConfigurationProperty("left", IsRequired = false)]
        public int? Left
        {
            get
            {
                if (this["left"] == null)
                {
                    return null;
                }
                return (int)Convert.ChangeType(this["left"], typeof(int));
            }
            set
            {
                this["left"] = value;
            }
        }
        
        /// <summary>
        /// Nullable Top position of the image in the display
        /// </summary>
        [ConfigurationProperty("top", IsRequired = false)]
        public int? Top
        {
            get
            {
                if (this["top"] == null)
                {
                    return null;
                }
                return (int)Convert.ChangeType(this["top"], typeof(int));
            }
            set
            {
                this["top"] = value;
            }
        }

        #endregion Left, Top, Wdith and Height
    }
}