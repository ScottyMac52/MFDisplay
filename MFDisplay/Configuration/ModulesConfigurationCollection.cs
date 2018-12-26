using System.Configuration;

namespace MFDisplay.Configuration
{
    public class ModulesConfigurationCollection : ConfigurationElementCollection
    {
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

        public new ModuleConfigurationDefintion this[string responseString]
        {
            get { return (ModuleConfigurationDefintion)BaseGet(responseString); }
            set
            {
                if (BaseGet(responseString) != null)
                {
                    BaseRemoveAt(BaseIndexOf(BaseGet(responseString)));
                }
                BaseAdd(value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ModuleConfigurationDefintion();
        }

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

    }
}