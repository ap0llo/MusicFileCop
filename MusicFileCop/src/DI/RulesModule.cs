using Ninject.Modules;
using Ninject.Extensions.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicFileCop.Model.Rules;
using MusicFileCop.Model;
using System.Reflection;
using MusicFileCop.Model.DI;

namespace MusicFileCop.Rules.DI
{
    public class RulesModule : NinjectModule
    {        
        const string s_RulesAssemblyName = "MusicFileCop.Rules";

        public override void Load()
        {
            // get all checkables
            var ruleTypes = GetCheckableTypes()
                .Select(checkableType => typeof(IRule<>).MakeGenericType(checkableType))
                .ToArray();

            this.Kernel.Bind(x =>
                x.FromAssembliesMatching(s_RulesAssemblyName)
                .SelectAllClasses()
                .InheritedFromAny(ruleTypes)
                .BindToSelf());
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
