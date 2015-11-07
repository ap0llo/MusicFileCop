using MusicFileCop.DI;
using MusicFileCop.Rules;
using Ninject;
using Ninject.Extensions.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicFileCop.Core;
using MusicFileCop.Core.DI;

namespace MusicFileCop
{
    class Program
    {       
        static void Main(string[] args)
        {
            //get instance of MusicFileCop using Ninject
            using (var kernel = new StandardKernel(new MainModule(), new CoreModule()))
            {
                // run configurator (configures injections of configuration objects and rules)
                var configurator = kernel.Get<IDynamicConfigurator>();
                configurator.CreateDynamicBindings();

                // run the program
                var program = kernel.Get<MusicFileCop>();
                program.Run(args);
            }
        }
    }
}
