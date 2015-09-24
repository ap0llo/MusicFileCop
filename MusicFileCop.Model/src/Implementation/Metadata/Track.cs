using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Model.Metadata
{
    class Track : ITrack
    {
        public IAlbum Album => Disk.Album;

        public IArtist AlbumArtist => Album.Artist;

        public IArtist Artist { get; internal set; }

        public IDisk Disk { get; internal set; }        

        public string Name { get; internal set; }

        public int TrackNumber { get; internal set; }        
    }
}
