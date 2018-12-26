using MFDisplay.Interfaces;
using System.Collections.Generic;

namespace MFDisplay.Models
{
    public class ModulesConfiguration : IModulesConfiguration
    {
        public string FilePath { get; set; }
        public string DefaultConfig { get; set; }
        public List<IModuleDefinition> Modules { get; set; }

        public ModulesConfiguration()
        {
            Modules = new List<IModuleDefinition>();
        }
    }
}
