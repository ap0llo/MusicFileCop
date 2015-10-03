using MusicFileCop.Core.Rules;
using MusicFileCop.Model;

namespace MusicFileCop.Core.Output
{

    public interface IOutputWriter<T> where T : ICheckable
    {
        void WriteViolation(IRule<T> violatedRule, T item);
    }


}
