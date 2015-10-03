using System.Collections.Generic;

namespace MusicFileCop.Core.Metadata
{
    class Album : IAlbum
    {
        readonly IDictionary<int, Disk> m_Disks = new Dictionary<int, Disk>();


        public IArtist Artist { get; internal set; }

        public IEnumerable<IDisk> Disks => m_Disks.Values;
        
        public string Name { get; internal set; }

        public int ReleaseYear { get; internal set; }


        public void AddDisk(Disk disk)
        {
            m_Disks.Add(disk.DiskNumber, disk);    
        }

        public bool DiskExists(int diskNumber) => m_Disks.ContainsKey(diskNumber);

        public Disk GetDisk(int diskNumber) => m_Disks[diskNumber];

        public void Accept(IVisitor visitor) => visitor.Visit(this);


        public override string ToString() => $"[Album '{Name}' (Artist = '{Artist.Name}')]";
        
    }
}
