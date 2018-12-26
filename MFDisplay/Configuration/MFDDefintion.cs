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

    }
}