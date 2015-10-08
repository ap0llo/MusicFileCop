namespace MusicFileCop.Core.Metadata
{
    interface IMetadataFactory
    {

        /// <summary>
        /// Gets or creates the artist with the specified name
        /// </summary>
        IArtist GetArtist(string name);

        /// <summary>
        /// Gets or creates the specified album for the specified artist (will create the artist if necessary)
        /// </summary>        
        IAlbum GetAlbum(string albumArtist, string albumName, int releaseYear);    

        IDisk GetDisk(string albumArtist, string albumName, int releaseYear, int diskNumber);

        ITrack GetTrack(string albumArtist, string albumName, int releaseYear, int diskNumber, int trackNumber, string name, string artist);

     
    }
}
