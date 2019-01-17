using MFDSettingsManager.Configuration;
using MFDSettingsManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MFDSettingsManager.Mappers;
using log4net;

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
            var model = ConfigSectionModelMapper.MapFromConfigurationSection(section);
            model.Logger = logger;
            return model;
        }

    }
}
