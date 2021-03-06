﻿using MFDSettingsManager.Configuration;
using MFDSettingsManager.Models;
using System.Collections.Generic;
using System.IO;
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
            if (section == null)
            {
                return null;
            }

            var logger = section.Logger;
            var logSep = new string('-', 160);

            // Get the default MFD configuration for all modules
            logger.Info("Loading the default configurations...");
            logger.Info(logSep);
            var defaultConfigurations = GetMfdDefaults(section);
            logger.Info($"{defaultConfigurations.Count} default configurations were loaded.");
            logger.Info(logSep);
            // Create the top level model
            var moduleConfigurations = ConvertFromConfigSection(section);
            var moduleList = section.Modules.List;
            logger.Info($"Loading {moduleList.Count} Modules...");
            logger.Info(logSep);

            moduleList.ForEach(currentModule =>  
            {
                logger.Info($"Loading the Module named {currentModule.DisplayName} as {currentModule.ModuleName} using {currentModule.FileName}...");
                var modelModule = ConvertFromConfigSectionModule(moduleConfigurations, currentModule);

                // Add the Configurations
                currentModule.Configurations.List.ForEach(currentConfig =>
                {
                    var mfdConfiguration = ConvertFromConfigSectionDefinition(section, modelModule, currentConfig);
                    logger.Info($"Loading Configuration {mfdConfiguration.ToReadableString()}...");
                    modelModule.Configurations.Add(mfdConfiguration);
                });

                defaultConfigurations.ForEach(dc =>
                {
                    var existConfig = modelModule.Configurations.SingleOrDefault(mmc => mmc.Name == dc.Name);
                    if (existConfig == null)
                    {
                        dc.Logger = logger;
                        dc.ModuleName = modelModule.ModuleName;
                        dc.FileName = !string.IsNullOrEmpty(modelModule.FileName) ? modelModule.FileName : dc.FileName;
                        logger.Info($"Adding default configuration {dc.ToReadableString()}...");
                        var newMfdConfiguration = new ConfigurationDefinition(dc)
                        {
                            FilePath = section.FilePath
                        };
                        modelModule.Configurations.Add(newMfdConfiguration);
                    }
                    else
                    {
                        existConfig.Opacity = existConfig.Opacity > 0F ? existConfig.Opacity : dc.Opacity;
                        existConfig.Left = existConfig.Left == 0 ? dc.Left : existConfig.Left;
                        existConfig.Top = existConfig.Top == 0 ? dc.Top : existConfig.Top;
                        existConfig.Height = existConfig.Height == 0 ? dc.Height : existConfig.Height;
                        existConfig.Width = existConfig.Width == 0 ? dc.Width : existConfig.Width;
                        existConfig.XOffsetStart = existConfig.XOffsetStart == 0 ? dc.XOffsetStart : existConfig.XOffsetStart;
                        existConfig.XOffsetFinish = existConfig.XOffsetFinish == 0 ? dc.XOffsetFinish : existConfig.XOffsetFinish;
                        existConfig.YOffsetStart = existConfig.YOffsetStart == 0 ? dc.YOffsetStart : existConfig.YOffsetStart;
                        existConfig.YOffsetFinish = existConfig.YOffsetFinish == 0 ? dc.YOffsetFinish : existConfig.YOffsetFinish;
                        existConfig.FileName = string.IsNullOrEmpty(existConfig.FileName) ? dc.FileName : existConfig.FileName;
                        existConfig.ModuleName = modelModule.ModuleName;
                        existConfig.FilePath = section.FilePath;
                        var completePath = Path.Combine(section.FilePath, existConfig.FileName);
                        if(!File.Exists(completePath))
                        {
                            logger.Error($"Unable to find {completePath}");
                        }
                        else
                        {
                            logger.Info($"Updated configuration from default {dc.Name} -> {existConfig.ToReadableString()}.");
                        }
                    }
                });

                logger.Info($"Loaded Module named {modelModule.DisplayName} as {modelModule.ModuleName} with {modelModule.Configurations.Count} Configurations.");
                logger.Info(logSep);
                moduleConfigurations.Modules.Add(modelModule);
            });

            logger.Debug($"Loaded {moduleConfigurations.Modules.Count} Modules.");
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
                if ((mfdDefaultConfiguration?.Opacity ?? 0.0F) > 0.0F)
                {
                    logger.Info($"Loading the default Configuration {mfdDefaultConfiguration.ToReadableString()}...");
                    defaultConfigurations.Add(mfdDefaultConfiguration);
                }

            });
            return defaultConfigurations;
        }

        private static ModulesConfiguration ConvertFromConfigSection(MFDConfigurationSection section)
        {
            return new ModulesConfiguration()
            {
                Logger = section.Logger,
                FilePath = section.FilePath,
                DefaultConfig = section.DefaultConfig
            };
        }

        private static ModuleDefinition ConvertFromConfigSectionModule(ModulesConfiguration parent, ModuleConfiguration module)
        {
            return new ModuleDefinition()
            {
                Parent = parent,
                DisplayName = module.DisplayName,
                ModuleName = module.ModuleName,
                FileName = module.FileName
            };
        }

        private static ConfigurationDefinition ConvertFromConfigSectionDefinition(MFDConfigurationSection section, ModuleDefinition parent, Configuration.Configuration currentConfig)
        {
            var config = new ConfigurationDefinition()
            {
                Logger = section.Logger,
                ModuleName = parent?.ModuleName,
                FilePath = section.FilePath,
                FileName = !string.IsNullOrEmpty(currentConfig.FileName) ? currentConfig.FileName : parent?.FileName,
                Height = currentConfig.Height ?? 0,
                Name = currentConfig.Name,
                Opacity = currentConfig.Opacity ?? 0,
                Left = currentConfig.Left ?? 0,
                Top = currentConfig.Top ?? 0,
                Width = currentConfig.Width ?? 0,
                XOffsetStart = currentConfig.XOffsetStart ?? 0,
                XOffsetFinish = currentConfig.XOffsetFinish ?? 0,
                YOffsetStart = currentConfig.YOffsetStart ?? 0,
                YOffsetFinish = currentConfig.YOffsetFinish ?? 0,
                Enabled = currentConfig.Enabled ?? true,
                SubConfigurations = new List<SubConfigurationDefinition>()
            };

            currentConfig.SubConfigurations.List.ForEach((subConfig) =>
            {
                var currentSubConfig = new SubConfigurationDefinition()
                {
                    Enabled = subConfig.Enabled ?? currentConfig.Enabled ?? true,
                    EndX = subConfig.EndX ?? 0,
                    EndY = subConfig.EndY ?? 0,
                    StartX = subConfig.StartX ?? 0,
                    StartY = subConfig.StartY ?? 0,
                    FileName = subConfig.FileName ?? currentConfig.FileName ?? parent.FileName,
                    FilePath = section.FilePath,
                    Logger = section.Logger,
                    ModuleName = parent?.ModuleName,
                    Name = subConfig.Name,
                    Opacity = subConfig.Opacity ?? 0,
                    XOffsetStart = subConfig.XOffsetStart ?? 0,
                    XOffsetFinish = subConfig.XOffsetFinish ?? 0,
                    YOffsetStart = subConfig.YOffsetStart ?? 0,
                    YOffsetFinish = subConfig.YOffsetFinish ?? 0
                };

                config.SubConfigurations.Add(currentSubConfig);
            });

            return config; 
        }
    }
}
