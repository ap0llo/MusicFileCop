using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicFileCop.Model.FileSystem;
using MusicFileCop.Model.Metadata;
using MusicFileCop.Model.Configuration;

namespace MusicFileCop.Model.Implementation
{
    class FileMapper : IFileMapper
    {
        //TODO: Use "doublesided dictionary"
        readonly IDictionary<IFile, ITrack> m_FileToTrackMapping = new Dictionary<IFile, ITrack>();
        readonly IDictionary<ITrack, IFile> m_TrackToFileMapping = new Dictionary<ITrack, IFile>();
        readonly IDictionary<IDirectory, IConfigurationNode> m_DirectoryToConfigMapping = new Dictionary<IDirectory, IConfigurationNode>();
        readonly IDictionary<IFile, IConfigurationNode> m_FileToConfigMapping = new Dictionary<IFile, IConfigurationNode>();

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

        public void AddMapping(IConfigurationNode configurationNode, IDirectory directory)
        {
            m_DirectoryToConfigMapping.Add(directory, configurationNode);
        }

        public void AddMapping(IConfigurationNode configurationNode, IFile file)
        {
            m_FileToConfigMapping.Add(file, configurationNode);
        }

        public IConfigurationNode GetConfiguration(IDirectory directory) => m_DirectoryToConfigMapping[directory];

        public IConfigurationNode GetConfiguration(IFile file) => m_FileToConfigMapping[file];

    }
}
