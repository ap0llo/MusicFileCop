using MusicFileCop.Model;
using MusicFileCop.Model.DI;
using MusicFileCop.Model.Metadata;
using MusicFileCop.Model.Rules;
using MusicFileCop.Rules;
using Ninject;
using Ninject.Extensions.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop
{
    class RuleLoader
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
                .Select(checkableType => typeof(IRule<>).MakeGenericType(checkableType))
                .ToArray();

            m_Kernel.Bind(x =>
                x.FromAssembliesMatching(s_RulesAssemblyName)
                .SelectAllClasses()
                .InheritedFromAny(ruleTypes)
                .BindAllInterfaces());
            
        }



        internal IEnumerable<Type> GetCheckableTypes() => GetCheckableTypes(Assembly.GetAssembly(typeof(ModelModule)));

        internal IEnumerable<Type> GetCheckableTypes(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(t => typeof(ICheckable).IsAssignableFrom(t))
                .Where(t => t.IsPublic);
        }


    }
}
