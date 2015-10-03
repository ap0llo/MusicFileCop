using MusicFileCop.Core.Rules;

namespace MusicFileCop.Core.Output
{

    public interface IOutputWriter<T> where T : ICheckable
    {
        void WriteViolation(IRule<T> violatedRule, T item);
    }


}
