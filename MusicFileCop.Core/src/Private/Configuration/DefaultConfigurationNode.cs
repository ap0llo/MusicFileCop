using System.Collections.Generic;

namespace MusicFileCop.Core.Configuration
{
    /// <summary>
    ///     Wraps a instance of <see cref="IConfigurationNode" /> and implements <see cref="IConfigurationWriter" />.
    /// </summary>
    internal class DefaultConfigurationNode : IDefaultConfigurationNode
    {
        readonly IConfigurationNode m_WrappedConfigurationNode;

        public DefaultConfigurationNode(IConfigurationNode wrappedConfigurationNode)
        {
            m_WrappedConfigurationNode = wrappedConfigurationNode;
        }


        public IEnumerable<string> Names => m_WrappedConfigurationNode.Names;

        public bool TryGetValue(string name, out string value) => m_WrappedConfigurationNode.TryGetValue(name, out value);

        public string GetValue(string name) => m_WrappedConfigurationNode.GetValue(name);

        public bool TryGetValue<T>(string name, out T value) => m_WrappedConfigurationNode.TryGetValue(name, out value);

        public T GetValue<T>(string name) => m_WrappedConfigurationNode.GetValue<T>(name);
    }
}