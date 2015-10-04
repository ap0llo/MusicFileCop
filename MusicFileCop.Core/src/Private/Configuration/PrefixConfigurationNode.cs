using System;

namespace MusicFileCop.Core.Configuration
{
    /// <summary>
    ///     Implementation of <see cref="IConfigurationNode" /> that adds a configurable prefix to all
    ///     settings names before calling inner implementation of IConfigurationNode
    /// </summary>
    public class PrefixConfigurationNode : IConfigurationNode
    {
        readonly string m_Prefix;
        readonly IConfigurationNode m_WrappedConfigurationNode;


        public PrefixConfigurationNode(IConfigurationNode wrappedConfigurationNode, string prefix)
        {
            if (wrappedConfigurationNode == null)
            {
                throw new ArgumentNullException(nameof(wrappedConfigurationNode));
            }
            if (prefix == null)
            {
                throw new ArgumentNullException(nameof(prefix));
            }

            if (String.IsNullOrEmpty(prefix))
            {
                throw new ArgumentException("Prefix must not be empty", nameof(prefix));
            }


            m_WrappedConfigurationNode = wrappedConfigurationNode;
            m_Prefix = prefix;
        }


        public bool TryGetValue(string name, out string value)
        {
            return m_WrappedConfigurationNode.TryGetValue(GetPrefixedName(name), out value);
        }

        public string GetValue(string name) => m_WrappedConfigurationNode.GetValue(GetPrefixedName(name));

        public bool TryGetValue<T>(string name, out T value)
        {
            return m_WrappedConfigurationNode.TryGetValue(GetPrefixedName(name), out value);
        }

        public T GetValue<T>(string name) => m_WrappedConfigurationNode.GetValue<T>(GetPrefixedName(name));


        protected string GetPrefixedName(string name) => $"{m_Prefix}:{name}";
    }
}