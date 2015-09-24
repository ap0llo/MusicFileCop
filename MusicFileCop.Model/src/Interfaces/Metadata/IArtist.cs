using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Model.Metadata
{
    public interface IArtist
    {

        string Name { get; }


        IEnumerable<IAlbum> Albums { get; }

    }
}
