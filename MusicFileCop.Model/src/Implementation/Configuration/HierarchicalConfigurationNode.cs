using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicFileCop.Model.Configuration
{
    class HierarchicalConfigurationNode : IConfigurationNode
    {
        readonly IConfigurationNode m_ParentNode;
        readonly IDictionary<string, string> m_Values;
        readonly IDictionary<string, object> m_ParsedValues = new Dictionary<string, object>();


        public HierarchicalConfigurationNode(IConfigurationNode parentNode, IDictionary<string, string> values) 
        {                      
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            this.m_ParentNode = parentNode;
            this.m_Values = values;
        }


        public T GetValue<T>(string name)
        {
            if(m_ParsedValues.ContainsKey(name))
            {
                return (T) m_ParsedValues[name];
            }
            
            if(m_Values.ContainsKey(name))
            {
                var stringValue = m_Values[name];
                var value = (T) Parse<T>(name);
                m_ParsedValues.Add(name, value);                
                return value;
            }                
            else if(m_ParentNode != null)
            {
                return m_ParentNode.GetValue<T>(name);
            }
            else
            {
                throw new KeyNotFoundException($"No value found for name '{name}'");
            }
        }


        private object Parse<T>(string value)
        {
            if(typeof(T) == typeof(string))
            {
                return value;
            }
            else if(typeof(T) == typeof(bool))
            {
                return bool.Parse(value);
            }
            else if(typeof(T) == typeof(int))
            {
                return int.Parse(value);
            }
            else
            {
                throw new NotSupportedException($"Type '{typeof(T)}' is not supported");
            }
        }

    }
}
