using System.IO;

namespace MusicFileCop.Core.Output
{
    public interface ITextOutputWriter
    {
        /// <summary>
        /// Writes output to the specified <see cref="TextWriter"/>
        /// </summary>
        void WriteTo(TextWriter writer);
    }
}