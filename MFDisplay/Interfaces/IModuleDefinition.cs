using System.Collections.Generic;

namespace MFDisplay.Interfaces
{
    public interface IModuleDefinition
    {
        List<IMFDDefintion> Configurations { get; }
        string DisplayName { get; }
        string ModuleName { get; }
    }
}