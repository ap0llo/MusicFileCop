using System;

namespace MusicFileCop.Core.Configuration
{
    /// <summary>
    /// Attribute to indicate a class that it only uses settings from the specified namespace
    /// This will be used to configure dependency injection to inject a specific configuration mapper into classes with this attribure
    /// </summary>
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