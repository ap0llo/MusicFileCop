using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicFileCop.Model.Configuration;
using MusicFileCop.Model.FileSystem;
using MusicFileCop.Model.Metadata;

namespace MusicFileCop.Model
{
    //TODO: Class should not be public
    public class PrefixConfigurationMapper : IMapper
    {
        readonly IMapper m_InnerMapper;
        readonly string m_SettingsPrefix;

        public PrefixConfigurationMapper(string settingsPrefix, IMapper innerMapper)
        {
            if (settingsPrefix == null)
                throw new ArgumentNullException(nameof(settingsPrefix));

            if (innerMapper == null)
                throw new ArgumentNullException(nameof(innerMapper));


            this.m_SettingsPrefix = settingsPrefix;
            this.m_InnerMapper = innerMapper;
        }


        public void AddMapping(ITrack track, IFile file) => m_InnerMapper.AddMapping(track, file);


        public void AddMapping(IConfigurationNode configurationNode, IDirectory directory) => m_InnerMapper.AddMapping(configurationNode, directory);

        public void AddMapping(IConfigurationNode configurationNode, IFile file) => m_InnerMapper.AddMapping(configurationNode, file);

        public ITrack GetTrack(IFile file) => m_InnerMapper.GetTrack(file);


        public IFile GetFile(ITrack track) => m_InnerMapper.GetFile(track);

        public IEnumerable<IDirectory> GetDirectories(IAlbum album) => m_InnerMapper.GetDirectories(album);

        public IEnumerable<IDirectory> GetDirectories(IDisk disk) => m_InnerMapper.GetDirectories(disk);
        
        public IConfigurationNode GetConfiguration(IDirectory directory)
        {
            //TODO: cache configuration node objects
            var actualConfiguration = m_InnerMapper.GetConfiguration(directory);
            return new PrefixConfigurationNode(actualConfiguration, this.m_SettingsPrefix);
        }

        public IConfigurationNode GetConfiguration(IFile file)
        {
            //TODO: cache configuration node objects
            var actualConfiguration = m_InnerMapper.GetConfiguration(file);
            return new PrefixConfigurationNode(actualConfiguration, this.m_SettingsPrefix);
        }
    }
}
