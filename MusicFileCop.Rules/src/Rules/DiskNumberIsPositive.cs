using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicFileCop.Model.Metadata;
using MusicFileCop.Model.Rules;

namespace MusicFileCop.Rules
{
    public class DiskNumberIsPostiveRule : IRule<IDisk>
    {
        public string Description => "Disk numbers must not have the value 0";

        public bool IsApplicable(IDisk item) => true;

        public bool IsConsistent(IDisk item) => item.DiskNumber > 0;
        
    }
}
