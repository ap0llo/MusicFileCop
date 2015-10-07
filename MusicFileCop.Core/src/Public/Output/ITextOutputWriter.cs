using System.IO;

namespace MusicFileCop.Core.Output
{
    public interface ITextOutputWriter
    {
        void WriteTo(TextWriter writer);
    }
}