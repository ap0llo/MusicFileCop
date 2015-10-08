using System.Collections.Generic;

namespace MusicFileCop.Core.Metadata
{
    class Disk : IDisk
    {
        readonly ISet<ITrack> m_Tracks = new HashSet<ITrack>();


        public IAlbum Album { get; internal set; }
        
        public int DiskNumber { get; internal set; }

        public IEnumerable<ITrack> Tracks => m_Tracks;


        public void AddTrack(ITrack track) => m_Tracks.Add(track);

        public void Accept(IVisitor visitor) => visitor.Visit(this);

    }
}
