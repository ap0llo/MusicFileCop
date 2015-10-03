using MusicFileCop.Core.Configuration;
using MusicFileCop.Core.FileSystem;
using MusicFileCop.Core.Metadata;
using MusicFileCop.Core.Output;
using MusicFileCop.Model;
using MusicFileCop.Model.Output;
using Ninject.Modules;

namespace MusicFileCop.Core.DI
{
    public class CoreModule : NinjectModule
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
