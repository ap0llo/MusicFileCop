using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicFileCop.Core.FileSystem;
using NLog;
using TagLib;
using TagLib.Mpeg;

namespace MusicFileCop.Core.Metadata
{
    class MetaDataLoader : IMetadataLoader
    {
        static readonly ISet<string> s_MusicFileExtensions = new HashSet<string>(new[] { ".mp3" }, StringComparer.InvariantCultureIgnoreCase);

        readonly ILogger m_Logger = LogManager.GetCurrentClassLogger();

        readonly IMetadataFactory m_MetadataFactory;
        readonly IMetadataMapper m_FileMetadataMapper;


        public MetaDataLoader(IMetadataFactory metadataFactory, IMetadataMapper fileMetadataMapper)
        {
            if(metadataFactory == null)
                throw new ArgumentNullException(nameof(metadataFactory));
            if (fileMetadataMapper == null)
                throw new ArgumentNullException(nameof(fileMetadataMapper));

            m_MetadataFactory = metadataFactory;
            m_FileMetadataMapper = fileMetadataMapper;
        }


        public void LoadMetadata(IDirectory directory)
        {
            var mediaFiles = directory.Files.Where(file => s_MusicFileExtensions.Contains(file.Extension));

            var fileTask = Task.Factory.StartNew(() => mediaFiles.AsParallel().WithDegreeOfParallelism(20).ForAll(LoadMetadata));
            var directoryTask = Task.Factory.StartNew(() => directory.Directories.AsParallel().WithDegreeOfParallelism(20).ForAll(LoadMetadata));
        
            fileTask.Wait();
            directoryTask.Wait();

        }


        void LoadMetadata(IFile file)
        {

            m_Logger.Info($"Loading metadata for file '{file.FullPath}'");

            using (var audioFile = new AudioFile(file.FullPath))
            {
                var tag = audioFile.GetTag(TagTypes.Id3v2);

                var track = m_MetadataFactory.GetTrack(
                    tag.AlbumArtists != null && tag.AlbumArtists.Any() ? tag.AlbumArtists.Aggregate((a, b) => $"{a}/{b}") : "",
                    tag.Album,
                    (int) tag.Year,
                    (int) tag.Disc,
                    (int) tag.Track,
                    tag.Title,
                    tag.Performers != null && tag.Performers.Any() ? tag.Performers.Aggregate((a, b) => $"{a}/{b}") : "");

                m_FileMetadataMapper.AddMapping(track, file);
            }
        }
    }
}
