using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicFileCop.Model.FileSystem;

namespace MusicFileCop.Model.Configuration
{
    class ConfigurationLoader : IConfigurationLoader
    {
        const string s_DirectoryConfigName = "MusicFileCop.json";
        const string s_FileConfigName = "{0}.MusicFileCop.json";

        readonly IFileMapper m_FileMapper;
        readonly IConfigurationNode m_DefaultConfiguration;

        public ConfigurationLoader(IFileMapper fileMapper, IConfigurationNode defaultConfiguration)
        {
            if(fileMapper == null)
            {
                throw new ArgumentNullException(nameof(fileMapper));
            }

            this.m_FileMapper = fileMapper;
            this.m_DefaultConfiguration = defaultConfiguration;
        }

        public void LoadConfiguration(IDirectory directory)
        {
            LoadConfiguration(m_DefaultConfiguration, directory);           
        }

        void LoadConfiguration(IConfigurationNode parentNode, IDirectory directory)
        {
            IConfigurationNode configNode;
            if (directory.FileExists(s_DirectoryConfigName))
            {
                configNode = new HierarchicalConfigurationNode(parentNode, LoadConfigurationFile(directory.GetFile(s_DirectoryConfigName)));
            }
            else
            {
                configNode = parentNode;
            }
            m_FileMapper.AddMapping(configNode, directory);


            foreach (var file in directory.Files)
            {
                LoadConfiguration(configNode, file);
            }

            foreach (var dir in directory.Directories)
            {
                LoadConfiguration(configNode, directory);
            }
        }

        void LoadConfiguration(IConfigurationNode parentNode, IFile file)
        {
            var configFileName = String.Format(s_FileConfigName, file.NameWithExtension);

            IConfigurationNode configNode;
            if (file.Directory.FileExists(configFileName))
            {
                configNode = new HierarchicalConfigurationNode(parentNode, LoadConfigurationFile(file.Directory.GetFile(configFileName)));
            }
            else
            {
                configNode = parentNode;
            }

            m_FileMapper.AddMapping(configNode, file);
        }

        IDictionary<string, string> LoadConfigurationFile(IFile configFile)
        {
            throw new NotImplementedException();
        }

    }
}
