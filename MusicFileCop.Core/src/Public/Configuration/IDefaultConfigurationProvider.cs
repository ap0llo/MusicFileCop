namespace MusicFileCop.Core.Configuration
{
    public interface IDefaultConfigurationProvider
    {
        string ConfigurationNamespace { get; }

        void Configure(IMutableConfigurationNode configurationNode);

    }
}