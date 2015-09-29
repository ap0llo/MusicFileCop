using System;
using System.Collections.Generic;

namespace MusicFileCop.Model.Configuration
{
    //TODO: Unit test (remember to test if names are handled case-invariant)
    class MutableConfigurationNode : ConfigurationNodeBase, IMutableConfigurationNode
    {
        readonly IDictionary<string, string> m_Values = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);



        public void AddValue<T>(string name, T value)
        {
            EnsureTypeIsSupported<T>();

            if (m_Values.ContainsKey(name))
            {
                m_Values[name] = value.ToString();
            }
            else
            {
                m_Values.Add(name, value.ToString());
            }
        }



        protected override bool TryGetValue(string name, out string value) => m_Values.TryGetValue(name, out value);

        protected override T HandleMissingValue<T>(string name)
        {
            throw new KeyNotFoundException($"No value for key '{name}' found");
        }
    }
}