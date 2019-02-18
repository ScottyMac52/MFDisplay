using System.Collections.Generic;
using System.Configuration;

namespace MFDSettingsManager.Configuration
{
    /// <summary>
    /// Collection of MFD configurations
    /// </summary>
    public class DefaultsDefinitionsCollection : ConfigurationElementCollection
    {

        /// <summary>
        /// Exposes the Collection as a List
        /// </summary>
        public List<MFDDefaultDefintion> List => GetAsList();

        private List<MFDDefaultDefintion> GetAsList()
        {
            var defList = new List<MFDDefaultDefintion>();

            var iterator = this.GetEnumerator();
            while(iterator.MoveNext())
            {
                defList.Add((MFDDefaultDefintion) iterator.Current);
            }

            return defList;
        }

        #region Collection item access

        /// <summary>
        /// Access collection item by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MFDDefaultDefintion this[int index]
        {
            get
            {
                return BaseGet(index) as MFDDefaultDefintion;
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
        public new MFDDefaultDefintion this[string key]
        {
            get { return (MFDDefaultDefintion)BaseGet(key); }
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
            return new MFDDefaultDefintion();
        }

        #endregion Create new element for the collection

        #region Key and item management

        /// <summary>
        /// Each unique entry is Name+FileName
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MFDDefaultDefintion)element).Name + ((MFDDefaultDefintion)element).FileName;
        }

        /// <summary>
        /// Public add needed to provide a method for adding items to the collection
        /// </summary>
        /// <param name="definition"></param>
        public void Add(MFDDefaultDefintion definition)
        {
            BaseAdd(definition);
        }

        /// <summary>
        /// Public remove needed to provide a method for removing items from the collection
        /// </summary>
        /// <param name="defintion"></param>
        public void Remove(MFDDefaultDefintion defintion)
        {
            BaseRemove(defintion.Name + defintion.FileName);
        }

        #endregion Key and item management
    }
}