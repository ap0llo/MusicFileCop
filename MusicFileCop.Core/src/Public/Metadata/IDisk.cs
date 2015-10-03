using System.Collections.Generic;
using MusicFileCop.Model;

namespace MusicFileCop.Core.Metadata
{
    public interface IDisk : ICheckable
    {
        IAlbum Album { get; }

        int DiskNumber { get; }

        IEnumerable<ITrack> Tracks { get; }
    }
}
