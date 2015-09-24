using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Model.FileSystem
{
    class Directory : IDirectory
    {
        readonly IDictionary<string, IFile> m_Files = new Dictionary<string, IFile>(StringComparer.InvariantCultureIgnoreCase);
        readonly IDictionary<string, IDirectory> m_Directories = new Dictionary<string, IDirectory>(StringComparer.InvariantCultureIgnoreCase);


        public IEnumerable<IDirectory> Directories => m_Directories.Values;

        public IEnumerable<IFile> Files => m_Files.Values;
        
        public string Name { get; }     

        public IDirectory ParentDirectory { get; }   


        public Directory(IDirectory parentDirectory, string name)
        {
            if(String.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Directory Name must not be null or empty", nameof(name));
            }

            this.ParentDirectory = parentDirectory;
            this.Name = name;
        }


        public bool FileExists(string nameWithExtension) => m_Files.ContainsKey(nameWithExtension);              

        internal void AddFile(IFile file)
        {
            this.m_Files.Add(file.NameWithExtension, file);
        }

        internal void AddDirectory(IDirectory directory)
        {
            this.m_Directories.Add(directory.Name, directory);
        }
    }
}
