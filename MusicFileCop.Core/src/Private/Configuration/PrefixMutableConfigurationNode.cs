using System;

namespace MusicFileCop.Core.Configuration
{
    /// <summary>
    ///     Implementation of <see cref="IMutableConfigurationNode" /> that adds a configurable prefix to all
    ///     settings names before calling inner implementation of <see cref="IMutableConfigurationNode"/>
    /// </summary>
    class PrefixMutableConfigurationNode : PrefixConfigurationNode, IMutableConfigurationNode
    {
        readonly IMutableConfigurationNode m_WrappedConfigurationNode;



        public PrefixMutableConfigurationNode(IMutableConfigurationNode wrappedConfigurationNode, string prefix)
            : base(wrappedConfigurationNode, prefix)
        {
            if (wrappedConfigurationNode == null)
            {
                throw new ArgumentNullException(nameof(wrappedConfigurationNode));
            }

            m_WrappedConfigurationNode = wrappedConfigurationNode;
        }



        public void AddValue<T>(string name, T value) => m_WrappedConfigurationNode.AddValue(GetPrefixedName(name), value);

    }
}