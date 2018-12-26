using MFDisplay.Configuration;
using MFDisplay.Interfaces;
using MFDisplay.Models;

namespace MFDisplay.Mappers
{
    public class ConfigSectionModelMapper
    {
        public static IModulesConfiguration MapFromConfigurationSection(MFDConfigurationSection section)
        {
            var moduleConfigurations = new ModulesConfiguration()
            {
                FilePath = section.FilePath,
                DefaultConfig = section.DefaultConfig
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
                        FileName = !string.IsNullOrEmpty(currentConfig.FileName) ? currentConfig.FileName : currentModule.FileName,
                        Height = section.Height,
                        Name = currentConfig.Name,
                        Opacity = currentConfig.Opacity,
                        Top = section.Top,
                        Width = section.Width,
                        YOffsetStart = section.YOffsetStart,
                        YOffsetFinish = section.YOffsetFinish,
                        UseOffsets = true
                    };
                    switch(currentConfig.Name)
                    {
                        case "LMFD":
                            mfdConfiguration.Left = section.LMFDLeft;
                            mfdConfiguration.XOffsetStart = section.XLFMDOffsetStart;
                            mfdConfiguration.XOffsetFinish = section.XLFMDOffsetFinish;
                            break;
                        case "RMFD":
                            mfdConfiguration.Left = section.RMFDLeft;
                            mfdConfiguration.XOffsetStart = section.XRFMDOffsetStart;
                            mfdConfiguration.XOffsetFinish = section.XRFMDOffsetFinish;
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
