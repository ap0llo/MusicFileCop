using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Model.Metadata
{
    public interface IDisk : ICheckable
    {
        IAlbum Album { get; }

        int DiskNumber { get; }

        IEnumerable<ITrack> Tracks { get; }
    }
}
