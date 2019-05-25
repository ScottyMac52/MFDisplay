using MFDSettingsManager.Configuration.Collections;
using System;
using System.Configuration;

namespace MFDSettingsManager.Configuration
{
    /// <summary>
    /// Module configuration
    /// </summary>
    public class ModuleConfiguration : ConfigurationElement
    {
        #region Module configuration properties

        /// <summary>
        /// Name of the Module
        /// </summary>
        [ConfigurationProperty("moduleName", IsRequired = true)]
        public string ModuleName
        {
            get
            {
                return this["moduleName"] as string;
            }
            set
            {
                this["moduleName"] = value;
            }
        }

        /// <summary>
        /// Display name of the Module
        /// </summary>
        [ConfigurationProperty("displayName", IsRequired = true)]
        public string DisplayName
        {
            get
            {
                return this["displayName"] as string;
            }
            set
            {
                this["displayName"] = value;
            }
        }

        /// <summary>
        /// Default FileName for the Module
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

        #endregion Module configuration properties

        #region MFD Collection 

        /// <summary>
        /// MFD Configurations
        /// </summary>
        [ConfigurationProperty("Configurations", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(ConfigurationCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public ConfigurationCollection Configurations
        {
            get
            {
                object o = this["Configurations"];
                return o as ConfigurationCollection;
            }

            set
            {
                this["Configurations"] = value;
            }
        }

        /// <summary>
        /// Gets the specified configuration
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Configuration GetMFDConfiguration(string name)
        {
            var iterator = Configurations.GetEnumerator();
            while(iterator.MoveNext())
            {
                var currentConfig = (Configuration) iterator.Current;
                if(currentConfig.Name == name)
                {
                    return currentConfig;
                }
            }

            return null;
        }

        #endregion MFD Collection 
    }
}