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
        /// <summary>
        /// 
        /// </summary>
        public bool IsDataDirty { get; set; }

        /// <summary>
        /// Logger 
        /// </summary>
        public ILog Logger { get; set; }

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
                            currentModule.MFDConfigurations.Add(new MFDDefintion()
                            {
                                FileName = currentModule.FileName,
                                Height = dc.Height,
                                Width = dc.Width,
                                Left = dc.Left,
                                Top = dc.Top,
                                Name = dc.Name,
                                XOffsetStart = dc.XOffsetStart,
                                XOffsetFinish = dc.XOffsetFinish,
                                YOffsetStart = dc.YOffsetStart,
                                YOffsetFinish = dc.YOffsetFinish,
                                Opacity = dc.Opacity
                            });
                        }
                    });
                    return currentModule;
                }
            }
            return null;
        }

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
        /// ImageType to use when SaveClips == true
        /// </summary>
        [ConfigurationProperty("imageType", IsRequired = false)]
        public SavedImageType? ImageType
        {
            get
            {
                try
                {
                    var result = this["imageType"] == null ? SavedImageType.Bmp : (SavedImageType) this["imageType"];
                    return result;
                }
                catch (Exception ex)
                {
                    Logger?.Error("There was an error in determining the ImageType", ex);
                    return SavedImageType.Bmp;
                }
            }

            set
            {
                IsDataDirty = true;
                this["imageType"] = value ?? SavedImageType.Bmp;
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
