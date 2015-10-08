using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Ninject;

namespace MusicFileCop.Core.Rules
{
    public class RuleSet : IRuleSet
    {

        readonly IKernel m_Kernel;
        readonly Lazy<IEnumerable<IRule>> m_AllRules;
        readonly IDictionary<Type, IEnumerable<IRule>> m_Rules = new ConcurrentDictionary<Type, IEnumerable<IRule>>();


        public RuleSet(IKernel kernel)
        {
            if (kernel == null)
            {
                throw new ArgumentNullException(nameof(kernel));
            }
            m_Kernel = kernel;

            m_AllRules = new Lazy<IEnumerable<IRule>>(() => m_Kernel.GetAll<IRule>(), true);
        }


        public IEnumerable<IRule> AllRules => m_AllRules.Value;
    

        public IEnumerable<IRule<T>> GetRules<T>() where T : ICheckable
        {
            if (!m_Rules.ContainsKey(typeof(T)))
            {
                var rules = m_Kernel.GetAll<IRule<T>>().ToArray();
                m_Rules.Add(typeof(T), rules);
            }

            return m_Rules[typeof(T)].Cast<IRule<T>>();
        }
    }
}