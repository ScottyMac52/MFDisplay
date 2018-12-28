using MFDisplay.Interfaces;
using System.Collections.Generic;

namespace MFDisplay.Models
{
    public class ModuleDefinition : IModuleDefinition
    {
        public string ModuleName { get; internal set; }
        public string DisplayName { get; internal set; }

        public List<IMFDDefintion> Configurations { get; internal set; }

        public ModuleDefinition()
        {
            Configurations = new List<IMFDDefintion>();
        }
            
    }
}
