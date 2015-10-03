using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicFileCop.Core;
using MusicFileCop.Core.Configuration;
using MusicFileCop.Core.FileSystem;
using MusicFileCop.Core.Metadata;

namespace MusicFileCop
{
    class MusicFileCop
    {

        readonly IFileSystemLoader m_FileSystemLoader;
        readonly IConfigurationLoader m_ConfigLoader;
        readonly IMetadataLoader m_MetadataLoader;
        readonly IConsistencyChecker m_ConsistencyChecker;
        
        public MusicFileCop(IFileSystemLoader fileSystemLoader, IConfigurationLoader configurationLoader,
                            IMetadataLoader metadataLoader, IConsistencyChecker consistencyChecker)
        {
            if (fileSystemLoader == null)
                throw new ArgumentNullException(nameof(fileSystemLoader));
            if (configurationLoader == null)
                throw new ArgumentNullException(nameof(configurationLoader));
            if (metadataLoader == null)
                throw new ArgumentNullException(nameof(metadataLoader));
            if (consistencyChecker == null)
                throw new ArgumentNullException(nameof(consistencyChecker));

            this.m_FileSystemLoader = fileSystemLoader;
            this.m_ConfigLoader = configurationLoader;
            this.m_MetadataLoader = metadataLoader;
            this.m_ConsistencyChecker = consistencyChecker;
        }


        public void Run(string directory)
        {
            //TODO: check if directory exists

            // load file system
            var rootDirectory = m_FileSystemLoader.LoadDirectory(directory);

            //load configuration
            //TODO: Get default configuration
            m_ConfigLoader.LoadConfiguration(rootDirectory);

            //load media metadata
            m_MetadataLoader.LoadMetadata(rootDirectory);

            m_ConsistencyChecker.CheckConsistency(rootDirectory);

        }


    }
}
