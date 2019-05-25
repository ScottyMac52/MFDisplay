using System;

namespace MFDSettingsManager.Configuration.Collections
{
    /// <summary>
    /// Module configuration collection
    /// </summary>
    public class ModulesConfigurationCollection : ConfigurationSectionCollectionBase<ModuleConfiguration>
    {
        /// <summary>
        /// Definition of a key
        /// </summary>
        /// <returns></returns>
        protected override Func<ModuleConfiguration, object> GetDefaultKey()
        {
            return (element) =>
            {
                return element.ModuleName;
            };

        }
    }
}