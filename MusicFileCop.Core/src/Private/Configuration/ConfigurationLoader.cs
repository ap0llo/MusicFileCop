using System;
using Microsoft.Framework.ConfigurationModel;
using MusicFileCop.Core.FileSystem;
using NLog;

namespace MusicFileCop.Core.Configuration
{
    class ConfigurationLoader : IConfigurationLoader
    {
        const string s_DirectoryConfigName = "MusicFileCop.json";
        const string s_FileConfigName = "{0}.MusicFileCop.json";

        readonly ILogger m_Logger = LogManager.GetCurrentClassLogger();
        readonly IConfigurationMapper m_Mapper;
        readonly IConfigurationNode m_DefaultConfiguration;


        public ConfigurationLoader(IConfigurationMapper mapper, IDefaultConfigurationNode defaultConfiguration)
        {
            if(mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            m_Mapper = mapper;
            m_DefaultConfiguration = defaultConfiguration;
        }



        public void LoadConfiguration(IDirectory directory)
        {
            LoadConfiguration(m_DefaultConfiguration, directory);           
        }

        /// <summary>
        /// Loads configuration for the specified directory and its subdirectories and file using the specified configuration node as parent node
        /// </summary>
        internal void LoadConfiguration(IConfigurationNode parentNode, IDirectory directory)
        {
            m_Logger.Info($"Loading configuration for directory '{directory.FullPath}'");

            // determine configuration node for directory
            IConfigurationNode configNode;

            // config file present in directory => create new node
            if (directory.FileExists(s_DirectoryConfigName))
            {
                configNode = new HierarchicalConfigurationNode(parentNode, LoadConfigurationFile(directory.GetFile(s_DirectoryConfigName)));
            }
            // no config file found => use parent node
            else
            {
                configNode = parentNode;
            }

            m_Mapper.AddMapping(configNode, directory);

            // load configuration for files in the directory
            foreach (var file in directory.Files)
            {
                LoadConfiguration(configNode, file);
            }

            // load configuration for subdirectories
            foreach (var dir in directory.Directories)
            {
                LoadConfiguration(configNode, dir);
            }
        }

        /// <summary>
        /// Loads configuration for the specified file using the specified configuration node as parent node
        /// </summary>
        internal void LoadConfiguration(IConfigurationNode parentNode, IFile file)
        {
            m_Logger.Info($"Loading configuration for file '{file.FullPath}'");

            // determine the configuration node to associate with the file
            IConfigurationNode configNode;
            var configFileName = String.Format(s_FileConfigName, file.NameWithExtension);

            // there is a file-specific configuration file => create new config node
            if (file.Directory.FileExists(configFileName))
            {
                configNode = new HierarchicalConfigurationNode(parentNode, LoadConfigurationFile(file.Directory.GetFile(configFileName)));
            }
            // no config file found => use parent node
            else
            {
                configNode = parentNode;
            }

            // associate configuration node with file
            m_Mapper.AddMapping(configNode, file);
        }

        /// <summary>
        /// Loads a json configuration file
        /// </summary>
        internal IConfiguration LoadConfigurationFile(IFile configFile)
        {
            var configuration = new Microsoft.Framework.ConfigurationModel.Configuration();
            configuration.AddJsonFile(configFile.FullPath);

            return configuration;
        }
        
        

     

    }
}
