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

namespace MusicFileCop.Model.DI
{
    public class ModelModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IConsistencyChecker>().To<ConsistencyChecker>();
            this.Bind<IMapper>().To<Mapper>().InSingletonScope();
            this.Bind<IConfigurationLoader>().To<ConfigurationLoader>();
            this.Bind<IFileSystemLoader>().To<FileSystemLoader>();
            this.Bind<IMetadataLoader>().To<MetaDataLoader>();
            this.Bind<IMetadataFactory>().To<MetadataFactory>().InSingletonScope();
            this.Bind<IOutputWriter>().To<OutputWriter>();
            this.Bind<IRuleLoader>().To<RuleLoader>();
        }
    }
}
