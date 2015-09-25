using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Model
{
    public interface IWrapper<T>
    {

        T WrappedObject { get; }

    }
}
