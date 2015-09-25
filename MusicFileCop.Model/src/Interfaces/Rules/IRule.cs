using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Model.Rules
{
    public interface IRule<T> where T : ICheckable
    {     
        string Description { get; }

        bool IsApplicable(T item);

        bool IsConsistent(T item);
    }
}
