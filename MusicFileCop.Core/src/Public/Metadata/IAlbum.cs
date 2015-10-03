using System.Collections.Generic;

namespace MusicFileCop.Core.Metadata
{
    public interface IAlbum : ICheckable
    {
        IArtist Artist { get; }

        string Name { get; }

        int ReleaseYear { get; }

        IEnumerable<IDisk> Disks { get; }
        
    }
}
