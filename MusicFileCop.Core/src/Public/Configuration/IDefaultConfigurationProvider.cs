namespace MusicFileCop.Core.Configuration
{
    /// <summary>
    /// Interface to be implemented by classes providing configuration default values
    /// </summary>
    public interface IDefaultConfigurationProvider
    {
        /// <summary>
        /// Gets the namespace of all settings provided by this provider
        /// </summary>
        string ConfigurationNamespace { get; }

        /// <summary>
        /// Offers the provider to add its values to the default configuraiton
        /// (Will be automatically called at program startup)
        /// </summary>
        /// <param name="configurationNode"></param>
        void Configure(IMutableConfigurationNode configurationNode);

    }
}