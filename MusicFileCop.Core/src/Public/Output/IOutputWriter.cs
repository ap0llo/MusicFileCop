using MusicFileCop.Core.Rules;

namespace MusicFileCop.Core.Output
{
    /// <summary>
    /// Interface to encapsulate emitting rule violations
    /// </summary>
    /// <typeparam name="T">The type of the checkable item that is not consistent with a rule</typeparam>
    public interface IOutputWriter<T> where T : ICheckable
    {
        /// <summary>
        /// Writes a violation to the output writer
        /// </summary>
        /// <param name="violatedRule">The instance of the rule that was violated</param>
        /// <param name="severity">The severity of the rule violation</param>
        /// <param name="item">The item that is not consistent with the rule</param>
        void WriteViolation(IRule<T> violatedRule, Severity severity, T item);        
    }


}
