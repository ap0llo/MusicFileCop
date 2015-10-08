using MusicFileCop.Core.Configuration;
using MusicFileCop.Core.FileSystem;
using MusicFileCop.Core.Metadata;
using MusicFileCop.Core.Output;
using MusicFileCop.Core.Rules;
using Ninject.Modules;

namespace MusicFileCop.Core.DI
{
    public class CoreModule : NinjectModule
    {
        public override void Load()
        {        
            // there must only be a single metadata mapper and output writer
            var metadataMapper = new MetadataMapper();
            var outputWriter = new StructuedTextOutputWriter(metadataMapper);

            Bind<IMetadataMapper>().ToConstant(metadataMapper);

            Bind<IConsistencyChecker>().To<ConsistencyChecker>();
            Bind<IConfigurationMapper>().To<ConfigurationMapper>().InSingletonScope();
            Bind<IConfigurationLoader>().To<ConfigurationLoader>();
            Bind<IFileSystemLoader>().To<FileSystemLoader>();
            Bind<IMetadataLoader>().To<MetaDataLoader>();
            Bind<IMetadataFactory>().To<MetadataFactory>().InSingletonScope();
            Bind<IConfigurationWriter>().To<ConfigurationWriter>();
            Bind<IRuleSet>().To<RuleSet>().InSingletonScope();

            Bind<StructuedTextOutputWriter>().ToConstant(outputWriter);
            Bind<ITextOutputWriter>().ToConstant(outputWriter);
            Bind<IOutputWriter<IFile>>().ToConstant(outputWriter);
            Bind<IOutputWriter<IDirectory>>().ToConstant(outputWriter);
            Bind<IOutputWriter<IArtist>>().ToConstant(outputWriter);
            Bind<IOutputWriter<IAlbum>>().ToConstant(outputWriter);
            Bind<IOutputWriter<IDisk>>().ToConstant(outputWriter);
            Bind<IOutputWriter<ITrack>>().ToConstant(outputWriter);


            Bind<IDynamicConfigurator>().To<DynamicConfigurator>();

            // default configuraiton node (only a single instance)
            var mutableDefaultConfigNode = new MutableConfigurationNode();
            var defaultConfigNode = new DefaultConfigurationNode(mutableDefaultConfigNode);            

            Bind<IMutableConfigurationNode>().ToConstant(mutableDefaultConfigNode).WhenInjectedExactlyInto<DynamicConfigurator>();
            Bind<IDefaultConfigurationNode>().ToConstant(defaultConfigNode);
        }
    }
}
