using System;
using System.Configuration;

namespace MFDSettingsManager.Configuration
{
    /// <summary>
    /// Base class for all configuration types
    /// </summary>
    public abstract class ConfigurationBase : ConfigurationElement
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ConfigurationBase()
        {

        }

        /// <summary>
        /// Base copy constructor
        /// </summary>
        /// <param name="dc"></param>
        public ConfigurationBase(ConfigurationBase dc) 
        {
            FileName = dc.FileName;
            Name = dc.Name;
            Enabled = dc.Enabled ?? true;
            XOffsetStart = dc.XOffsetStart;
            XOffsetFinish = dc.XOffsetFinish;
            YOffsetStart = dc.YOffsetStart;
            YOffsetFinish = dc.YOffsetFinish;
            Opacity = dc.Opacity;
        }

        #region General configuration properties

        /// <summary>
        /// Name of the Configuration, LMFD, RMFD etc.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return this["name"] as string;
            }
            set
            {
                this["name"] = value;
            }
        }

        /// <summary>
        /// Is this configuration enabled for use
        /// </summary>
        [ConfigurationProperty("enable", IsRequired = false, DefaultValue = true)]
        public bool? Enabled
        {
            get
            {
                var objectVar = this["enable"];
                return objectVar == null ? true : Convert.ToBoolean(objectVar);
            }
            set
            {
                this["enable"] = value;
            }
        }

        /// <summary>
        /// Opacity setting for the image
        /// </summary>
        [ConfigurationProperty("opacity", IsRequired = false)]
        public float? Opacity
        {
            get
            {
                if (this["opacity"] == null)
                {
                    return null;
                }
                return (float)Convert.ChangeType(this["opacity"], typeof(float));
            }
            set
            {
                this["opacity"] = value;
            }
        }

        /// <summary>
        /// FileName for the image source
        /// </summary>
        [ConfigurationProperty("filename", IsRequired = false)]
        public string FileName
        {
            get
            {
                return this["filename"] as string;
            }
            set
            {
                this["filename"] = value;
            }
        }

        #endregion General configuration properties

        #region X Offsets

        /// <summary>
        /// Nullable starting X position of the crop of the image
        /// </summary>
        [ConfigurationProperty("xOffsetStart", IsRequired = false)]
        public int? XOffsetStart
        {
            get
            {
                if (this["xOffsetStart"] == null)
                {
                    return null;
                }
                return (int)Convert.ChangeType(this["xOffsetStart"], typeof(int));
            }
            set
            {
                this["xOffsetStart"] = value;
            }
        }

        /// <summary>
        /// Nullable ending X position of the crop of the image
        /// </summary>
        [ConfigurationProperty("xOffsetFinish", IsRequired = false)]
        public int? XOffsetFinish
        {
            get
            {
                if (this["xOffsetFinish"] == null)
                {
                    return null;
                }
                return (int)Convert.ChangeType(this["xOffsetFinish"], typeof(int));
            }
            set
            {
                this["xOffsetFinish"] = value;
            }
        }

        #endregion X Offsets

        #region Y Offsets

        /// <summary>
        /// Nullable starting Y position of the crop of the image, used to override the default
        /// </summary>
        [ConfigurationProperty("yOffsetStart", IsRequired = false)]
        public int? YOffsetStart
        {
            get
            {
                if (this["yOffsetStart"] == null)
                {
                    return null;
                }
                return (int)Convert.ChangeType(this["yOffsetStart"], typeof(int));
            }
            set
            {
                this["yOffsetStart"] = value;
            }
        }

        /// <summary>
        /// Nullable ending Y position of the crop of the image, used to override the default
        /// </summary>
        [ConfigurationProperty("yOffsetFinish", IsRequired = false)]
        public int? YOffsetFinish
        {
            get
            {
                if (this["yOffsetFinish"] == null)
                {
                    return null;
                }
                return (int)Convert.ChangeType(this["yOffsetFinish"], typeof(int));
            }
            set
            {
                this["yOffsetFinish"] = value;
            }
        }

        #endregion Y Offsets
    }
}
