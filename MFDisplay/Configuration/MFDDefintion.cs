using MFDisplay.Interfaces;
using System;
using System.Configuration;

namespace MFDisplay.Configuration
{
    /// <summary>
    /// Encapsulates a MFD configuration
    /// </summary>
    public class MFDDefintion : ConfigurationElement
    {
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

        #region MFD Common Top, Wdith and Height

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