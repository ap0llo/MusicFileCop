using CommandLine;

namespace MusicFileCop.CL
{
    [Verb("check", HelpText = "Check consistency of all music files in a directory")]
    public class CheckOptions
    {
        [Option(Required = true, HelpText = "The path of the directory to seach for music files and execute the consistency check on")]
        public string Path { get; set; }


        [Option("out", Required = true)]
        public string OutputFile { get; set; }
    }
}