using MusicFileCop.Core.FileSystem;

namespace MusicFileCop.Core
{
    public interface IConsistencyChecker
    {

        /// <summary>
        /// Recursivley checks all items wihtin the specified directory for consistency
        /// </summary>
        void CheckConsistency(IDirectory directory);
      
    }
}
