
namespace MusicFileCop.Core
{
    /// <summary>
    /// Base interface for all elements that can be checked by a rule
    /// </summary>
    public interface ICheckable
    {
        void Accept(IVisitor visitor);

    }
}
