using MusicFileCop.Core.Rules;

namespace MusicFileCop.Core
{
    class ConsistencyCheckerBase
    {
        public const string ConfigurationNamespace = "ConsistencyChecker";

        protected string GetRuleEnableSettingsName(IRule rule) => $"Enable-{rule.Id}";
    }
}