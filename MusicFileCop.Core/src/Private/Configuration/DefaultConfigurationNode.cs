using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Core.Configuration
{
    class DefaultConfigurationNode : IDefaultConfigurationNode
    {

        private readonly IConfigurationNode m_WrappedConfigurationNode;

        public DefaultConfigurationNode(IConfigurationNode wrappedConfigurationNode)
        {
            m_WrappedConfigurationNode = wrappedConfigurationNode;
        }

        public bool TryGetValue(string name, out string value) => m_WrappedConfigurationNode.TryGetValue(name, out value);

        public string GetValue(string name) => m_WrappedConfigurationNode.GetValue(name);

        public bool TryGetValue<T>(string name, out T value) => m_WrappedConfigurationNode.TryGetValue(name, out value);

        public T GetValue<T>(string name) => m_WrappedConfigurationNode.GetValue<T>(name);

        public IEnumerable<string> Names => m_WrappedConfigurationNode.Names;
    }
}
