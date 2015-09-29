using System;

namespace MusicFileCop.Model.Configuration
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ConfigurationNamespaceAttribute : Attribute
    {

        public string Namespace { get; }


        public ConfigurationNamespaceAttribute(string configurationNamespace)
        {
            if (configurationNamespace == null)
            {
                throw new ArgumentNullException(nameof(configurationNamespace));
            }

            if (String.IsNullOrEmpty(configurationNamespace))
            {
                throw new ArgumentException("Value must not be empty", nameof(configurationNamespace));
            }

            Namespace = configurationNamespace;
        }
    }
}