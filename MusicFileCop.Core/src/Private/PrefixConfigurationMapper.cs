using System;
using MusicFileCop.Core.Configuration;
using MusicFileCop.Core.FileSystem;

namespace MusicFileCop.Core
{
    /// <summary>
    /// Configuration mapper that maps wraps all configuraiton nodes on a <see cref="PrefixConfigurationNode"/> before returning it
    /// </summary>
    class PrefixConfigurationMapper : IConfigurationMapper
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
          
            // get configuraiton node from the actual mapper
            var actualConfiguration = m_InnerMapper.GetConfiguration(directory);

            // wrap node into PrefixConfigurationNode
            return new PrefixConfigurationNode(actualConfiguration, this.m_SettingsPrefix);
        }

        public IConfigurationNode GetConfiguration(IFile file)
        {
            //TODO: cache configuration node objects
            
            // get configuraiton node from the actual mapper
            var actualConfiguration = m_InnerMapper.GetConfiguration(file);

            // wrap node into PrefixConfigurationNode
            return new PrefixConfigurationNode(actualConfiguration, this.m_SettingsPrefix);
        }
    }
}
