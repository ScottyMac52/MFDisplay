using MFDisplay.Interfaces;
using System.Collections.Generic;

namespace MFDisplay.Models
{
    public class ModuleDefinition : IModuleDefinition
    {
        public string ModuleName { get; set; }
        public string DisplayName { get; set; }

        public List<IMFDDefintion> Configurations { get; set; }

        public ModuleDefinition()
        {
            Configurations = new List<IMFDDefintion>();
        }
            
    }
}
