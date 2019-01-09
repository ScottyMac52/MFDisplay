using log4net;
using MFDSettingsManager.Configuration;
using MFDSettingsManager.Models;

namespace MFDSettingsManager.Mappers
{
    /// <summary>
    /// Configuration Section mapper
    /// </summary>
    public class ConfigSectionModelMapper
    {
        /// <summary>
        /// Get the <seealso cref="ModulesConfiguration"/> from the <seealso cref="MFDConfigurationSection"/>
        /// </summary>
        /// <param name="section"></param>
        /// <param name="logger"><seealso cref="ILog"/>Logger to use</param>
        /// <returns></returns>
        public static ModulesConfiguration MapFromConfigurationSection(MFDConfigurationSection section, ILog logger)
        {
            var moduleConfigurations = new ModulesConfiguration()
            {
                Logger = logger,
                FilePath = section.FilePath,
                DefaultConfig = section.DefaultConfig,
                SaveClips = section.SaveClips ?? false,
                ImageType = section.ImageType
            };
            var iterator = section.Modules.GetEnumerator();
            while(iterator.MoveNext())
            {
                var currentModule = (ModuleConfigurationDefintion) iterator.Current;
                logger.Debug($"Loading the Module named {currentModule.DisplayName} as {currentModule.ModuleName} using {currentModule.FileName}");
                var modelModule = new ModuleDefinition()
                {
                    Parent = moduleConfigurations,
                    DisplayName = currentModule.DisplayName,
                    ModuleName = currentModule.ModuleName
                };
                var mfdIterator = currentModule.MFDConfigurations.GetEnumerator();
                while(mfdIterator.MoveNext())
                {
                    var currentConfig = (MFDDefintion) mfdIterator.Current;
                    logger.Debug($"Loading the Configuration named {currentModule.DisplayName}-{currentConfig.Name}");
                    var mfdConfiguration = new ConfigurationDefinition()
                    {
                        Logger = moduleConfigurations.Logger,
                        Parent = modelModule,
                        ModuleName = modelModule.ModuleName,
                        FileName = !string.IsNullOrEmpty(currentConfig.FileName) ? currentConfig.FileName : currentModule.FileName,
                        Height = currentConfig.Height ?? section.Height,
                        Name = currentConfig.Name,
                        Opacity = currentConfig.Opacity,
                        Top = currentConfig.Top ?? section.Top,
                        Width = currentConfig.Width ?? section.Width,
                        YOffsetStart = currentConfig.YOffsetStart ?? section.YOffsetStart,
                        YOffsetFinish = currentConfig.YOffsetFinish ?? section.YOffsetFinish,
                        SaveResults = section.SaveClips,
                        UseOffsets = true
                    };
                    switch(currentConfig.Name)
                    {
                        case "LMFD":
                            mfdConfiguration.Left = currentConfig.LMFDLeft ?? section.LMFDLeft;
                            mfdConfiguration.XOffsetStart = currentConfig.XLFMDOffsetStart ?? section.XLFMDOffsetStart;
                            mfdConfiguration.XOffsetFinish = currentConfig.XLFMDOffsetFinish ?? section.XLFMDOffsetFinish;
                            break;
                        case "RMFD":
                            mfdConfiguration.Left = currentConfig.RMFDLeft ?? section.RMFDLeft;
                            mfdConfiguration.XOffsetStart = currentConfig.XRFMDOffsetStart ?? section.XRFMDOffsetStart;
                            mfdConfiguration.XOffsetFinish = currentConfig.XRFMDOffsetFinish ?? section.XRFMDOffsetFinish;
                            break;
                        default:
                            mfdConfiguration.Left = currentConfig.LMFDLeft ?? currentConfig.RMFDLeft ?? 0;
                            mfdConfiguration.XOffsetStart = currentConfig.XLFMDOffsetStart ?? currentConfig.XRFMDOffsetStart ?? 0;
                            mfdConfiguration.XOffsetFinish = currentConfig.XLFMDOffsetFinish ?? currentConfig.XRFMDOffsetFinish ?? 0;
                            break;
                    }
                    logger.Debug($"Loaded Configuration {currentModule.DisplayName}-{currentConfig.Name} as {mfdConfiguration.ToReadableString()}");
                    modelModule.Configurations.Add(mfdConfiguration);
                }
                logger.Debug($"Loaded Module {modelModule.ModuleName}");
                moduleConfigurations.Modules.Add(modelModule);
            }
            return moduleConfigurations;
        }
    }
}
