using MusicFileCop.Core.FileSystem;

namespace MusicFileCop.Core
{
    public interface IConsistencyChecker
    {

        void CheckConsistency(IDirectory directory);
      
    }
}
