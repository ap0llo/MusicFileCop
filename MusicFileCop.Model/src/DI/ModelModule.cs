using MusicFileCop.Model.Configuration;
using MusicFileCop.Model.FileSystem;
using MusicFileCop.Model.Metadata;
using MusicFileCop.Model.Output;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicFileCop.Model.Rules;
using Ninject.Extensions.Conventions;

namespace MusicFileCop.Model.DI
{
    public class ModelModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IConsistencyChecker>().To<ConsistencyChecker>();
            this.Bind<IMetadataMapper>().To<MetadataMapper>().InSingletonScope();
            this.Bind<IConfigurationMapper>().To<ConfigurationMapper>().InSingletonScope();
            this.Bind<IConfigurationLoader>().To<ConfigurationLoader>();
            this.Bind<IFileSystemLoader>().To<FileSystemLoader>();
            this.Bind<IMetadataLoader>().To<MetaDataLoader>();
            this.Bind<IMetadataFactory>().To<MetadataFactory>().InSingletonScope();
            
            var consoleOutputWriter = new ConsoleOutputWriter();
            this.Bind(typeof(IOutputWriter<>)).ToConstant(consoleOutputWriter);
            
            this.Bind<IDynamicConfigurator>().To<DynamicConfigurator>();

            var defaultConfigNode = new MutableConfigurationNode();            
            this.Bind<IConfigurationNode>().ToConstant(defaultConfigNode).WhenInjectedExactlyInto<ConfigurationLoader>();
            this.Bind<IMutableConfigurationNode>().ToConstant(defaultConfigNode).WhenInjectedExactlyInto<DynamicConfigurator>();
        }

    }
}
