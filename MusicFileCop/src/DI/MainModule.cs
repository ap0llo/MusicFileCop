﻿using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicFileCop.Core.Configuration;

namespace MusicFileCop.DI
{
    public class MainModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<MusicFileCop>().ToSelf();
        }
    }
}
