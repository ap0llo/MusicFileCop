using System;
using System.Collections.Generic;
using Microsoft.Framework.ConfigurationModel;

namespace MusicFileCop.Model.Configuration
{
    class HierarchicalConfigurationNode : ConfigurationNodeBase, IConfigurationNode
    {
        readonly IConfiguration m_Configuration;
        readonly IConfigurationNode m_ParentNode;


        public HierarchicalConfigurationNode(IConfigurationNode parentNode, IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            m_ParentNode = parentNode;
            m_Configuration = configuration;
        }



        protected override bool TryGetValue(string name, out string value) => m_Configuration.TryGet(name, out value);

        protected override T HandleMissingValue<T>(string name)
        {
            if (m_ParentNode != null)
            {
                return m_ParentNode.GetValue<T>(name);
            }
            throw new KeyNotFoundException($"No value found for name '{name}'");
        }
    }
}