using MusicFileCop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Test
{
    public class TestCheckable : ICheckable
    {
        public void Accept(IVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }

}
