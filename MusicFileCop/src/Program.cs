using MusicFileCop.DI;
using MusicFileCop.Model.DI;
using MusicFileCop.Model.Metadata;
using MusicFileCop.Model.Rules;
using MusicFileCop.Rules;
using Ninject;
using Ninject.Extensions.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;
using TagLib.Mpeg;

namespace MusicFileCop
{
    class Program
    {
       

        static void Main(string[] args)
        {
            //get instance of Program using ninject
            using (var kernel = new StandardKernel(new MainModule(), new ModelModule()))
            {
                var ruleLoader = kernel.Get<IRuleLoader>();
                ruleLoader.LoadAllRules();

                var program = kernel.Get<MusicFileCop>();
                program.Run(args[0]);
            }
            
        }
    }
}
