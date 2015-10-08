using MusicFileCop.Core.FileSystem;

namespace MusicFileCop.Core.Configuration
{
    public interface IConfigurationLoader
    {

        /// <summary>
        /// Recursively loads the configuration for the specified directory
        /// </summary>
        void LoadConfiguration(IDirectory rootDirectory);

    }
}
