using MusicFileCop.Core.Configuration;
using MusicFileCop.Core.FileSystem;

namespace MusicFileCop.Core
{
    public interface IConfigurationMapper
    {
        void AddMapping(IConfigurationNode configurationNode, IDirectory directory);

        void AddMapping(IConfigurationNode configurationNode, IFile file);


        IConfigurationNode GetConfiguration(IDirectory directory);

        IConfigurationNode GetConfiguration(IFile file);
    }
}