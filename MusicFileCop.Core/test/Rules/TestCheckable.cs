using System;
using MusicFileCop.Core;

namespace MusicFileCop.Core.Test.Rules
{
    public class TestCheckable : ICheckable
    {
        public void Accept(IVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }

}
