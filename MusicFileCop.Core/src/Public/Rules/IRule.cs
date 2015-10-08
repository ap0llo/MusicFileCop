
using System.Security.Cryptography.X509Certificates;

namespace MusicFileCop.Core.Rules
{
    public interface IRule
    {
        /// <summary>
        /// Gets the Id of this rule. This id needs to be unique, as configuration of consistency checker is tied to this id.
        /// The Id should be human-readable (a short description of what the rule checks)
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Description of the rule to be displayed in help and output
        /// </summary>
        string Description { get; }
    }

    /// <summary>
    /// Interface to be implemented by all rule classes.
    /// Classes implementing this interface will be automatically discovered and instantiated if they are required for the consistency check
    /// </summary>
    /// <typeparam name="T">The type of item this rule checks</typeparam>
    public interface IRule<T> : IRule where T : ICheckable
    {     
        /// <summary>
        /// Determines whether this rule applies to the concrete item.
        /// If rule is not applicable, IsConsistent() will not be called
        /// </summary>
        bool IsApplicable(T item);

        /// <summary>
        /// Checks whether the specified item is consistent with this rule
        /// </summary>
        bool IsConsistent(T item);
    }
}
