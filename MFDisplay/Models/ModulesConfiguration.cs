using MFDisplay.Interfaces;
using System.Collections.Generic;

namespace MFDisplay.Models
{
    public class ModulesConfiguration : IModulesConfiguration
    {
        public string FilePath { get; internal set; }
        public string DefaultConfig { get; internal set; }
        public List<IModuleDefinition> Modules { get; internal set; }
        public bool? SaveClips { get; internal set; }

        public ModulesConfiguration()
        {
            Modules = new List<IModuleDefinition>();
        }
    }
}
