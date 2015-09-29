﻿using System;
using System.Collections.Generic;

namespace MusicFileCop.Model.Configuration
{
    abstract class ConfigurationNodeBase : IConfigurationNode
    {
        static readonly ISet<Type> s_SupportedTypes = new HashSet<Type>()
        {
            typeof(string),
            typeof(bool),
            typeof(int)
        }; 

        readonly IDictionary<string, object> m_ParsedValues = new Dictionary<string, object>();



        public string GetValue(string name) => GetValue<string>(name);


        public T GetValue<T>(string name)
        {
            if (m_ParsedValues.ContainsKey(name))
            {
                return (T)m_ParsedValues[name];
            }

            string stringValue;
            if (TryGetValue(name, out stringValue))
            {
                var value = (T) Parse<T>(stringValue);
                m_ParsedValues.Add(name, value);
                return value;
            }
            else
            {
                return HandleMissingValue<T>(name);
            }
          
        }


        protected abstract bool TryGetValue(string name, out string value);

        protected abstract T HandleMissingValue<T>(string name);


        protected object Parse<T>(string value)
        {
            EnsureTypeIsSupported<T>();

            if (typeof(T) == typeof(string))
            {
                return value;
            }
            else if (typeof(T) == typeof(bool))
            {
                bool result;
                if (bool.TryParse(value, out result))
                {
                    return result;
                }
                else
                {
                    throw new ArgumentException($"Value '{value}' cannot be parsed to bool");
                }
            }
            else if (typeof(T) == typeof(int))
            {
                int result;

                if (int.TryParse(value, out result))
                {
                    return result;
                }
                else
                {
                    throw new ArgumentException($"Value '{value}' cannot be parsed to int");
                }
            }
            else
            {
                throw new NotSupportedException($"Type '{typeof(T)}' is not supported");
            }
        }


        protected void EnsureTypeIsSupported<T>()
        {
            if (!s_SupportedTypes.Contains(typeof (T)))
            {
                throw new NotSupportedException($"Type '{typeof(T)}' is not supported");
            }
        }


    }
}