using System;
using System.Collections.Generic;
using System.Linq;
using MusicFileCop.Core.FileSystem;
using TagLib;
using TagLib.Mpeg;

namespace MusicFileCop.Core.Metadata
{
    class MetaDataLoader : IMetadataLoader
    {
        static readonly ISet<string> s_MusicFileExtensions = new HashSet<string>(new[] { ".mp3" }, StringComparer.InvariantCultureIgnoreCase);
        readonly IMetadataFactory m_MetadataFactory;
        readonly IMetadataMapper m_FileMetadataMapper;

        public MetaDataLoader(IMetadataFactory metadataFactory, IMetadataMapper fileMetadataMapper)
        {
            if(metadataFactory == null)
                throw new ArgumentNullException(nameof(metadataFactory));
            if (fileMetadataMapper == null)
                throw new ArgumentNullException(nameof(fileMetadataMapper));

            this.m_MetadataFactory = metadataFactory;
            this.m_FileMetadataMapper = fileMetadataMapper;
        }

        public void LoadMetadata(IDirectory directory)
        {

            foreach (var file in directory.Files)
            {
                if(s_MusicFileExtensions.Contains(file.Extension))
                {
                    LoadMetadata(file);
                }
            }

            foreach (var dir in directory.Directories)
            {
                LoadMetadata(dir);
            }            
        }


        void LoadMetadata(IFile file)
        {
            using (var audioFile = new AudioFile(file.FullPath))
            {
                var tag = audioFile.GetTag(TagTypes.Id3v2);

                var track = m_MetadataFactory.GetTrack(
                    tag.AlbumArtists?.FirstOrDefault(),
                    tag.Album,
                    (int)tag.Year,
                    (int)tag.Disc,
                    (int)tag.Track,
                    tag.Title,
                    tag.Performers?.FirstOrDefault());

                m_FileMetadataMapper.AddMapping(track, file);
            }
        }
    }
}
