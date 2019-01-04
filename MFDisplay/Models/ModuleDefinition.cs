using System.Collections.Generic;

namespace MFDisplay.Models
{
    public class ModuleDefinition
    {
        public string ModuleName { get; internal set; }
        public string DisplayName { get; internal set; }

        public List<ConfigurationDefinition> Configurations { get; internal set; }

        public ModuleDefinition()
        {
            Configurations = new List<ConfigurationDefinition>();
        }
            
    }
}
