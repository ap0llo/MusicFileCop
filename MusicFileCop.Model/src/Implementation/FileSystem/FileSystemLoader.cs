using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO = System.IO;
using Directory = MusicFileCop.Model.FileSystem.Directory;
using System.IO;

namespace MusicFileCop.Model.FileSystem
{
    class FileSystemLoader : IFileSystemLoader
    {

        public IDirectory LoadDirectory(string path)
        {        
            return LoadDirectory(path, null);
            
        }

        private IDirectory LoadDirectory(string path, IDirectory parent)
        {
            if (!IO.Directory.Exists(path))
            {
                throw new DirectoryNotFoundException($"Could not find directory '{path}'");
            }

            var name = Path.GetFileName(path);
            var currentDirectory = new Directory(parent, name);

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
