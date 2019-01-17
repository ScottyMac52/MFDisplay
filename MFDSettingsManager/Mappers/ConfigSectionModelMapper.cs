using log4net;
using MFDSettingsManager.Configuration;
using MFDSettingsManager.Enum;
using MFDSettingsManager.Models;
using System.Collections.Generic;
using System.Linq;

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
        /// <returns></returns>
        internal static ModulesConfiguration MapFromConfigurationSection(MFDConfigurationSection section)
        {
            var logger = section.Logger;

            // Get the default MFD configuration for all modules
            var defaultConfigurations = GetMfdDefaults(section);

            // Create the top level model
            var moduleConfigurations = ConvertFromConfigSection(section);
            var moduleList = section.Modules.List;
            logger.Debug($"Loading {moduleList.Count} Modules.");
            moduleList.ForEach(currentModule =>
            {
                logger.Debug($"Loading the Module named {currentModule.DisplayName} as {currentModule.ModuleName} using {currentModule.FileName}");
                var modelModule = ConvertFromConfigSectionModule(moduleConfigurations, currentModule);
                var configurationList = currentModule.MFDConfigurations.List;
                // Add the Configurations
                configurationList.ForEach(currentConfig =>
                {
                    var mfdConfiguration = ConvertFromConfigSectionDefinition(section, modelModule, currentConfig);
                    logger.Debug($"Loaded Configuration {currentModule.DisplayName}-{currentConfig.Name} as {mfdConfiguration.ToReadableString()}");
                    modelModule.Configurations.Add(mfdConfiguration);
                });

                // Add the default configurations as applicable
                defaultConfigurations.ForEach(dc =>
                {
                    var existConfig = modelModule.Configurations.SingleOrDefault(mmc => mmc.Name == dc.Name);
                    if (existConfig == null)
                    {
                        var newMfdConfiguration = new ConfigurationDefinition(dc);
                        newMfdConfiguration.Logger = logger;
                        newMfdConfiguration.ModuleName = modelModule.ModuleName;
                        newMfdConfiguration.FileName = !string.IsNullOrEmpty(modelModule.FileName) ? modelModule.FileName : dc.FileName;
                        newMfdConfiguration.ImageType = modelModule.ImageType ?? section.ImageType ?? SavedImageType.Jpeg;
                        newMfdConfiguration.SaveResults = newMfdConfiguration.SaveResults ?? section.SaveClips ?? false;
                        logger.Debug($"Adding configuration {newMfdConfiguration.ToReadableString()} to module {modelModule.DisplayName}");
                        modelModule.Configurations.Add(newMfdConfiguration);
                    }
                });

                logger.Debug($"Loaded Module {modelModule.ModuleName}");
                moduleConfigurations.Modules.Add(modelModule);
            });
            return moduleConfigurations;
        }

        /// <summary>
        /// Loads the default configurations that are applied to ALL modules unless a module defines them
        /// </summary>
        /// <param name="section"><seealso cref="MFDConfigurationSection"/>Configuration section</param>
        /// <returns>List of defaults</returns>
        private static List<ConfigurationDefinition> GetMfdDefaults(MFDConfigurationSection section)
        {
            var logger = section.Logger;
            var defaultsCollection = section.DefaultConfigurations.List;
            var defaultConfigurations = new List<ConfigurationDefinition>();
            defaultsCollection.ForEach(dc =>
            {
                var mfdDefaultConfiguration = ConvertFromConfigSectionDefinition(section, null, dc);
                logger.Debug($"Loading the default Configuration {mfdDefaultConfiguration.ToReadableString()}");
                defaultConfigurations.Add(mfdDefaultConfiguration);

            });
            return defaultConfigurations;
        }

        private static ModulesConfiguration ConvertFromConfigSection(MFDConfigurationSection section)
        {
            return new ModulesConfiguration()
            {
                Logger = section.Logger,
                FilePath = section.FilePath,
                DefaultConfig = section.DefaultConfig,
                SaveClips = section.SaveClips ?? false,
                ImageType = section.ImageType
            };
        }

        private static ModuleDefinition ConvertFromConfigSectionModule(ModulesConfiguration parent, ModuleConfigurationDefintion module)
        {
            return new ModuleDefinition()
            {
                Parent = parent,
                DisplayName = module.DisplayName,
                ModuleName = module.ModuleName,
                FileName = module.FileName
            };
        }

        private static ConfigurationDefinition ConvertFromConfigSectionDefinition(MFDConfigurationSection section, ModuleDefinition parent, MFDDefintion currentConfig)
        {
            return new ConfigurationDefinition()
            {
                Logger = section.Logger,
                ImageType = parent?.ImageType ?? parent?.Parent?.ImageType ?? SavedImageType.Jpeg,
                ModuleName = parent?.ModuleName,
                FileName = !string.IsNullOrEmpty(currentConfig.FileName) ? currentConfig.FileName : parent?.FileName,
                Height = currentConfig.Height ?? 0,
                Name = currentConfig.Name,
                Opacity = currentConfig.Opacity,
                Left = currentConfig.Left ?? 0,
                Top = currentConfig.Top ?? 0,
                Width = currentConfig.Width ?? 0,
                XOffsetStart = currentConfig.XOffsetStart ?? 0,
                XOffsetFinish = currentConfig.XOffsetFinish ?? 0,
                YOffsetStart = currentConfig.YOffsetStart ?? 0,
                YOffsetFinish = currentConfig.YOffsetFinish ?? 0,
                SaveResults = section.SaveClips
           };

        }
    }
}
