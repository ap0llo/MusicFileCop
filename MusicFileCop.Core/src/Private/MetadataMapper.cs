using System.Collections.Generic;
using System.Linq;
using MusicFileCop.Core.FileSystem;
using MusicFileCop.Core.Metadata;

namespace MusicFileCop.Core
{
    class MetadataMapper : IMetadataMapper
    {
        //TODO: Use "doublesided dictionary"
        readonly IDictionary<IFile, ITrack> m_FileToTrackMapping = new Dictionary<IFile, ITrack>();
        readonly IDictionary<ITrack, IFile> m_TrackToFileMapping = new Dictionary<ITrack, IFile>();
        
        //TODO: Some kind of caching mechanism would be great
        public IEnumerable<IDirectory> GetDirectories(IDisk disk) => GetDirectories(disk.Tracks);

        //TODO: Some kind of caching mechanism would be great
        public IEnumerable<IDirectory> GetDirectories(IArtist artist)
        {
            var directories = artist.Albums.SelectMany(GetDirectories).Distinct().ToList();           
            return CombineCommonAncestors(directories);
        }

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

        internal IEnumerable<IDirectory> CombineCommonAncestors(IEnumerable<IDirectory> directories)
        {            
            var result = new List<IDirectory>(directories);            
            var combinedAny = false;

            foreach (var group in result.Where(x=> x.ParentDirectory != null).GroupBy(dir => dir.ParentDirectory).ToList())
            {
                var parent = group.Key;
                if (parent.Directories.All(x => result.Contains(x)))
                {
                    combinedAny = true;
                    result.Add(parent);
                    foreach (var directory in group)
                    {
                        result.Remove(directory);
                    }
                }
            }

            if (combinedAny)
            {
                return CombineCommonAncestors(result);
            }
            else
            {
                return result;                
            }

        } 

    }
}
