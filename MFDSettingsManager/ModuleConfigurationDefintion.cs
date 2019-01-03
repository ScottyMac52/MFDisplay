using System.Configuration;

namespace MFDSettingsManager
{
    public class ModuleConfigurationDefintion : ConfigurationElement
    {
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
    }
}