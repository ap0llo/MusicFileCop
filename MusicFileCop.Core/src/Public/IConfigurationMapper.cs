using MusicFileCop.Core.Configuration;
using MusicFileCop.Core.FileSystem;

namespace MusicFileCop.Core
{
    /// <summary>
    /// Maper to retrieve configuration for a directory or file
    /// </summary>
    public interface IConfigurationMapper
    {
        /// <summary>
        /// Associates the specified configuration node with the directory
        /// </summary>
        void AddMapping(IConfigurationNode configurationNode, IDirectory directory);

        /// <summary>
        /// Associates the specified configuration node with the file
        /// </summary>
        void AddMapping(IConfigurationNode configurationNode, IFile file);

        /// <summary>
        /// Gets the configuration node associated with the directory
        /// </summary>        
        IConfigurationNode GetConfiguration(IDirectory directory);

        /// <summary>
        /// Gets the configuration node associated with the file
        /// </summary>
        IConfigurationNode GetConfiguration(IFile file);
    }
}