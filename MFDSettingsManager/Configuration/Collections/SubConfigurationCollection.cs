using System;

namespace MFDSettingsManager.Configuration.Collections
{
    /// <summary>
    /// 
    /// </summary>
    public class SubConfigurationCollection : ConfigurationSectionCollectionBase<SubConfiguration>
    {
        /// <summary>
        /// Definition of a key
        /// </summary>
        /// <returns></returns>
        protected override Func<SubConfiguration, object> GetDefaultKey()
        {
            return (subconfig) =>
            {
                return subconfig.Name;
            };
        }
    }
}