using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Model.FileSystem
{
    public interface IDirectory
    {
        string Name { get; }

        IDirectory ParentDirectory { get; }

        IEnumerable<IDirectory> Directories { get; }

        IEnumerable<IFile> Files { get; }

        bool FileExists(string nameWithExtension);


    }
}
