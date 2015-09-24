using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Model.Metadata
{
    public interface ITrack
    {
        IDisk Disk { get; }

        IAlbum Album { get; }

        IArtist AlbumArtist { get; }

        IArtist Artist { get; }

        string Name { get; }

        int TrackNumber { get; }


    }
}
