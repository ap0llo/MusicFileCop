using System.IO;
using System.Linq;

namespace MusicFileCop.Core
{
    public static class TextWriterExtensions
    {

        public static void WriteIndentedLine(this TextWriter writer, string line, int indentationDepth)
        {
            var prefix = indentationDepth > 0
                ? Enumerable.Range(0, indentationDepth).Select(x => "  ").Aggregate((a, b) => a + b)
                : "";

            writer.WriteLine(prefix + line);
        }

    }
}