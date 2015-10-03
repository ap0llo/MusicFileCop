
namespace MusicFileCop.Core.Rules
{
    public interface IRule<in T> where T : ICheckable
    {     
        string Description { get; }

        bool IsApplicable(T item);

        bool IsConsistent(T item);
    }
}
