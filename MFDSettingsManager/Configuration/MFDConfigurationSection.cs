using System;
using System.Configuration;
using System.IO;
using System.Linq;
using log4net;
using MFDSettingsManager.Enum;

namespace MFDSettingsManager.Configuration
{
    /// <summary>
    /// Encapsulates a Configuration section for the MFDs
    /// </summary>
    public class MFDConfigurationSection : ConfigurationSection
    {
        private const string XmlNamespaceConfigurationPropertyName = "xmlns";
        private const string XsiNamespaceConfigurationPropertyName = "xmlns:xsi";
        private const string XsiSchemaLocationNamespaceConfigurationPropertyName = "xsi:schemaLocation";

        /// <summary>
        /// Tracks changes 
        /// </summary>
        public bool IsDataDirty { get; internal set; }

        /// <summary>
        /// Logger 
        /// </summary>
        public ILog Logger { get; internal set; }

        /// <summary>
        /// Gets the configurations section
        /// </summary>
        /// <returns></returns>
        public static MFDConfigurationSection GetConfig(ILog logger, string fullPathtoConfig = null)
        {
            System.Configuration.Configuration currentConfiguration = null;

            if(string.IsNullOrEmpty(fullPathtoConfig))
            {
                currentConfiguration = ConfigurationManager.OpenExeConfiguration(Path.Combine(Environment.CurrentDirectory, "MFDisplay.exe"));
            }
            else
            {
                var exeFileMap = new ExeConfigurationFileMap()
                {
                    ExeConfigFilename = fullPathtoConfig
                };
                currentConfiguration = ConfigurationManager.OpenMappedExeConfiguration(exeFileMap, ConfigurationUserLevel.None);
            }

            var configSection = (MFDConfigurationSection)currentConfiguration.GetSection("MFDSettings");
            configSection.Logger = logger;
            configSection.IsDataDirty = false;
            return configSection ?? new MFDConfigurationSection();
        }

        /// <summary>
        /// Get the module
        /// </summary>
        /// <param name="moduleName"></param>
        /// <returns><seealso cref="ModuleConfigurationDefintion"/></returns>
        public ModuleConfigurationDefintion GetModuleConfiguration(string moduleName)
        {
            var iterator = Modules.GetEnumerator();
            while(iterator.MoveNext())
            {
                var currentModule = (ModuleConfigurationDefintion)iterator.Current;
                if(currentModule.ModuleName == moduleName)
                {
                    var defaultConfigs = DefaultConfigurations.List;
                    defaultConfigs.ForEach(dc =>
                    {
                        var existingConfig = currentModule.MFDConfigurations.List.FirstOrDefault(mfdConfig => mfdConfig.Name == dc.Name);
                        if(existingConfig == null)
                        {
                            var newDef = new MFDDefintion(dc);
                            dc.FileName = currentModule.FileName;
                            currentModule.MFDConfigurations.Add(newDef);
                        }
                    });
                    return currentModule;
                }
            }
            return null;
        }

        #region Properties to support schema definitions 

        /// <summary>
        /// Define the xmlns attribute
        /// </summary>
        [ConfigurationProperty(XmlNamespaceConfigurationPropertyName, IsRequired = false)]
        public string XmlNamespace
        {
            get
            {
                return (string)this[XmlNamespaceConfigurationPropertyName];
            }
            internal set
            {
                this[XmlNamespaceConfigurationPropertyName] = value;
            }
        }

        /// <summary>
        /// Define the xmlns:xsi attribute
        /// </summary>
        [ConfigurationProperty(XsiNamespaceConfigurationPropertyName, IsRequired = false)]
        public string XsiNamespaceConfigurationName
        {
            get
            {
                return (string)this[XsiNamespaceConfigurationPropertyName];
            }
            internal set
            {
                this[XsiNamespaceConfigurationPropertyName] = value;
            }
        }

        /// <summary>
        /// Define the xsi:schemaLocation attribute
        /// </summary>
        [ConfigurationProperty(XsiSchemaLocationNamespaceConfigurationPropertyName, IsRequired = false)]
        public string XsiSchemaLocationNamespaceConfigurationName
        {
            get
            {
                return (string)this[XsiSchemaLocationNamespaceConfigurationPropertyName];
            }
            internal set
            {
                this[XsiSchemaLocationNamespaceConfigurationPropertyName] = value;
            }
        }

        #endregion Properties to support schema definitions 

        #region Main configuration properties

        /// <summary>
        /// SaveClips results in the cropped images being saved
        /// </summary>
        [ConfigurationProperty("saveClips", IsRequired = false)]
        public bool? SaveClips
        {
            get
            {
                return (bool?)this["saveClips"];
            }
            set
            {
                IsDataDirty = true;
                this["saveClips"] = value;
            }
        }

        /// <summary>
        /// The file path to the images to be cropped
        /// </summary>
        [ConfigurationProperty("filePath", IsRequired = true)]
        public string FilePath
        {
            get
            {
                return this["filePath"] as string;
            }
            set
            {
                IsDataDirty = true;
                this["filePath"] = value;
            }
        }

        /// <summary>
        /// The default configuration to load
        /// </summary>
        [ConfigurationProperty("defaultConfig", IsRequired = false)]
        public string DefaultConfig
        {
            get
            {
                return this["defaultConfig"] as string;
            }
            set
            {
                IsDataDirty = true;
                this["defaultConfig"] = value;
            }
        }

        #endregion Main configuration properties

        #region Default Configurations

        /// <summary>
        /// MFD Default Configurations
        /// </summary>
        [ConfigurationProperty("DefaultConfigurations", IsDefaultCollection = false, IsRequired = false)]
        [ConfigurationCollection(typeof(MFDDefinitionsCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public MFDDefinitionsCollection DefaultConfigurations
        {
            get
            {
                object o = this["DefaultConfigurations"];
                return o as MFDDefinitionsCollection;
            }

            set
            {
                IsDataDirty = true;
                this["DefaultConfigurations"] = value;
            }
        }
               
        #endregion Default Configurations

        #region Modules Collection

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
                IsDataDirty = true;
                this["Modules"] = value;
            }
        }

        #endregion Modules Collection

    }
}
