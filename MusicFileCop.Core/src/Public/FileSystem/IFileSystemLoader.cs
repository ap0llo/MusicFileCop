namespace MusicFileCop.Core.FileSystem
{
    public interface IFileSystemLoader
    {
        /// <summary>
        /// Recursively loads the specified directory
        /// </summary>
        IDirectory LoadDirectory(string path);
    }
}
