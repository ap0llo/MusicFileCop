using System.Collections.Generic;
using MusicFileCop.Model;

namespace MusicFileCop.Core.Metadata
{
    public interface IArtist : ICheckable
    {

        string Name { get; }


        IEnumerable<IAlbum> Albums { get; }

    }
}
