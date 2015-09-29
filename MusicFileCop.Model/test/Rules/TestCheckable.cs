using System;

namespace MusicFileCop.Model.Test.Rules
{
    public class TestCheckable : ICheckable
    {
        public void Accept(IVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }

}
