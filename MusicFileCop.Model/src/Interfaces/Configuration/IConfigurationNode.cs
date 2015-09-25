using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Model.Configuration
{
    public interface IConfigurationNode
    {
        T GetValue<T>(string settingsId);   
    }
}
