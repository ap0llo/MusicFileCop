using System;
using Microsoft.Framework.ConfigurationModel;
using MusicFileCop.Core.FileSystem;

namespace MusicFileCop.Core.Configuration
{
    class ConfigurationLoader : IConfigurationLoader
    {
        const string s_DirectoryConfigName = "MusicFileCop.json";
        const string s_FileConfigName = "{0}.MusicFileCop.json";

        readonly IConfigurationMapper m_Mapper;
        readonly IConfigurationNode m_DefaultConfiguration;


        public ConfigurationLoader(IConfigurationMapper mapper, IDefaultConfigurationNode defaultConfiguration)
        {
            if(mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this.m_Mapper = mapper;
            this.m_DefaultConfiguration = defaultConfiguration;
        }



        public void LoadConfiguration(IDirectory directory)
        {
            LoadConfiguration(m_DefaultConfiguration, directory);           
        }


        internal void LoadConfiguration(IConfigurationNode parentNode, IDirectory directory)
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
            m_Mapper.AddMapping(configNode, directory);


            foreach (var file in directory.Files)
            {
                LoadConfiguration(configNode, file);
            }

            foreach (var dir in directory.Directories)
            {
                LoadConfiguration(configNode, dir);
            }
        }

        internal void LoadConfiguration(IConfigurationNode parentNode, IFile file)
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

            m_Mapper.AddMapping(configNode, file);
        }

        internal IConfiguration LoadConfigurationFile(IFile configFile)
        {
            var configuration = new Microsoft.Framework.ConfigurationModel.Configuration();
            configuration.AddJsonFile(configFile.FullPath);

            return configuration;
        }
        
        

     

    }
}
