using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Model.FileSystem
{
    public interface IDirectory : ICheckable
    {
        string Name { get; }

        string FullPath { get; }

        IDirectory ParentDirectory { get; }

        IEnumerable<IDirectory> Directories { get; }

        IEnumerable<IFile> Files { get; }

        bool FileExists(string nameWithExtension);

        IFile GetFile(string nameWithExtension);

    }
}
