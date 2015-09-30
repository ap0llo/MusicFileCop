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
    public class PrefixConfigurationMapper : IConfigurationMapper
    {
        readonly IConfigurationMapper m_InnerMapper;
        readonly string m_SettingsPrefix;

        public PrefixConfigurationMapper(string settingsPrefix, IConfigurationMapper innerMapper)
        {
            if (settingsPrefix == null)
            {
                throw new ArgumentNullException(nameof(settingsPrefix));                
            }

            if (innerMapper == null)
            {
                throw new ArgumentNullException(nameof(innerMapper));                
            }

            this.m_SettingsPrefix = settingsPrefix;
            this.m_InnerMapper = innerMapper;
        }


        public void AddMapping(IConfigurationNode configurationNode, IDirectory directory) => m_InnerMapper.AddMapping(configurationNode, directory);

        public void AddMapping(IConfigurationNode configurationNode, IFile file) => m_InnerMapper.AddMapping(configurationNode, file);

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
