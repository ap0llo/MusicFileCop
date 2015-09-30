using MusicFileCop.Model.Configuration;
using MusicFileCop.Model.FileSystem;

namespace MusicFileCop.Model
{
    public interface IConfigurationMapper
    {
        void AddMapping(IConfigurationNode configurationNode, IDirectory directory);

        void AddMapping(IConfigurationNode configurationNode, IFile file);


        IConfigurationNode GetConfiguration(IDirectory directory);

        IConfigurationNode GetConfiguration(IFile file);
    }
}