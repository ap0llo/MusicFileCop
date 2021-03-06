﻿using System;
using System.IO;

namespace MusicFileCop.Core.FileSystem
{
    public class File : IFile
    {

        public IDirectory Directory { get; }

        public string NameWithExtension => Path.GetFileName(FullPath);

        public string Extension => Path.GetExtension(NameWithExtension);

        public string Name => Path.GetFileNameWithoutExtension(NameWithExtension);

        public string FullPath { get; }
       

        public File(IDirectory parentDirectory, string fullPath)
        {
            if (parentDirectory == null)
            {
                throw new ArgumentNullException(nameof(parentDirectory));
            }

            if (String.IsNullOrEmpty(fullPath))
            {
                throw new ArgumentException("File path must not be empty", nameof(fullPath));
            }

            this.Directory = parentDirectory;
            this.FullPath = fullPath;            

        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
