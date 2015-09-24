using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Model.Metadata
{
    public interface IAlbum
    {
        IArtist Artist { get; }

        string Name { get; }

        int ReleaseYear { get; }

        IEnumerable<IDisk> Disks { get; }
        
    }
}
