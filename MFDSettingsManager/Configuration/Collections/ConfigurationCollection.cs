using System;

namespace MFDSettingsManager.Configuration.Collections
{
    /// <summary>
    /// Collection of MFD configurations
    /// </summary>
    public class ConfigurationCollection : ConfigurationSectionCollectionBase<Configuration>
    {
        /// <summary>
        /// Definition of a key
        /// </summary>
        /// <returns></returns>
        protected override Func<Configuration, object> GetDefaultKey()
        {
            return (element) =>
            {
                return element.Name + element.FileName;
            };
        }
    }
}