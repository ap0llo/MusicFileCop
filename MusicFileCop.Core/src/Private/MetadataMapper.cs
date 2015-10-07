using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Cache;
using MusicFileCop.Core.FileSystem;
using MusicFileCop.Core.Metadata;

namespace MusicFileCop.Core
{
    class MetadataMapper : IMetadataMapper
    {
        //TODO: Use "doublesided dictionary"
        readonly IDictionary<IFile, ITrack> m_FileToTrackMapping = new ConcurrentDictionary<IFile, ITrack>();
        readonly IDictionary<ITrack, IFile> m_TrackToFileMapping = new ConcurrentDictionary<ITrack, IFile>();
        
        readonly IDictionary<IDisk, IEnumerable<IDirectory>> m_DiskDirectoriesCache = new ConcurrentDictionary<IDisk, IEnumerable<IDirectory>>();
        readonly IDictionary<IArtist, IEnumerable<IDirectory>> m_ArtistDirectoriesCache = new ConcurrentDictionary<IArtist, IEnumerable<IDirectory>>();

        public IEnumerable<IDirectory> GetDirectories(IDisk disk) => ExecuteWithCaching(m_DiskDirectoriesCache, disk, d => GetDirectories(d.Tracks).ToArray());


        public bool TryGetFile(ITrack track, out IFile file) => m_TrackToFileMapping.TryGetValue(track, out file);
        
        public IEnumerable<IDirectory> GetDirectories(IArtist artist)
        {
            return ExecuteWithCaching(m_ArtistDirectoriesCache, artist, (IArtist a) =>
            {
                var directories = artist.Albums.SelectMany(GetDirectories).Distinct().ToList();           
                return CombineCommonAncestors(directories).ToList();
            });
        }

        public IEnumerable<IDirectory> GetDirectories(IAlbum album) => album.Disks.SelectMany(GetDirectories);

        public bool TryGetTrack(IFile file, out ITrack track) => m_FileToTrackMapping.TryGetValue(file, out track);
        
        public IFile GetFile(ITrack track) => m_TrackToFileMapping[track];

        public ITrack GetTrack(IFile file) => m_FileToTrackMapping[file];        

        public void AddMapping(ITrack track, IFile file)
        {
            m_FileToTrackMapping.Add(file, track);
            m_TrackToFileMapping.Add(track, file);

            // Invalidte caches
            m_DiskDirectoriesCache.Clear();
            m_ArtistDirectoriesCache.Clear();
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



        TRESULT ExecuteWithCaching<TPARAM, TRESULT>(IDictionary<TPARAM, TRESULT> cache, TPARAM parameter, Func<TPARAM, TRESULT> func)
        {

            if (cache.ContainsKey(parameter))
            {
                return cache[parameter];
            }
            else
            {
                var value = func.Invoke(parameter);
                cache.Add(parameter, value);
                return value;
            }            
        }

    }
}
