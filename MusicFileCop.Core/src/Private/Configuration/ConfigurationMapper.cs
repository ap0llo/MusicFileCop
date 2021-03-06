﻿using System.Collections.Generic;
using MusicFileCop.Core.FileSystem;

namespace MusicFileCop.Core.Configuration
{
    //TODO: Unit tests
    /// <summary>
    /// Implementation of <see cref="IConfigurationMapper"/>
    /// </summary>
    public class ConfigurationMapper : IConfigurationMapper
    {
        readonly IDictionary<IDirectory, IConfigurationNode> m_DirectoryToConfigMapping = new Dictionary<IDirectory, IConfigurationNode>();
        readonly IDictionary<IFile, IConfigurationNode> m_FileToConfigMapping = new Dictionary<IFile, IConfigurationNode>();

        
        public void AddMapping(IConfigurationNode configurationNode, IDirectory directory) => m_DirectoryToConfigMapping.Add(directory, configurationNode);

        public void AddMapping(IConfigurationNode configurationNode, IFile file) => m_FileToConfigMapping.Add(file, configurationNode);

        public IConfigurationNode GetConfiguration(IDirectory directory) => m_DirectoryToConfigMapping[directory];

        public IConfigurationNode GetConfiguration(IFile file) => m_FileToConfigMapping[file];

    }
}