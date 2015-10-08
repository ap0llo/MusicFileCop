using System;
using System.Collections.Generic;

namespace MusicFileCop.Core.Metadata
{
    /// <summary>
    ///     Implementation of <see cref="IMetadataFactory" /> Thar reuses instances of IAlbum, IDisk and IArtist to create a
    ///     usable graph of metadata
    /// </summary>
    internal class MetadataFactory : IMetadataFactory
    {
        readonly IDictionary<string, Artist> m_Artists = new Dictionary<string, Artist>(StringComparer.InvariantCultureIgnoreCase);

        readonly object m_Lock = new object();


        public IArtist GetArtist(string name) => GetArtistInternal(name);

        public IAlbum GetAlbum(string albumArtist, string albumName, int releaseYear) => GetAlbumInternal(albumArtist, albumName, releaseYear);

        public IDisk GetDisk(string albumArtist, string albumName, int releaseYear, int diskNumber) => GetDiskInternal(albumArtist, albumArtist, releaseYear, diskNumber);

        public ITrack GetTrack(string albumArtist, string albumName, int releaseYear, int diskNumber, int trackNumber, string name, string artist)
        {
            var disk = GetDiskInternal(albumArtist, albumName, releaseYear, diskNumber);

            var track = new Track
            {
                Artist = GetArtist(artist),
                Disk = disk,
                Name = name ?? "",
                TrackNumber = trackNumber
            };
            lock (disk)
            {
                disk.AddTrack(track);
            }

            return track;
        }


        Artist GetArtistInternal(string name)
        {
            if (name == null)
            {
                name = string.Empty;
            }

            lock (m_Lock)
            {
                if (!m_Artists.ContainsKey(name))
                {
                    var newArtist = new Artist
                    {
                        Name = name
                    };
                    m_Artists.Add(name, newArtist);
                }
                return m_Artists[name];
            }
        }

        Album GetAlbumInternal(string albumArtist, string albumName, int releaseYear)
        {
            var artist = GetArtistInternal(albumArtist);

            if (albumName == null)
            {
                albumName = string.Empty;
            }

            lock (m_Lock)
            {
                if (!artist.AlbumExists(albumName, releaseYear))
                {
                    var album = new Album
                    {
                        Artist = artist,
                        Name = albumName,
                        ReleaseYear = releaseYear
                    };
                    artist.AddAlbum(album);
                }
                return artist.GetAlbum(albumName, releaseYear);
            }
        }

        Disk GetDiskInternal(string albumArtist, string albumName, int releaseYear, int diskNumber)
        {
            var album = GetAlbumInternal(albumArtist, albumName, releaseYear);


            lock (m_Lock)
            {
                if (!album.DiskExists(diskNumber))
                {
                    var disk = new Disk
                    {
                        Album = album,
                        DiskNumber = diskNumber
                    };
                    album.AddDisk(disk);
                }

                return album.GetDisk(diskNumber);
            }
        }
    }
}