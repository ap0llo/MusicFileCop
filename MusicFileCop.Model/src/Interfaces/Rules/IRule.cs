using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Model.Rules
{
    public interface IRule<T>
    {     
        string Description { get; }

        bool IsApplicable(IFileMapper fileMapper, T musicFile);

        bool IsConsistent(IFileMapper fileMapper, T musicFile);
    }
}
