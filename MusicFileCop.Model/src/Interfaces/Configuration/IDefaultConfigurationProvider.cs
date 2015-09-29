namespace MusicFileCop.Model.Configuration
{
    public interface IDefaultConfigurationProvider
    {
        string ConfigurationNamespace { get; }

        void Configure(IMutableConfigurationNode configurationNode);

    }
}