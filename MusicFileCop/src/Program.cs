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
using TagLib;
using TagLib.Mpeg;

namespace MusicFileCop
{
    class Program
    {
       

        static void Main(string[] args)
        {
            //get instance of Program using ninject
            using (var kernel = new StandardKernel(new MainModule(), new CoreModule()))
            {
                var configurator = kernel.Get<IDynamicConfigurator>();
                configurator.CreateDynamicBindings();

                var program = kernel.Get<MusicFileCop>();
                program.Run(args[0]);

            }

            Console.WriteLine();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();

            
        }
    }
}
