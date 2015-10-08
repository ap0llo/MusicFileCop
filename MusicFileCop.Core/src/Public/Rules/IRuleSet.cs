using System.Collections.Generic;

namespace MusicFileCop.Core.Rules
{
    public interface IRuleSet
    {
        IEnumerable<IRule> AllRules { get; }

        IEnumerable<IRule<T>> GetRules<T>() where T : ICheckable;
    }
}