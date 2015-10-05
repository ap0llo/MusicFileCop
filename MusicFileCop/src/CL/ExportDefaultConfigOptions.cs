using CommandLine;

namespace MusicFileCop.CL
{
    [Verb("export-default-config", HelpText = "Loads the default configuration and writes it to the specified file")]
    public class ExportDefaultConfigOptions
    {

        [Option("out", Required = true)]
        public string OutputFile { get; set; }

    }
}