using System;
using System.Linq;
using MusicFileCop.Core.Configuration;
using MusicFileCop.Core.Rules;
using Ninject;

namespace MusicFileCop.Core
{
    /// <summary>
    /// Default configuration provider for consistency checker
    /// </summary>
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


        public string ConfigurationNamespace => s_ConfigurationNamespace;

        public void Configure(IMutableConfigurationNode configurationNode)
        {
            // by default, all rules are enabled and have a severity of "Warning"
            foreach (var rule in m_RuleSet.AllRules)
            {
                configurationNode.AddValue(GetRuleEnableSettingsName(rule), true);
                configurationNode.AddValue(GetRuleSeveritySettingsName(rule), Severity.Warning);
            }
        }        
        
    }
}