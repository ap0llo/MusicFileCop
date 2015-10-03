using System.Collections.Generic;

namespace MusicFileCop.Core.Metadata
{
    public interface IArtist : ICheckable
    {

        string Name { get; }


        IEnumerable<IAlbum> Albums { get; }

    }
}
