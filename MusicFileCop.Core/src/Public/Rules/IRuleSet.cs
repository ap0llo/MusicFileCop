using System.Collections.Generic;

namespace MusicFileCop.Core.Rules
{
    public interface IRuleSet
    {
        /// <summary>
        /// Gets all rules that are currently loaded
        /// </summary>
        IEnumerable<IRule> AllRules { get; }

        /// <summary>
        /// Gets all rules for the specified item type
        /// </summary>
        IEnumerable<IRule<T>> GetRules<T>() where T : ICheckable;
    }
}