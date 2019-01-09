using System;
using System.Configuration;

namespace MFDSettingsManager.Configuration
{
    /// <summary>
    /// Encapsulates a MFD configuration
    /// </summary>
    public class MFDDefintion : ConfigurationElement
    {

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
        /// Opacity setting for the image
        /// </summary>
        [ConfigurationProperty("opacity", IsRequired = true)]
        public float Opacity
        {
            get
            {
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

        #region MFD Common Top, Wdith and Height

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
        /// Nullable Top position of the image in the display, used to override the default
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

        #endregion MFD Common Top, Wdith and Height

        #region L&R MFD Left

        /// <summary>
        /// Nullable Left position of the LMFD image, used to override the default
        /// </summary>
        [ConfigurationProperty("lMfdLeft", IsRequired = false)]
        public int? LMFDLeft
        {
            get
            {
                if (this["lMfdLeft"] == null)
                {
                    return null;
                }
                return (int)Convert.ChangeType(this["lMfdLeft"], typeof(int));
            }
            set
            {
                this["lMfdLeft"] = value;
            }
        }

        /// <summary>
        /// Nullable Left position of the RMFD image, used to override the default
        /// </summary>
        [ConfigurationProperty("rMfdLeft", IsRequired = false)]
        public int? RMFDLeft
        {
            get
            {
                if (this["rMfdLeft"] == null)
                {
                    return null;
                }
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
        /// Nullable starting X position of the crop of the image for the LMFD, used to override the default
        /// </summary>
        [ConfigurationProperty("xLMFDOffsetStart", IsRequired = false)]
        public int? XLFMDOffsetStart
        {
            get
            {
                if (this["xLMFDOffsetStart"] == null)
                {
                    return null;
                }
                return (int)Convert.ChangeType(this["xLMFDOffsetStart"], typeof(int));
            }
            set
            {
                this["xLMFDOffsetStart"] = value;
            }
        }

        /// <summary>
        /// Nullable ending X position of the crop of the image for the LMFD, used to override the default
        /// </summary>
        [ConfigurationProperty("xLMFDOffsetFinish", IsRequired = false)]
        public int? XLFMDOffsetFinish
        {
            get
            {
                if (this["xLMFDOffsetFinish"] == null)
                {
                    return null;
                }
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
        /// Nullable starting X position of the crop of the image for the RMFD, used to override the default
        /// </summary>
        [ConfigurationProperty("xRMFDOffsetStart", IsRequired = false)]
        public int? XRFMDOffsetStart
        {
            get
            {
                if (this["xRMFDOffsetStart"] == null)
                {
                    return null;
                }
                return (int)Convert.ChangeType(this["xRMFDOffsetStart"], typeof(int));
            }
            set
            {
                this["xRMFDOffsetStart"] = value;
            }
        }

        /// <summary>
        /// Nullable ending X position of the crop of the image for the RMFD, used to override the default
        /// </summary>
        [ConfigurationProperty("xRMFDOffsetFinish", IsRequired = false)]
        public int? XRFMDOffsetFinish
        {
            get
            {
                if (this["xRMFDOffsetFinish"] == null)
                {
                    return null;
                }
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
                if(this["yOffsetFinish"] == null)
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

        #endregion MFD Y Offsets
    }
}