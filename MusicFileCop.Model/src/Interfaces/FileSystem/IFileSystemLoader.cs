using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Model.FileSystem
{
    public interface IFileSystemLoader
    {
        /// <summary>
        /// Recursively loads the specified directory
        /// </summary>
        IDirectory LoadDirectory(string path);
    }
}
