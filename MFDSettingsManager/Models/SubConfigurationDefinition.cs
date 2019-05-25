using System;
using System.IO;

namespace MFDSettingsManager.Models
{
    /// <summary>
    /// Defines a SubConfiguration
    /// </summary>
    public class SubConfigurationDefinition : ConfigurationModelBase
    {
        #region Ctor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SubConfigurationDefinition()
        {

        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="dc"></param>
        public SubConfigurationDefinition(SubConfigurationDefinition dc) : base(dc)
        {
            StartX = dc.StartX;
            EndX = dc.EndX;
            StartY = dc.StartY;
            EndY = dc.EndY;
        }

        #endregion Ctor

        #region Basic Image Properties StartX, EndX, StartY and EndY

        /// <summary>
        /// The X coordinate inside of the Configuration image relative to (0,0) to start rendering the image
        /// </summary>
        public int StartX { get; set; }
        /// <summary>
        /// The X coordinate inside of the Configuration image relative to (0,0) to end rendering the image
        /// </summary>
        public int EndX { get; set; }
        /// <summary>
        /// The Y coordinate inside of the Configuration image relative to (0,0) to start rendering the image
        /// </summary>
        public int StartY { get; set; }
        /// <summary>
        /// The Y coordinate inside of the Configuration image relative to (0,0) to end rendering the image
        /// </summary>
        public int EndY { get; set; }

        #endregion Basic Image Properties StartX, EndX, StartY and EndY

        #region Protected overrides 

        /// <summary>
        /// Returns the portion of the readable string specific to <seealso cref="SubConfigurationDefinition"/>
        /// </summary>
        /// <returns></returns>

        protected override string GetReadableString()
        {
            return $"{EndX - StartX}_{EndY - StartY}";
        }

        #endregion Protected overrides 
    }
}