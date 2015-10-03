﻿using MusicFileCop.Core.Configuration;
using MusicFileCop.Core.FileSystem;
using MusicFileCop.Core.Metadata;
using MusicFileCop.Core.Output;
using Ninject;
using Ninject.Modules;
using Ninject.Planning.Bindings;

namespace MusicFileCop.Core.DI
{
    public class CoreModule : NinjectModule
    {
        public override void Load()
        {        
            var metadataMapper = new MetadataMapper();
            var outputWriter = new StructuedOutputWriter(metadataMapper);

            Bind<IConsistencyChecker>().To<ConsistencyChecker>();
            Bind<IMetadataMapper>().ToConstant(metadataMapper);
            Bind<IConfigurationMapper>().To<ConfigurationMapper>().InSingletonScope();
            Bind<IConfigurationLoader>().To<ConfigurationLoader>();
            Bind<IFileSystemLoader>().To<FileSystemLoader>();
            Bind<IMetadataLoader>().To<MetaDataLoader>();
            Bind<IMetadataFactory>().To<MetadataFactory>().InSingletonScope();

            Bind<StructuedOutputWriter>().ToConstant(outputWriter);
            Bind<IOutputWriter<IFile>>().ToConstant(outputWriter);
            Bind<IOutputWriter<IDirectory>>().ToConstant(outputWriter);
            Bind<IOutputWriter<IArtist>>().ToConstant(outputWriter);
            Bind<IOutputWriter<IAlbum>>().ToConstant(outputWriter);
            Bind<IOutputWriter<IDisk>>().ToConstant(outputWriter);
            Bind<IOutputWriter<ITrack>>().ToConstant(outputWriter);


            Bind<IDynamicConfigurator>().To<DynamicConfigurator>();

            var defaultConfigNode = new MutableConfigurationNode();
            Bind<IConfigurationNode>().ToConstant(defaultConfigNode).WhenInjectedExactlyInto<ConfigurationLoader>();
            Bind<IMutableConfigurationNode>().ToConstant(defaultConfigNode).WhenInjectedExactlyInto<DynamicConfigurator>();
        }
    }
}