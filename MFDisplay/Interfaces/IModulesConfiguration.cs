using System.Collections.Generic;

namespace MFDisplay.Interfaces
{
    /// <summary>
    /// Interface
    /// </summary>
    public interface IModulesConfiguration
    {
        string FilePath { get; }
        string DefaultConfig { get; }
        List<IModuleDefinition> Modules { get; }
        bool? SaveClips { get; }
    }
}