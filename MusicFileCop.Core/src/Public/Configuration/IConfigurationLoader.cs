using MusicFileCop.Core.FileSystem;

namespace MusicFileCop.Core.Configuration
{
    public interface IConfigurationLoader
    {

        void LoadConfiguration(IDirectory rootDirectory);

    }
}
