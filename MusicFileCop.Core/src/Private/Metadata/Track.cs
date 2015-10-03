
namespace MusicFileCop.Core.Metadata
{
    class Track : ITrack
    {
        public IAlbum Album => Disk.Album;

        public IArtist AlbumArtist => Album.Artist;

        public IArtist Artist { get; internal set; }

        public IDisk Disk { get; internal set; }        

        public string Name { get; internal set; }

        public int TrackNumber { get; internal set; }

        public void Accept(IVisitor visitor) => visitor.Visit(this);


        public override string ToString() => $"[Track '{Artist.Name} - {Name}']";
        
    }
}
