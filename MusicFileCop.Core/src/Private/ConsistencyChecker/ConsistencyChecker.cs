using System;
using System.Collections.Generic;
using System.Linq;
using MusicFileCop.Core.Configuration;
using MusicFileCop.Core.FileSystem;
using MusicFileCop.Core.Metadata;
using MusicFileCop.Core.Output;
using MusicFileCop.Core.Rules;
using Ninject;
using NLog;

namespace MusicFileCop.Core
{
    [ConfigurationNamespace(s_ConfigurationNamespace)]
    class ConsistencyChecker : ConsistencyCheckerBase, IConsistencyChecker, IVisitor
    {
        readonly ILogger m_Logger = LogManager.GetCurrentClassLogger();
        readonly IKernel m_Kernel;
        readonly IMetadataMapper m_MetadataMapper;
        readonly IConfigurationMapper m_ConfigurationMapper;
        readonly IRuleSet m_RuleSet;
       
        readonly IDictionary<Type, object> m_OutputWriterCache = new Dictionary<Type, object>(); 
        readonly ISet<ICheckable> m_VisitedNodes = new HashSet<ICheckable>();


        public ConsistencyChecker(IMetadataMapper fileMetadataMapper, IKernel kernel, IConfigurationMapper configurationMapper, IRuleSet ruleSet)
        {
            if (fileMetadataMapper == null)
            {
                throw new ArgumentNullException(nameof(fileMetadataMapper));                
            }
            if (kernel == null)
            {
                throw new ArgumentNullException(nameof(kernel));                
            }
            if (configurationMapper == null)
            {
                throw new ArgumentNullException(nameof(configurationMapper));
            }
            if (ruleSet == null)
            {
                throw new ArgumentNullException(nameof(ruleSet));
            }

            m_MetadataMapper = fileMetadataMapper;
            m_Kernel = kernel;
            m_ConfigurationMapper = configurationMapper;
            m_RuleSet = ruleSet;
        }


        /// <summary>
        /// Recursively checks the consistency of the specified directory
        /// </summary>        
        public void CheckConsistency(IDirectory directory)
        {
            m_Logger.Info($"Starting consistency check, root directory {directory.FullPath}");

            m_VisitedNodes.Clear();

            directory.Accept(this);
        }

        public void Visit(IDirectory directory)
        {
            if (AlreadyVisited(directory))
            {
                return;
            }

            MarkVisited(directory);

            ApplyRules(directory, m_ConfigurationMapper.GetConfiguration(directory));

            foreach (var file in directory.Files)
            {
                file.Accept(this);
            }
            foreach (var dir in directory.Directories)
            {
                dir.Accept(this);
            }
        }

        public void Visit(IFile file)
        {
            if (AlreadyVisited(file))
            {
                return;
            }

            MarkVisited(file);

            ApplyRules(file, m_ConfigurationMapper.GetConfiguration(file));


            ITrack track;
            if (m_MetadataMapper.TryGetTrack(file, out track))
            {
                track.Accept(this);
            }            
        }

        public void Visit(IAlbum album)
        {
            if (AlreadyVisited(album))
            {
                return;
            }

            MarkVisited(album);


            var configurationNodes = m_MetadataMapper.GetDirectories(album).Select(m_ConfigurationMapper.GetConfiguration);
            ApplyRules(album, configurationNodes.ToArray());

            album.Artist.Accept(this);

            foreach (var disk in album.Disks)
            {
                disk.Accept(this);
            }
        }

        public void Visit(IArtist artist)
        {
            if (AlreadyVisited(artist))
            {
                return;
            }

            MarkVisited(artist);

            var configurationNodes = m_MetadataMapper.GetDirectories(artist).Select(m_ConfigurationMapper.GetConfiguration);

            ApplyRules(artist, configurationNodes.ToArray());

            foreach (var album in artist.Albums)
            {
                album.Accept(this);
            }
        }

        public void Visit(IDisk disk)
        {
            if (AlreadyVisited(disk))
            {
                return;
            }

            MarkVisited(disk);

            var configurationNodes = m_MetadataMapper.GetDirectories(disk).Select(m_ConfigurationMapper.GetConfiguration);

            ApplyRules(disk, configurationNodes.ToArray());

            disk.Album?.Accept(this);
            foreach (var track in disk.Tracks)
            {
                track.Accept(this);
            }
        }

        public void Visit(ITrack track)
        {
            if (AlreadyVisited(track))
            {
                return;
            }

            MarkVisited(track);

            ApplyRules(track, m_ConfigurationMapper.GetConfiguration(m_MetadataMapper.GetFile(track)));

            track?.Disk.Accept(this);
            track?.Album.Accept(this);
            track?.AlbumArtist.Accept(this);
            track?.Artist.Accept(this);
        }

        /// <summary>
        /// Applies all rules to the specified item
        /// </summary>
        void ApplyRules<T>(T checkable, params IConfigurationNode[] configurations) where T : ICheckable
        {
            // get all rules for the type, filter out rules that are disabled or not applicate
            var rules = m_RuleSet.GetRules<T>()
                .Where(r => IsRuleEnabled(r, configurations))
                .Where(r => r.IsApplicable(checkable));

            foreach (var rule in rules)
            {
                // if a violation was found, write a violatioion to the output writer
                if (!rule.IsConsistent(checkable))
                {
                    var severity = configurations.Select(c => c.GetValue<Severity>(GetRuleSeveritySettingsName(rule))).Max();
                    GetOutputWriter<T>().WriteViolation(rule, severity, checkable);
                }                
            }
        }


        bool IsRuleEnabled(IRule rule, IEnumerable<IConfigurationNode> configurationNodes)
        {
            return configurationNodes.Any(c => c.GetValue<bool>(GetRuleEnableSettingsName(rule)));
        }

        IOutputWriter<T> GetOutputWriter<T>() where T : ICheckable
        {
            if (!m_OutputWriterCache.ContainsKey(typeof (T)))
            {
                var writer = m_Kernel.Get<IOutputWriter<T>>();
                m_OutputWriterCache.Add(typeof(T), writer);
            }

            return (IOutputWriter <T>) m_OutputWriterCache[typeof (T)];
        }
                           
        bool AlreadyVisited(ICheckable item) => m_VisitedNodes.Contains(item);

        void MarkVisited(ICheckable item) => m_VisitedNodes.Add(item);
    }
}