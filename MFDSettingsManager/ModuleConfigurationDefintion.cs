using System.Configuration;

namespace MFDSettingsManager
{
    /// <summary>
    /// Module configuration
    /// </summary>
    public class ModuleConfigurationDefintion : ConfigurationElement
    {
        #region Module configuration properties

        /// <summary>
        /// Name of the Module
        /// </summary>
        [ConfigurationProperty("moduleName", IsRequired = false)]
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
        [ConfigurationProperty("displayName", IsRequired = false)]
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
        [ConfigurationProperty("filename", IsRequired = true)]
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
        [ConfigurationCollection(typeof(MFDDefinitionsCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public MFDDefinitionsCollection MFDConfigurations
        {
            get
            {
                object o = this["Configurations"];
                return o as MFDDefinitionsCollection;
            }

            set
            {
                this["Configurations"] = value;
            }
        }

        #endregion MFD Collection 
    }
}