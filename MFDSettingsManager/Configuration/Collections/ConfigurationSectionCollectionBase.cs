using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace MFDSettingsManager.Configuration.Collections
{
    /// <summary>
    /// Base class for all ConfigurationSectionCollection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConfigurationSectionCollectionBase<T> : ConfigurationElementCollection where T : ConfigurationElement, new()
    {
        #region Properties

        /// <summary>
        /// Logger to use
        /// </summary>
        protected ILog Logger => LogManager.GetLogger("MFD4CTS");

        #endregion Properties

        #region Overrides to permit error logging

        /// <summary>
        /// Error deserializing an attribute
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            Logger?.Error($"Unrecognized attribute named: {name} with value: {value}");
            return base.OnDeserializeUnrecognizedAttribute(name, value);
        }

        /// <summary>
        /// Logs an unrecognized element
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
        {
            Logger?.Warn($"Unrecognized element named: {elementName} location: {ElementInformation.Source} line number: {ElementInformation.LineNumber}");
            return base.OnDeserializeUnrecognizedElement(elementName, reader);
        }

        /// <summary>
        /// Logs a required property missing
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override object OnRequiredPropertyNotFound(string name)
        {
            Logger?.Error($"Required property missing named: {name}.");
            return base.OnRequiredPropertyNotFound(name);
        }

        #endregion Overrides to permit error logging

        #region Collection item access

        /// <summary>
        /// Exposes the Collection as a List
        /// </summary>
        public List<T> List => GetAsList();

        /// <summary>
        /// Access collection item by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                return BaseGet(index) as T;
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
        public new T this[string key]
        {
            get { return (T)BaseGet(key); }
            set
            {
                if (BaseGet(key) != null)
                {
                    BaseRemoveAt(BaseIndexOf(BaseGet(key)));
                }
                BaseAdd(value);
            }
        }

        /// <summary>
        /// Public add needed to provide a method for adding items to the collection
        /// </summary>
        /// <param name="definition"></param>
        public void Add(T definition)
        {
            BaseAdd(definition);
        }

        /// <summary>
        /// Public remove method to provide a method for removing items from the collection
        /// </summary>
        /// <param name="definition"></param>
        public void Remove(T definition)
        {
            BaseRemove(GetCompositeKey);
        }

        #endregion Collection item access

        #region Create new element for the collection

        /// <summary>
        /// Creates a new element for the collection
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new T();
        }

        #endregion Create new element for the collection

        #region Key and item management

        /// <summary>
        /// Function to define how we get a key
        /// </summary>
        public Func<T, object> GetCompositeKey => GetDefaultKey();

        /// <summary>
        /// Default implementation returns empty string
        /// </summary>
        /// <returns></returns>
        protected virtual Func<T, object> GetDefaultKey()
        {
            return (T) => "";
        }

        /// <summary>
        /// Each unique entry is Name+FileName
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return GetCompositeKey?.Invoke((T)element) ?? "";
        }

        #endregion Key and item management

        #region Private helpers

        private List<T> GetAsList()
        {
            var defList = new List<T>();

            var iterator = this.GetEnumerator();
            while (iterator.MoveNext())
            {
                defList.Add((T)iterator.Current);
            }

            return defList;
        }
        

        #endregion Private helpers    
    }
}
    
