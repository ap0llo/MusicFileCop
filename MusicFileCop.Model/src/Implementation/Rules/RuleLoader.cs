using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MusicFileCop.Model.Configuration;
using MusicFileCop.Model.DI;
using Ninject;
using Ninject.Activation;
using Ninject.Extensions.Conventions;
using Ninject.Parameters;

namespace MusicFileCop.Model.Rules
{
    class RuleLoader : IRuleLoader
    {
        const string s_RulesAssemblyName = "MusicFileCop.Rules.dll";

        readonly IKernel m_Kernel;


        public RuleLoader(IKernel kernel)
        {
            if (kernel == null)
                throw new ArgumentNullException(nameof(kernel));

            m_Kernel = kernel;
        }


        public void LoadAllRules()
        {
            // get all checkables
            var ruleTypes = GetCheckableTypes()
                .Select(checkableType => typeof (IRule<>).MakeGenericType(checkableType))
                .ToArray();

            m_Kernel.Bind(x =>
                x.FromAssembliesMatching(s_RulesAssemblyName)
                    .SelectAllClasses()
                    .InheritedFromAny(ruleTypes)
                    .BindAllInterfaces()
                );

            var ruleImplementingTypes = ruleTypes
                .SelectMany(t => m_Kernel.GetAll(t))
                .Select(instance => instance.GetType())
                .ToArray();

            foreach (var type in ruleImplementingTypes)
            {
                if (type.GetCustomAttribute<ConfigurationNamespaceAttribute>() != null)
                {
                    m_Kernel.Bind<IMapper>()
                        .ToMethod(c => GetMapperForRule(c, type))
                        .WhenInjectedInto(type);                    
                }
            }
        }


        internal IEnumerable<Type> GetCheckableTypes() => GetCheckableTypes(Assembly.GetAssembly(typeof (ModelModule)));

        internal IEnumerable<Type> GetCheckableTypes(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(t => typeof (ICheckable).IsAssignableFrom(t))
                .Where(t => t.IsPublic);
        }


        IMapper GetMapperForRule(IContext context, Type ruleImplemetationType)
        {
            var configurationNamespace = ruleImplemetationType.GetCustomAttribute<ConfigurationNamespaceAttribute>().Namespace;

            return context.Kernel.Get<PrefixConfigurationMapper>(
                new ConstructorArgument("settingsPrefix", configurationNamespace)
                );
        }
    }
}