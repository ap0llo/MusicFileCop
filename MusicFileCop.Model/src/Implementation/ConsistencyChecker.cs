using System;
using System.Collections.Generic;
using System.Linq;
using MusicFileCop.Model.FileSystem;
using MusicFileCop.Model.Metadata;
using MusicFileCop.Model.Output;
using MusicFileCop.Model.Rules;
using Ninject;

namespace MusicFileCop.Model
{
    class ConsistencyChecker : IConsistencyChecker, IVisitor
    {
        readonly IKernel m_Kernel;
        readonly IMapper m_Mapper;
        readonly IOutputWriter m_OutputWriter;

        readonly IDictionary<Type, IEnumerable<object>> m_RulesInstanceCache = new Dictionary<Type, IEnumerable<object>>();    
        readonly ISet<ICheckable> m_VisitedNodes = new HashSet<ICheckable>();


        public ConsistencyChecker(IMapper fileMapper, IOutputWriter outputWriter, IKernel kernel)
        {
            if (fileMapper == null)
                throw new ArgumentNullException(nameof(fileMapper));

            if (outputWriter == null)
                throw new ArgumentNullException(nameof(outputWriter));

            if (kernel == null)
                throw new ArgumentNullException(nameof(kernel));

            m_Mapper = fileMapper;
            m_OutputWriter = outputWriter;
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

            //TODO: IMapper should offer something like TryGet()
            try
            {
                var track = m_Mapper.GetTrack(file);
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
                    m_OutputWriter.WriteViolation(rule, checkable);
                }
            }
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