using System.Collections.Generic;

namespace MFDisplay.Models
{
    /// <summary>
    /// Implementation of the ModulesConfiguration interface
    /// </summary>
    public class ModulesConfiguration
    {
        /// <summary>
        /// The path to the graphic files from the CTS utility
        /// </summary>
        public string FilePath { get; internal set;  }
        /// <summary>
        /// Default configuration to load on startup
        /// </summary>
        public string DefaultConfig { get; internal set; }
        /// <summary>
        /// If True then the cropped images are saved
        /// </summary>
        public bool? SaveClips { get; internal set; }
        /// <summary>
        /// List of modules available
        /// </summary>
        public List<ModuleDefinition> Modules { get; internal set; }
        /// <summary>
        /// Ctor
        /// </summary>
        public ModulesConfiguration()
        {
            Modules = new List<ModuleDefinition>();
        }
    }
}
