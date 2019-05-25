using System;
using System.Collections.Generic;
using System.Configuration;

namespace MFDSettingsManager.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class SubConfiguration : ConfigurationBase
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public SubConfiguration()
        {

        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="dc"></param>
        public SubConfiguration(SubConfiguration dc) : base(dc)
        {
            StartX = dc.StartX;
            StartY = dc.StartY;
            EndX = dc.EndX;
            EndY = dc.EndY;
        }


        #region startX, startY, endX and endY

        /// <summary>
        /// Starting X position
        /// </summary>
        [ConfigurationProperty("startX", IsRequired = false)]
        public int? StartX
        {
            get
            {
                if (this["startX"] == null)
                {
                    return null;
                }

                return (int)Convert.ChangeType(this["startX"], typeof(int));
            }
            set
            {
                this["startX"] = value;
            }
        }

        /// <summary>
        /// Starting Y position
        /// </summary>
        [ConfigurationProperty("startY", IsRequired = false)]
        public int? StartY
        {
            get
            {
                if (this["startY"] == null)
                {
                    return null;
                }

                return (int)Convert.ChangeType(this["startY"], typeof(int));
            }
            set
            {
                this["startY"] = value;
            }
        }

        /// <summary>
        /// ending X position
        /// </summary>
        [ConfigurationProperty("endX", IsRequired = false)]
        public int? EndX
        {
            get
            {
                if (this["endX"] == null)
                {
                    return null;
                }

                return (int)Convert.ChangeType(this["endX"], typeof(int));
            }
            set
            {
                this["endX"] = value;
            }
        }

        /// <summary>
        /// ending Y position
        /// </summary>
        [ConfigurationProperty("endY", IsRequired = false)]
        public int? EndY
        {
            get
            {
                if (this["endY"] == null)
                {
                    return null;
                }

                return (int)Convert.ChangeType(this["endY"], typeof(int));
            }
            set
            {
                this["endY"] = value;
            }
        }


        #endregion startX, startY, endX and endY
    }
}