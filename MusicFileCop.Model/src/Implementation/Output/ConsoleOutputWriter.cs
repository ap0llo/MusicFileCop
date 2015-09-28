using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicFileCop.Model.Rules;

namespace MusicFileCop.Model.Output
{
    class OutputWriter : IOutputWriter
    {
        public void WriteViolation<T>(IRule<T> violatedRule, T item) where T : ICheckable
        {

            Console.WriteLine($"{item} violates Rule {violatedRule.GetType().Name}");
            

        }
    }
}
