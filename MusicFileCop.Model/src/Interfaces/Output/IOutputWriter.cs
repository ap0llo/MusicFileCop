using MusicFileCop.Model.FileSystem;
using MusicFileCop.Model.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Model.Output
{
    public interface IOutputWriter
    {
        
        void WriteViolation<T>(IRule<T> violatedRule, T file) where T : ICheckable;

    }
}
