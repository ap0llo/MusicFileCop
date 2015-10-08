using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Framework.ConfigurationModel;
using TagLib.IFD.Tags;

namespace MusicFileCop.Core.Configuration
{
    /// <summary>
    /// Configuration node that delegates requests for values not present in the node to a parent node
    /// </summary>
    class HierarchicalConfigurationNode : ConfigurationNodeBase
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


        public override IEnumerable<string> Names  { get { throw new NotImplementedException(); } }
        
        public override bool TryGetValue(string name, out string value) => m_Configuration.TryGet(name, out value);
  

        protected override T HandleMissingValue<T>(string name)
        {
            // if we have a parent node, request the setting from there
            if (m_ParentNode != null)
            {
                return m_ParentNode.GetValue<T>(name);
            }
            throw new KeyNotFoundException($"No value found for name '{name}'");
        }
    }
}