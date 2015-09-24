using MusicFileCop.Model.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Model.Metadata
{
    class Album : IAlbum
    {
        readonly IDictionary<int, Disk> m_Disks = new Dictionary<int, Disk>();


        public IArtist Artist { get; internal set; }

        public IEnumerable<IDisk> Disks => Disks;
        
        public string Name { get; internal set; }

        public int ReleaseYear { get; internal set; }


        public void AddDisk(Disk disk)
        {
            m_Disks.Add(disk.DiskNumber, disk);    
        }

        public bool DiskExists(int diskNumber) => m_Disks.ContainsKey(diskNumber);

        public Disk GetDisk(int diskNumber) => m_Disks[diskNumber];
        
    }
}
