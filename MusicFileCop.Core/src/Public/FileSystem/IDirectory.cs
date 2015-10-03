using System.Collections.Generic;
using MusicFileCop.Model;

namespace MusicFileCop.Core.FileSystem
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
