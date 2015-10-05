using System.Collections.Generic;
using MusicFileCop.Core.FileSystem;
using MusicFileCop.Core.Metadata;

namespace MusicFileCop.Core
{
    public interface IMetadataMapper
    {
        void AddMapping(ITrack track, IFile file);
        
        ITrack GetTrack(IFile file);

        bool TryGetTrack(IFile file, out ITrack track);

        IFile GetFile(ITrack track);

        bool TryGetFile(ITrack track, out IFile file);

        IEnumerable<IDirectory> GetDirectories(IArtist artist);

        IEnumerable<IDirectory> GetDirectories(IAlbum album);

        IEnumerable<IDirectory> GetDirectories(IDisk disk);
    
    }
}
