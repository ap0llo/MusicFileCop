using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Model.Metadata
{
    interface IMetadataFactory
    {

        /// <summary>
        /// Gets or creates the artist with the specified name
        /// </summary>
        IArtist GetArtist(string name);

        /// <summary>
        /// Gets or creates the specifed album for the specifed artist (will create the artist if necessary)
        /// </summary>        
        IAlbum GetAlbum(string albumArtist, string albumName, int releaseYear);    

        IDisk GetDisk(string albumArtist, string albumName, int releaseYear, int diskNumber);

        ITrack GetTrack(string albumArtist, string albumName, int releaseYear, int diskNumber, int trackNumber, string name, string artist);

     
    }
}
