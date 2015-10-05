
using System.Security.Cryptography.X509Certificates;

namespace MusicFileCop.Core.Rules
{
    public interface IRule
    {
        string Id { get; }

        string Description { get; }
    }

    public interface IRule<T> : IRule where T : ICheckable
    {     
        

        bool IsApplicable(T item);

        bool IsConsistent(T item);
    }
}
