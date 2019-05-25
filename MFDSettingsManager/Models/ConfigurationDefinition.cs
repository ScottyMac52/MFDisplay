using System;
using System.Collections.Generic;
using System.IO;

namespace MFDSettingsManager.Models
{
    /// <summary>
    /// Single Configuration for a Module
    /// </summary>
    public class ConfigurationDefinition : ConfigurationModelBase
    {
        #region Ctor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ConfigurationDefinition()
        {

        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="dc"></param>
        public ConfigurationDefinition(ConfigurationDefinition dc) : base(dc)
        {
            Left = dc.Left;
            Top = dc.Top;
            Width = dc.Width;
            Height = dc.Height;
        }

        #endregion Ctor

        #region Basic Image Properties Left, Top, Width, Height and Opacity
        /// <summary>
        /// Width of the displayed image
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// The Height of the displayed image
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Left coordinate of the displayed image
        /// </summary>
        public int Left { get; set; }
        /// <summary>
        /// Top coordinate of the displayed image
        /// </summary>
        public int Top { get; set; }

        #endregion Basic Image Properties Left, Top, Width, Height and Opacity

        #region SubConfiguration support

        /// <summary>
        /// List of SubConfigurations
        /// </summary>
        public List<SubConfigurationDefinition> SubConfigurations { get; set; }

        #endregion SubConfiguration support

        #region Protected overrides 

        /// <summary>
        /// Returns the portion of the readable string specific to <seealso cref="ConfigurationDefinition"/>
        /// </summary>
        /// <returns></returns>
        protected override string GetReadableString()
        {
            return $"{Width}_{Height}";
        }

        #endregion Protected overrides 

    }
}