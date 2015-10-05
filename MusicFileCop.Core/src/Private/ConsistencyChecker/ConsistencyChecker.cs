using System;
using System.Collections.Generic;
using System.Linq;
using MusicFileCop.Core.Configuration;
using MusicFileCop.Core.FileSystem;
using MusicFileCop.Core.Metadata;
using MusicFileCop.Core.Output;
using MusicFileCop.Core.Rules;
using Ninject;

namespace MusicFileCop.Core
{
    [ConfigurationNamespace(ConfigurationNamespace)]
    class ConsistencyChecker : ConsistencyCheckerBase, IConsistencyChecker, IVisitor
    {
        readonly IKernel m_Kernel;
        readonly IMetadataMapper m_MetadataMapper;
        readonly IConfigurationMapper m_ConfigurationMapper;

        readonly IDictionary<Type, IEnumerable<object>> m_RulesInstanceCache = new Dictionary<Type, IEnumerable<object>>();
        readonly IDictionary<Type, object> m_OutputWriterCache = new Dictionary<Type, object>(); 
        readonly ISet<ICheckable> m_VisitedNodes = new HashSet<ICheckable>();


        public ConsistencyChecker(IMetadataMapper fileMetadataMapper, IKernel kernel, IConfigurationMapper configurationMapper)
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

            m_MetadataMapper = fileMetadataMapper;
            m_Kernel = kernel;
            m_ConfigurationMapper = configurationMapper;
        }


        public void CheckConsistency(IDirectory directory)
        {
            m_VisitedNodes.Clear();
            m_RulesInstanceCache.Clear();

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


        void ApplyRules<T>(T checkable, params IConfigurationNode[] configurations) where T : ICheckable
        {
            var rules = GetRules<T>()
                .Where(r => r.IsApplicable(checkable))
                .Where(r => IsRuleEnabled(r, configurations));

            foreach (var rule in rules)
            {
                if (!rule.IsConsistent(checkable))
                {
                    GetOutputWriter<T>().WriteViolation(rule, checkable);
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
            
            
        IEnumerable<IRule<T>> GetRules<T>() where T : ICheckable
        {
            if (!m_RulesInstanceCache.ContainsKey(typeof (T)))
            {
                var rules = m_Kernel.GetAll<IRule<T>>().ToArray();
                m_RulesInstanceCache.Add(typeof(T), rules);
            }

            return m_RulesInstanceCache[typeof(T)].Cast<IRule<T>>();
        }


        bool AlreadyVisited(ICheckable item) => m_VisitedNodes.Contains(item);

        void MarkVisited(ICheckable item) => m_VisitedNodes.Add(item);
    }
}