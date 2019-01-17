using System.Collections.Generic;
using MFDSettingsManager.Enum;

namespace MFDSettingsManager.Models
{
    /// <summary>
    /// Module Definition
    /// </summary>
    public class ModuleDefinition
    {
        /// <summary>
        /// Parent to this Module
        /// </summary>
        public ModulesConfiguration Parent { get; internal set; }
        /// <summary>
        /// Name of the Module
        /// </summary>
        public string ModuleName { get; internal set; }
        /// <summary>
        /// Display Name for the Module
        /// </summary>
        public string DisplayName { get; internal set; }
        /// <summary>
        /// Filename to use for the entire module
        /// </summary>
        public string FileName { get; internal set; }
        /// <summary>
        /// The list of Configurations for this Module
        /// </summary>
        public List<ConfigurationDefinition> Configurations { get; internal set; }
        /// <summary>
        /// ImageType to use for saving
        /// </summary>
        public SavedImageType? ImageType => Parent?.ImageType ?? SavedImageType.Jpeg;
        /// <summary>
        /// Ctor
        /// </summary>        
        public ModuleDefinition()
        {
            Configurations = new List<ConfigurationDefinition>();
        }
            
    }
}
