using System.IO;
using NLog;
using IO = System.IO;

namespace MusicFileCop.Core.FileSystem
{
    class FileSystemLoader : IFileSystemLoader
    {
        readonly ILogger m_Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Recursivley loads the specified directory
        /// </summary>
        public IDirectory LoadDirectory(string path) => LoadDirectory(path, null);

        IDirectory LoadDirectory(string path, IDirectory parent)
        {
            m_Logger.Info($"Loading directory '{path}'");

            // throw exception if directory does not exist
            if (!IO.Directory.Exists(path))
            {
                throw new DirectoryNotFoundException($"Could not find directory '{path}'");
            }
            
            // create directory object
            var currentDirectory = new Directory(parent, path);

            // load files 
            foreach (var filePath in IO.Directory.GetFiles(path))
            {
                var file = new File(currentDirectory, filePath);
                currentDirectory.AddFile(file);
            }

            // load sub-directories
            foreach (var directoryPath in IO.Directory.GetDirectories(path))
            {
                var directory = LoadDirectory(directoryPath, currentDirectory);
                currentDirectory.AddDirectory(directory);
            }

            return currentDirectory;
        }

    }
}
