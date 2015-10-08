namespace MusicFileCop.Core.Configuration
{
    /// <summary>
    /// Interface for default configuration. 
    /// By making it a separate interface, a class can request the default configuration using Ninject 
    /// </summary>
    public interface IDefaultConfigurationNode : IConfigurationNode
    {
         
    }
}