
namespace MusicFileCop.Core.Rules
{
    public interface IRule
    {
        
    }

    public interface IRule<T> : IRule where T : ICheckable
    {     
        string Description { get; }

        bool IsApplicable(T item);

        bool IsConsistent(T item);
    }
}
