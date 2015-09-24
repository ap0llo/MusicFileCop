using MusicFileCop.Model.FileSystem;
using MusicFileCop.Model.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Model
{
    public interface IFileMapper
    {
        void AddMapping(ITrack track, IFile file);

        ITrack GetTrack(IFile file);

        IFile GetFile(ITrack track);

        IEnumerable<IDirectory> GetDirectories(IAlbum album);

        IEnumerable<IDirectory> GetDirectories(IDisk disk);        
    }
}
