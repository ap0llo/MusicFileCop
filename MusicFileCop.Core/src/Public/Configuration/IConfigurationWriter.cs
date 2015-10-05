namespace MusicFileCop.Core.Configuration
{
    public interface IConfigurationWriter
    {
        void WriteConfiguration(IConfigurationNode configuration, string outputPath);

    }
}