using MusicFileCop.Model.FileSystem;
using MusicFileCop.Model.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Model
{
    public interface IConsistencyChecker
    {


        void CheckConsistency(IFileMapper fileMapper, IDirectory directory);
      
    }
}
