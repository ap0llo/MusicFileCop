using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using MusicFileCop.CL;
using MusicFileCop.Core;
using MusicFileCop.Core.Configuration;
using MusicFileCop.Core.FileSystem;
using MusicFileCop.Core.Metadata;
using MusicFileCop.Core.Output;


namespace MusicFileCop
{
    class MusicFileCop
    {

        readonly IFileSystemLoader m_FileSystemLoader;
        readonly IConfigurationLoader m_ConfigLoader;
        readonly IMetadataLoader m_MetadataLoader;
        readonly IConsistencyChecker m_ConsistencyChecker;
        readonly IConfigurationNode m_DefaultConfiguration;
        readonly IConfigurationWriter m_ConfigWriter;
        readonly ITextOutputWriter m_OutputWriter;

        public MusicFileCop(IFileSystemLoader fileSystemLoader, IConfigurationLoader configurationLoader,
                            IMetadataLoader metadataLoader, IConsistencyChecker consistencyChecker, 
                            IDefaultConfigurationNode defaultConfiguration, IConfigurationWriter configWriter, ITextOutputWriter outputWriter)
        {
            if (fileSystemLoader == null)
            {
                throw new ArgumentNullException(nameof(fileSystemLoader));
            }
            if (configurationLoader == null)
            {
                throw new ArgumentNullException(nameof(configurationLoader));
            }
            if (metadataLoader == null)
            {
                throw new ArgumentNullException(nameof(metadataLoader));
            }
            if (consistencyChecker == null)
            {
                throw new ArgumentNullException(nameof(consistencyChecker));                
            }
            if (defaultConfiguration == null)
            {
                throw new ArgumentNullException(nameof(defaultConfiguration));
            }
            if (configWriter == null)
            {
                throw new ArgumentNullException(nameof(configWriter));
            }
            if (outputWriter == null)
            {
                throw new ArgumentNullException(nameof(outputWriter));
            }

            this.m_FileSystemLoader = fileSystemLoader;
            this.m_ConfigLoader = configurationLoader;
            this.m_MetadataLoader = metadataLoader;
            this.m_ConsistencyChecker = consistencyChecker;
            this.m_DefaultConfiguration = defaultConfiguration;
            m_ConfigWriter = configWriter;
            m_OutputWriter = outputWriter;
        }


        public int Run(string[] args)
        {

            return Parser.Default.ParseArguments<CheckOptions, ExportDefaultConfigOptions>(args).MapResult(
                (CheckOptions options) =>
                    {
                        if (!System.IO.Directory.Exists(options.Path))
                        {
                            Console.Error.Write($"Directory '{options.Path}'not found");
                            return 1;
                        }
                        // load file system
                        var rootDirectory = m_FileSystemLoader.LoadDirectory(options.Path);
                        
                        //load configuration
                        m_ConfigLoader.LoadConfiguration(rootDirectory);

                        //load media metadata
                        m_MetadataLoader.LoadMetadata(rootDirectory);

                        m_ConsistencyChecker.CheckConsistency(rootDirectory);

                        using (var stream = new StreamWriter(System.IO.File.Open(options.OutputFile, FileMode.Create)))
                        {
                            m_OutputWriter.WriteTo(stream);
                        }

                        return 0;
                    },
                (ExportDefaultConfigOptions opts) =>
                {
                    m_ConfigWriter.WriteConfiguration(m_DefaultConfiguration, opts.OutputFile);
                    return 0;
                },
                (IEnumerable<Error> errs) => 1                
            );
            
        }


    }
}
