using System.Collections.Generic;
using MusicFileCop.Core.FileSystem;
using MusicFileCop.Core.Metadata;

namespace MusicFileCop.Core
{
    public interface IMetadataMapper
    {
        void AddMapping(ITrack track, IFile file);
        
        ITrack GetTrack(IFile file);

        IFile GetFile(ITrack track);

        IEnumerable<IDirectory> GetDirectories(IArtist artist);

        IEnumerable<IDirectory> GetDirectories(IAlbum album);

        IEnumerable<IDirectory> GetDirectories(IDisk disk);
    
    }
}
