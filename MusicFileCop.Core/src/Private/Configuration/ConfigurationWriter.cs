using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MusicFileCop.Core.Configuration
{
    /// <summary>
    /// Implementation of <see cref="IConfigurationWriter"/>
    /// </summary>
    internal class ConfigurationWriter : IConfigurationWriter
    {

        public void WriteConfiguration(IConfigurationNode configuration, string outputPath)
        {
            // create root config json object
            var json = new JObject();

            //serialize all settings of the node
            foreach (var name in configuration.Names)
            {
                SerializeSetting(name, configuration, json);
            }

            // write json to file
            File.WriteAllText(outputPath, json.ToString(Formatting.Indented));
        }


        void SerializeSetting(string name, IConfigurationNode configurationNode, JObject json)
        {
            // name  has sub-key
            if (name.Contains(":"))
            {
                var prefix = name.Split(':').First();

                // get or create the inner json object for the sub-keys
                var innerJson = json[prefix] as JObject;
                if (innerJson == null)
                {
                    innerJson = new JObject();
                    json[prefix] = innerJson;
                }

                // serialize setting (remove prefix from name)
                var newName = name.Remove(0, prefix.Length + 1);
                SerializeSetting(newName, new PrefixConfigurationNode(configurationNode, prefix), innerJson);
            }
            // no subkeys => write setting to current json object
            else
            {
                var value = configurationNode.GetValue(name);
                json[name] = value;
            }
        }

    }
}