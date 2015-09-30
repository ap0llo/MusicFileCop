using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicFileCop.Model.FileSystem;
using MusicFileCop.Model.Metadata;
using MusicFileCop.Model.Configuration;

namespace MusicFileCop.Model
{
    class MetadataMapper : IMetadataMapper
    {
        //TODO: Use "doublesided dictionary"
        readonly IDictionary<IFile, ITrack> m_FileToTrackMapping = new Dictionary<IFile, ITrack>();
        readonly IDictionary<ITrack, IFile> m_TrackToFileMapping = new Dictionary<ITrack, IFile>();
        
        //TODO: Some kind of caching mechanism would be great
        public IEnumerable<IDirectory> GetDirectories(IDisk disk) => GetDirectories(disk.Tracks);

        //TODO: Some kind of caching mechanism would be great
        public IEnumerable<IDirectory> GetDirectories(IAlbum album) => album.Disks.SelectMany(GetDirectories);

        public IFile GetFile(ITrack track) => m_TrackToFileMapping[track];

        public ITrack GetTrack(IFile file) => m_FileToTrackMapping[file];        


        public void AddMapping(ITrack track, IFile file)
        {
            m_FileToTrackMapping.Add(file, track);
            m_TrackToFileMapping.Add(track, file);
        }


        IEnumerable<IDirectory> GetDirectories(IEnumerable<ITrack> tracks)
        {
            return tracks.Select(GetFile).Select(t => t.Directory).Distinct();
        }


    }
}
