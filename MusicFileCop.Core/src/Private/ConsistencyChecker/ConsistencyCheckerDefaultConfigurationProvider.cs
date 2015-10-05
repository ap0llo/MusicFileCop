using System;
using System.Linq;
using MusicFileCop.Core.Configuration;
using MusicFileCop.Core.Rules;
using Ninject;

namespace MusicFileCop.Core
{
    class ConsistencyCheckerDefaultConfigurationProvider : ConsistencyCheckerBase, IDefaultConfigurationProvider
    {
        private readonly IKernel m_Kernel;

        public ConsistencyCheckerDefaultConfigurationProvider(IKernel kernel)
        {
            if (kernel == null)
            {
                throw new ArgumentNullException(nameof(kernel));
            }

            m_Kernel = kernel;
        }


        public string ConfigurationNamespace => ConsistencyCheckerBase.ConfigurationNamespace;

        public void Configure(IMutableConfigurationNode configurationNode)
        {
            var rules = m_Kernel.GetAll<IRule>();
            foreach (var rule in rules)
            {
                configurationNode.AddValue(GetRuleEnableSettingsName(rule), true);
            }
        }        
        
    }
}