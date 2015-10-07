using System.IO;
using NLog;
using IO = System.IO;

namespace MusicFileCop.Core.FileSystem
{
    class FileSystemLoader : IFileSystemLoader
    {

        private readonly ILogger m_Logger = LogManager.GetCurrentClassLogger();


        public IDirectory LoadDirectory(string path)
        {        
            return LoadDirectory(path, null);
            
        }

        private IDirectory LoadDirectory(string path, IDirectory parent)
        {
            m_Logger.Info($"Loading directory '{path}'");

            if (!IO.Directory.Exists(path))
            {
                throw new DirectoryNotFoundException($"Could not find directory '{path}'");
            }
            
            var currentDirectory = new Directory(parent, path);

            foreach (var filePath in IO.Directory.GetFiles(path))
            {
                var file = new File(currentDirectory, filePath);
                currentDirectory.AddFile(file);
            }

            foreach (var directoryPath in IO.Directory.GetDirectories(path))
            {
                var directory = LoadDirectory(directoryPath, currentDirectory);
                currentDirectory.AddDirectory(directory);
            }

            return currentDirectory;
        }

    }
}
