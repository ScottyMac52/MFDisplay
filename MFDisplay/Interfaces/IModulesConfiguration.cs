using System.Collections.Generic;

namespace MFDisplay.Interfaces
{
    /// <summary>
    /// Interface
    /// </summary>
    public interface IModulesConfiguration
    {
        string FilePath { get; set; }
        string DefaultConfig { get; set; }
        List<IModuleDefinition> Modules { get; }
    }
}