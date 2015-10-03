using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MusicFileCop.Core.FileSystem
{
    [DebuggerDisplay("Directory [{Name}]")]
    class Directory : IDirectory
    {
        readonly IDictionary<string, IFile> m_Files = new Dictionary<string, IFile>(StringComparer.InvariantCultureIgnoreCase);
        readonly IDictionary<string, IDirectory> m_Directories = new Dictionary<string, IDirectory>(StringComparer.InvariantCultureIgnoreCase);


        public IEnumerable<IDirectory> Directories => m_Directories.Values;

        public IEnumerable<IFile> Files => m_Files.Values;

        public string Name => Path.GetFileName(FullPath);   

        public string FullPath { get; }

        public IDirectory ParentDirectory { get; }   


        public Directory(IDirectory parentDirectory, string fullPath)
        {
            if(String.IsNullOrEmpty(fullPath))
            {
                throw new ArgumentException("Directory path must not be null or empty", nameof(fullPath));
            }

            this.ParentDirectory = parentDirectory;
            this.FullPath = fullPath;
        }


        public bool FileExists(string nameWithExtension) => m_Files.ContainsKey(nameWithExtension);

        public IFile GetFile(string nameWithExtension) => m_Files[nameWithExtension];

        internal void AddFile(IFile file)
        {
            this.m_Files.Add(file.NameWithExtension, file);
        }

        internal void AddDirectory(IDirectory directory)
        {
            this.m_Directories.Add(directory.Name, directory);
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
