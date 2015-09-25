using MusicFileCop.Model.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Model.Metadata
{
    public interface IMetadataLoader
    {

        /// <summary>
        /// Recursively loads all loadable metadata from the specified directory and associates them with 
        /// the file it was loaded from using the specified file mapper
        /// </summary>
        void LoadMetadata(IDirectory directory);


    }
}
