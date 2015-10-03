
namespace MusicFileCop.Core.FileSystem
{
    public interface IFile : ICheckable
    {
        IDirectory Directory { get; }

        string Name { get; }

        string Extension { get; }

        string NameWithExtension { get; }

        string FullPath { get; }
    }

}
