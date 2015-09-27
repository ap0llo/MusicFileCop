using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicFileCop.Model.FileSystem;
using MusicFileCop.Model.Metadata;
using MusicFileCop.Model.Rules;

namespace MusicFileCop.Model
{
    class ConsistencyChecker : IConsistencyChecker, IVisitor
    {
        readonly IMapper m_FileMapper;
        readonly IOutputWriter m_OutputWriter;


        public ConsistencyChecker(IMapper fileMapper, IOutputWriter outputWriter)
        {
            if (fileMapper == null)
                throw new ArgumentNullException(nameof(fileMapper));

            if (outputWriter == null)
                throw new ArgumentNullException(nameof(outputWriter));

            this.m_FileMapper = fileMapper;
            this.m_OutputWriter = outputWriter;
        }



        public void CheckConsistency(IDirectory directory)
        {
            directory.Accept(this);
        }


        public void Visit(IDirectory directory)
        {            
            if(AlreadyVisited(directory))
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

            var track = m_FileMapper.GetTrack(file);
            if(track != null)
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

            ApplyRules(album);

            album?.Artist.Accept(this);

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

        private IEnumerable<IRule<T>> GetRules<T>() where T : ICheckable
        {
            throw new NotImplementedException();
        }

        bool AlreadyVisited(object item)
        {
            throw new NotImplementedException();
        }

        void MarkVisited(object item)
        {
            throw new NotImplementedException();
        }
    }

}
