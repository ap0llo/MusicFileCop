using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MusicFileCop.Core.Configuration;
using MusicFileCop.Core.DI;
using MusicFileCop.Core.Rules;
using MusicFileCop.Model;
using Ninject;
using Ninject.Activation;
using Ninject.Extensions.Conventions;
using Ninject.Parameters;

namespace MusicFileCop.Core
{
    class DynamicConfigurator : IDynamicConfigurator
    {
        const string s_RulesAssemblyName = "MusicFileCop.Rules.dll";

        readonly IKernel m_Kernel;
        readonly IMutableConfigurationNode m_DefaultConfigurationNode;

        /// <summary>
        /// Initializes a new instance of DynamicConfigurator
        /// </summary>
        /// <param name="kernel">The Ninject kernel to create the bindings on</param>
        /// <param name="defaultConfigurationNode">The configuration node to add all default config to</param>
        public DynamicConfigurator(IKernel kernel, IMutableConfigurationNode defaultConfigurationNode)
        {
            if (kernel == null)
            {
                throw new ArgumentNullException(nameof(kernel));                
            }

            if (defaultConfigurationNode == null)
            {
                throw new ArgumentNullException(nameof(defaultConfigurationNode));
            }

            m_Kernel = kernel;
            m_DefaultConfigurationNode = defaultConfigurationNode;
        }


        public void CreateDynamicBindings()
        {
  
            CreateRuleBindings();

            CreateConfigurationBindings();

            LoadDefaultConfiguration();
        }


        void CreateRuleBindings()
        {
            // get all checkables
            var ruleTypes = GetCheckableTypes()
                .Select(checkableType => typeof(IRule<>).MakeGenericType(checkableType))
                .ToArray();

            m_Kernel.Bind(x =>
                x.FromAssembliesMatching(s_RulesAssemblyName)
                    .SelectAllClasses()
                    .InheritedFromAny(ruleTypes)
                    .BindAllInterfaces()
                );
        }

        void CreateConfigurationBindings()
        {
            var configurableTypes = GetConfigurableTypes();

            foreach (var type in configurableTypes)
            {
                m_Kernel.Bind<IConfigurationMapper>()
                    .ToMethod(c => GetMapperForRule(c, type))
                    .WhenInjectedExactlyInto(type);
            }
        }

        void LoadDefaultConfiguration()
        {
            // bind all types implementing IDefaultConfigurationProvider
            m_Kernel.Bind(x =>
                x.FromAssembliesMatching(s_RulesAssemblyName)
                    .SelectAllClasses()
                    .InheritedFrom<IDefaultConfigurationProvider>()
                    .BindAllInterfaces());

            // get instances of all configuraiton providers
            var defaultConfigurations = m_Kernel.GetAll<IDefaultConfigurationProvider>();

            // call configure on all providers
            foreach (var configProvider in defaultConfigurations)
            {
                var wrapper = new PrefixMutableConfigurationNode(m_DefaultConfigurationNode, configProvider.ConfigurationNamespace);
                configProvider.Configure(wrapper);
            }
        }

        

        internal IEnumerable<Type> GetCheckableTypes() => GetCheckableTypes(Assembly.GetAssembly(typeof(CoreModule)));

        internal IEnumerable<Type> GetCheckableTypes(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(t => typeof (ICheckable).IsAssignableFrom(t))
                .Where(t => t.IsPublic);
        }

        internal IEnumerable<Type> GetConfigurableTypes()
        {
            return GetConfigurableTypes(Assembly.Load("MusicFileCop.Rules"))
                .Union(GetConfigurableTypes(Assembly.GetExecutingAssembly()));
        }

        internal IEnumerable<Type> GetConfigurableTypes(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(t => t.GetCustomAttribute<ConfigurationNamespaceAttribute>() != null);
        }

        IConfigurationMapper GetMapperForRule(IContext context, Type ruleImplemetationType)
        {
            var configurationNamespace = ruleImplemetationType.GetCustomAttribute<ConfigurationNamespaceAttribute>().Namespace;

            return context.Kernel.Get<PrefixConfigurationMapper>(
                new ConstructorArgument("settingsPrefix", configurationNamespace)
                );
        }
    }
}