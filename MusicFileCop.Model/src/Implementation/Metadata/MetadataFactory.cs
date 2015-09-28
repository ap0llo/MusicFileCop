using MusicFileCop.Model.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Model.Metadata
{
    class MetadataFactory : IMetadataFactory
    {
        readonly IDictionary<string, Artist> m_Artists = new Dictionary<string, Artist>(StringComparer.InvariantCultureIgnoreCase);        


        public IArtist GetArtist(string name) => GetArtistInternal(name);

        private Artist GetArtistInternal(string name)
        {
            if(name == null)
            {
                name = String.Empty;
            }

            if(!m_Artists.ContainsKey(name))
            {
                var newArtist = new Artist()
                {
                    Name = name
                };
                m_Artists.Add(name, newArtist);
            }
            return m_Artists[name];
        }

        public IAlbum GetAlbum(string albumArtist, string albumName, int releaseYear) => GetAlbumInternal(albumArtist, albumName, releaseYear);

        private Album GetAlbumInternal(string albumArtist, string albumName, int releaseYear)
        {
            var artist = GetArtistInternal(albumArtist);          

            if(albumName == null)
            {
                albumName = String.Empty;
            }

            if(!artist.AlbumExists(albumName, releaseYear))
            {
                var album = new Album()
                {
                    Artist = artist,
                    Name = albumName,
                    ReleaseYear = releaseYear
                };
                artist.AddAlbum(album);
            }
            return artist.GetAlbum(albumName, releaseYear);
        }

        public IDisk GetDisk(string albumArtist, string albumName, int releaseYear, int diskNumber) => GetDiskInternal(albumArtist, albumArtist, releaseYear, diskNumber);

        public Disk GetDiskInternal(string albumArtist, string albumName, int releaseYear, int diskNumber)
        {
            var album = GetAlbumInternal(albumArtist, albumName, releaseYear);

            if(!album.DiskExists(diskNumber))
            {
                var disk = new Disk()
                {
                    Album = album,
                    DiskNumber = diskNumber
                };
                album.AddDisk(disk);
            }

            return album.GetDisk(diskNumber);
        }

        public ITrack GetTrack(string albumArtist, string albumName, int releaseYear, int diskNumber, int trackNumber, string name, string artist)
        {
            var disk = GetDiskInternal(albumArtist, albumName, releaseYear, diskNumber);            

            var track = new Track()
            {
                Artist = GetArtist(artist),
                Disk = disk,
                Name = name == null ? "" : name,
                TrackNumber = trackNumber
            };
            disk.AddTrack(track);

            return track;
        }
        
    }
}
