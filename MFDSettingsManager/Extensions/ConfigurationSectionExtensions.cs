using log4net;
using MFDSettingsManager.Configuration;
using MFDSettingsManager.Mappers;
using MFDSettingsManager.Models;

namespace MFDSettingsManager.Extensions
{
    /// <summary>
    /// Extensions for the Configuration Section objects and the Model objects
    /// </summary>
    public static class ConfigurationSectionExtensions
    {
        /// <summary>
        /// Converts a Configuration Section to a Model
        /// </summary>
        /// <param name="section"></param>
        /// <param name="logger"><seealso cref="ILog"/></param>
        /// <returns></returns>
        public static ModulesConfiguration ToModel(this MFDConfigurationSection section, ILog logger)
        {
            if(section == null)
            {
                return null;
            }
            var model = ConfigSectionModelMapper.MapFromConfigurationSection(section);
            model.Logger = logger;
            return model;
        }

    }
}
