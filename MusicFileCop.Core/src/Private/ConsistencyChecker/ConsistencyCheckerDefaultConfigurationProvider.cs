using System;
using System.Linq;
using MusicFileCop.Core.Configuration;
using MusicFileCop.Core.Rules;
using Ninject;

namespace MusicFileCop.Core
{
    class ConsistencyCheckerDefaultConfigurationProvider : ConsistencyCheckerBase, IDefaultConfigurationProvider
    {
        readonly IRuleSet m_RuleSet;        

        public ConsistencyCheckerDefaultConfigurationProvider(IRuleSet ruleSet)
        {
            if (ruleSet == null)
            {
                throw new ArgumentNullException(nameof(ruleSet));
            }
            m_RuleSet = ruleSet;
        }


        public string ConfigurationNamespace => ConsistencyCheckerBase.ConfigurationNamespace;

        public void Configure(IMutableConfigurationNode configurationNode)
        {
            foreach (var rule in m_RuleSet.AllRules)
            {
                configurationNode.AddValue(GetRuleEnableSettingsName(rule), true);
                configurationNode.AddValue(GetRuleSeveritySettingsName(rule), Severity.Warning);
            }
        }        
        
    }
}