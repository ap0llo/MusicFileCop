using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Core
{
    public static class StringExtensions
    {

        public static string ReplaceInvalidFileNameChars(this string value, string replaceWith) => value.ReplaceInvalidChars(Path.GetInvalidFileNameChars(), replaceWith);

        public static string ReplaceInvalidPathChars(this string value, string replaceWith) => value.ReplaceInvalidChars(Path.GetInvalidPathChars(), replaceWith);


        public static string ReplaceInvalidChars(this string value, char[] invalidChars, string replaceWith)
        {
            var result = value;
            foreach (var invalidChar in invalidChars)
            {
                result = result.Replace(invalidChar.ToString(), replaceWith);
            }

            return result;
        }

    }
}
