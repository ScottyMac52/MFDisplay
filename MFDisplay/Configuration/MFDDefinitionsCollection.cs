using MFDisplay.Interfaces;
using System.Configuration;

namespace MFDisplay.Configuration
{
    /// <summary>
    /// Collection of MFD configurations
    /// </summary>
    public class MFDDefinitionsCollection : ConfigurationElementCollection
    {
        public MFDDefintion this[int index]
        {
            get
            {
                return BaseGet(index) as MFDDefintion;
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

        public new MFDDefintion this[string responseString]
        {
            get { return (MFDDefintion)BaseGet(responseString); }
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
            return new MFDDefintion();
        }

        /// <summary>
        /// Each unique entry is IntegrationType+SubWorkflowKey
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MFDDefintion)element).Name + ((MFDDefintion)element).FileName;
        }

        /// <summary>
        /// Public add needed to provide a method for adding items to the collection
        /// </summary>
        /// <param name="definition"></param>
        public void Add(MFDDefintion definition)
        {
            BaseAdd(definition);
        }

        /// <summary>
        /// Public remove needed to provide a method for removing items from the collection
        /// </summary>
        /// <param name="defintion"></param>
        public void Remove(MFDDefintion defintion)
        {
            BaseRemove(defintion.Name + defintion.FileName);
        }
    }
}