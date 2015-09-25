using MusicFileCop.Model.FileSystem;
using MusicFileCop.Model.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Model
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
