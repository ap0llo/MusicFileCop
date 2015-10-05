using MusicFileCop.Core.Rules;

namespace MusicFileCop.Core
{
    class ConsistencyCheckerBase
    {
        public const string ConfigurationNamespace = "ConsistencyChecker";

        protected string GetRuleEnableSettingsName(IRule rule) => $"{rule.Id}:Enabled";

        protected string GetRuleSeveritySettingsName(IRule rule) => $"{rule.Id}:Severity";
    }
}