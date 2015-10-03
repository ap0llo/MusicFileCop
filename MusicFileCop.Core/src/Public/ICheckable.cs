using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicFileCop.Model;

namespace MusicFileCop.Core
{
    /// <summary>
    /// Base interface for all elements that can be checked by a rule
    /// </summary>
    public interface ICheckable
    {
        void Accept(IVisitor visitor);

    }
}
