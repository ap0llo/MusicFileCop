namespace MusicFileCop.Core.Configuration
{
    public interface IConfigurationWriter
    {
        /// <summary>
        /// Serializes the specified configuration to JSON and writes it to the specified file
        /// </summary>
        void WriteConfiguration(IConfigurationNode configuration, string outputPath);

    }
}