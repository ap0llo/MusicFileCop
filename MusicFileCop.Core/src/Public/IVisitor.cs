using MusicFileCop.Core.FileSystem;
using MusicFileCop.Core.Metadata;

namespace MusicFileCop.Core
{
    public interface IVisitor
    {
        void Visit(IDirectory directory);

        void Visit(IFile file);

        void Visit(IAlbum album);

        void Visit(IArtist artist);

        void Visit(IDisk disk);

        void Visit(ITrack track);
    }
}
