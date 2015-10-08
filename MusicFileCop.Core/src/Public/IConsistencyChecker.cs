using MusicFileCop.Core.FileSystem;

namespace MusicFileCop.Core
{
    public interface IConsistencyChecker
    {

        /// <summary>
        /// Recursively checks all items within the specified directory for consistency
        /// </summary>
        void CheckConsistency(IDirectory directory);
      
    }
}
