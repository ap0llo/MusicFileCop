using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicFileCop.Model.FileSystem;
using TagLib.Mpeg;
using TagLib;

namespace MusicFileCop.Model.Metadata
{
    class MetaDataLoader : IMetadataLoader
    {
        static readonly ISet<string> s_MusicFileExtensions = new HashSet<string>(new[] { ".mp3" }, StringComparer.InvariantCultureIgnoreCase);
        readonly IMetadataFactory m_MetadataFactory;


        public MetaDataLoader(IMetadataFactory metadataFactory)
        {
            if(metadataFactory == null)
            {
                throw new ArgumentNullException(nameof(metadataFactory));
            }

            this.m_MetadataFactory = metadataFactory;
        }

        public void LoadMetadata(IFileMapper fileMapper, IDirectory directory)
        {

            foreach (var file in directory.Files)
            {
                if(s_MusicFileExtensions.Contains(file.Extension))
                {
                }
            }

            foreach (var dir in directory.Directories)
            {
                LoadMetadata(fileMapper, dir);
            }            
        }

        private void LoadMetadata(IFileMapper fileMapper, IFile file)
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

                fileMapper.AddMapping(track, file);
            }
        }
    }
}
