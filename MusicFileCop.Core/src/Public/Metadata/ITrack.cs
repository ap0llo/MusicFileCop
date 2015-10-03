
namespace MusicFileCop.Core.Metadata
{
    public interface ITrack : ICheckable
    {
        IDisk Disk { get; }

        IAlbum Album { get; }

        IArtist AlbumArtist { get; }

        IArtist Artist { get; }

        string Name { get; }

        int TrackNumber { get; }


    }
}
