using System.Configuration;

namespace MFDSettingsManager.Configuration
{
    /// <summary>
    /// Module configuration collection
    /// </summary>
    public class ModulesConfigurationCollection : ConfigurationElementCollection
    {
        #region Collection item access

        /// <summary>
        /// Access collection item by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ModuleConfigurationDefintion this[int index]
        {
            get
            {
                return BaseGet(index) as ModuleConfigurationDefintion;
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        /// <summary>
        /// Access collection item by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new ModuleConfigurationDefintion this[string key]
        {
            get { return (ModuleConfigurationDefintion)BaseGet(key); }
            set
            {
                if (BaseGet(key) != null)
                {
                    BaseRemoveAt(BaseIndexOf(BaseGet(key)));
                }
                BaseAdd(value);
            }
        }

        #endregion Collection item access

        #region Create new element for the collection

        /// <summary>
        /// Creates a new element for the collection
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ModuleConfigurationDefintion();
        }

        #endregion Create new element for the collection

        #region Key and item management
        
        /// <summary>
        /// Each unique entry is IntegrationType+SubWorkflowKey
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ModuleConfigurationDefintion)element).ModuleName;
        }

        /// <summary>
        /// Public add needed to provide a method for adding items to the collection
        /// </summary>
        /// <param name="definition"></param>
        public void Add(ModuleConfigurationDefintion definition)
        {
            BaseAdd(definition);
        }

        /// <summary>
        /// Public remove needed to provide a method for removing items from the collection
        /// </summary>
        /// <param name="defintion"></param>
        public void Remove(ModuleConfigurationDefintion defintion)
        {
            BaseRemove(defintion.ModuleName);
        }

        #endregion Key and item management
    }
}