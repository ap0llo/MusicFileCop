using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Model.FileSystem
{
    public interface IFile
    {
        IDirectory Directory { get; }

        string Name { get; }

        string Extension { get; }

        string NameWithExtension { get; }

        string FullPath { get; }
    }

}
