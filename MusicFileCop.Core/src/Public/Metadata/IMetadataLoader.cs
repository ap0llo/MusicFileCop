using MusicFileCop.Core.FileSystem;

namespace MusicFileCop.Core.Metadata
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
