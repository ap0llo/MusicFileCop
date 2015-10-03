﻿using System;
using System.Collections.Generic;
using System.Linq;
using MusicFileCop.Core.FileSystem;
using MusicFileCop.Core.Metadata;
using MusicFileCop.Core.Output;
using MusicFileCop.Core.Rules;
using MusicFileCop.Model;
using MusicFileCop.Model.Output;
using Ninject;

namespace MusicFileCop.Core
{
    class ConsistencyChecker : IConsistencyChecker, IVisitor
    {
        readonly IKernel m_Kernel;
        readonly IMetadataMapper m_MetadataMapper;
        
        readonly IDictionary<Type, IEnumerable<object>> m_RulesInstanceCache = new Dictionary<Type, IEnumerable<object>>();
        readonly IDictionary<Type, object> m_OutputWriterCache = new Dictionary<Type, object>(); 
        readonly ISet<ICheckable> m_VisitedNodes = new HashSet<ICheckable>();


        public ConsistencyChecker(IMetadataMapper fileMetadataMapper, IKernel kernel)
        {
            if (fileMetadataMapper == null)
            {
                throw new ArgumentNullException(nameof(fileMetadataMapper));                
            }
            if (kernel == null)
            {
                throw new ArgumentNullException(nameof(kernel));                
            }

            m_MetadataMapper = fileMetadataMapper;
            m_Kernel = kernel;
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

            ApplyRules(directory);

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

            ApplyRules(file);

            //TODO: IMetadataMapper should offer something like TryGet()
            try
            {
                var track = m_MetadataMapper.GetTrack(file);
                track.Accept(this);
            }
            catch (KeyNotFoundException)
            {
                //ignore exception (no mapping found)
            }
        }

        public void Visit(IAlbum album)
        {
            if (AlreadyVisited(album))
            {
                return;
            }

            MarkVisited(album);

            ApplyRules(album);

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

            ApplyRules(artist);

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

            ApplyRules(disk);

            disk?.Album.Accept(this);
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

            ApplyRules(track);

            track?.Disk.Accept(this);
            track?.Album.Accept(this);
            track?.AlbumArtist.Accept(this);
            track?.Artist.Accept(this);
        }


        void ApplyRules<T>(T checkable) where T : ICheckable
        {
            var rules = GetRules<T>();

            foreach (var rule in rules.Where(r => r.IsApplicable(checkable)))
            {
                if (!rule.IsConsistent(checkable))
                {
                    GetOutputWriter<T>().WriteViolation(rule, checkable);
                }
            }
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