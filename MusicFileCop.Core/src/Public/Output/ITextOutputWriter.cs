using System.IO;

namespace MusicFileCop.Core.Output
{
    public interface ITextOutputWriter
    {
        /// <summary>
        /// Writes output to the specifed <see cref="TextWriter"/>
        /// </summary>
        void WriteTo(TextWriter writer);
    }
}