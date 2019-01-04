using MFDisplay.Models;
using MFDSettingsManager;

namespace MFDisplay.Mappers
{
    public class ConfigSectionModelMapper
    {
        public static ModulesConfiguration MapFromConfigurationSection(MFDConfigurationSection section)
        {
            var moduleConfigurations = new ModulesConfiguration()
            {
                FilePath = section.FilePath,
                DefaultConfig = section.DefaultConfig,
                SaveClips = section.SaveClips ?? false
            };
            var iterator = section.Modules.GetEnumerator();
            while(iterator.MoveNext())
            {
                var currentModule = (ModuleConfigurationDefintion) iterator.Current;
                var modelModule = new ModuleDefinition()
                {
                    DisplayName = currentModule.DisplayName,
                    ModuleName = currentModule.ModuleName,
                };
                var mfdIterator = currentModule.MFDConfigurations.GetEnumerator();
                while(mfdIterator.MoveNext())
                {
                    var currentConfig = (MFDDefintion) mfdIterator.Current;
                    var mfdConfiguration = new ConfigurationDefinition()
                    {
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
                    modelModule.Configurations.Add(mfdConfiguration);
                }
                moduleConfigurations.Modules.Add(modelModule);
            }
            return moduleConfigurations;
        }
    }
}
