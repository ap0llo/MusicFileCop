using System;
using System.Collections.Generic;

namespace MusicFileCop.Core.Configuration
{
    /// <summary>
    ///     Base class for implementations of <see cref="IConfigurationNode" />
    /// </summary>
    internal abstract class ConfigurationNodeBase : IConfigurationNode
    {
        static readonly ISet<Type> s_SupportedTypes = new HashSet<Type>
        {
            typeof (string),
            typeof (bool),
            typeof (int)
        };

        // cache for parsed values
        readonly IDictionary<string, object> m_ParsedValues = new Dictionary<string, object>();


        public abstract IEnumerable<string> Names { get; }


        public string GetValue(string name) => GetValue<string>(name);

        public T GetValue<T>(string name)
        {
            T value;
            if (TryGetValue(name, out value))
            {
                return value;
            }
            return HandleMissingValue<T>(name);
        }

        public bool TryGetValue<T>(string name, out T value)
        {
            if (m_ParsedValues.ContainsKey(name))
            {
                value = (T) m_ParsedValues[name];
                return true;
            }

            string stringValue;
            if (TryGetValue(name, out stringValue))
            {
                value = (T) Parse<T>(stringValue);
                m_ParsedValues.Add(name, value);
                return true;
            }
            value = default(T);
            return false;
        }

        public abstract bool TryGetValue(string name, out string value);

        /// <summary>
        /// Called if a value cannot be located for the node
        /// </summary>
        protected abstract T HandleMissingValue<T>(string name);

        protected object Parse<T>(string value)
        {
            EnsureTypeIsSupported<T>();

            if (typeof (T) == typeof (string))
            {
                return value;
            }
            if (typeof (T) == typeof (bool))
            {
                bool result;
                if (bool.TryParse(value, out result))
                {
                    return result;
                }
                throw new ArgumentException($"Value '{value}' cannot be parsed to bool");
            }
            if (typeof (T) == typeof (int))
            {
                int result;

                if (int.TryParse(value, out result))
                {
                    return result;
                }
                throw new ArgumentException($"Value '{value}' cannot be parsed to int");
            }
            if (typeof (T).IsEnum)
            {
                try
                {
                    return (T) Enum.Parse(typeof (T), value, true);
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException($"Value '{value}' cannot be parsed to enum type {typeof (T)}", ex);
                }
            }
            throw new NotSupportedException($"Type '{typeof (T)}' is not supported");
        }

        protected void EnsureTypeIsSupported<T>()
        {
            if (!s_SupportedTypes.Contains(typeof (T)) && !typeof (T).IsEnum)
            {
                throw new NotSupportedException($"Type '{typeof (T)}' is not supported");
            }
        }
    }
}